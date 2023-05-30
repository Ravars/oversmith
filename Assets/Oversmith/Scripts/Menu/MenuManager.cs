using System.Collections.Generic;
using UnityEngine;
using Oversmith.Scripts.Managers;

namespace Oversmith.Scripts.Menu
{
    [AddComponentMenu("Menu/Manage/Menu Manager"),
    RequireComponent(typeof(Fader))]
    public class MenuManager : MonoBehaviour
    {
        [SerializeField, Tooltip("List of canvases that compose the menu.\n"+
        "The first one is the initial active canvas.")]
        private List<MenuCanvas> canvases = new();
        ///<summary>Stack of canvases in order they were called.<br/>Values are canvas' indexes.</summary>
        private Stack<int> history = new();
        private int current = 0;
        
        // TRANSITION EFFECT
        [SerializeField] private Fader fader;
        private FadeHandler handler;

        private void Start()
        {
            foreach (MenuCanvas canvas in canvases)
            {
                canvas.OnReturn.AddListener(ReturnCanvas);
                canvas.enabled = false;
            }
            canvases[0].enabled = true;
            fader.FadeIn(canvases[0], canvases[0].Begin);
        }

        #region TRANSITION
        ///<summary>
        /// Change between canvas using a transition effect.
        ///</summary>
        ///<param name="i">Index of the canvas in canvases list.</param>
        public void ChangeCanvas(int i)
        {
            if (IsHandlerRun(handler)) return;
            if (i < 0 || i >= canvases.Count)
                throw new System.ArgumentOutOfRangeException("i", i, "Out of range of the canvases list.");
            handler = fader.FadeOut(canvases[current], () =>
            {
                canvases[current].enabled = false;
                history.Push(current);
                current = i;
                canvases[i].enabled = true;
                
                handler = fader.FadeIn(canvases[i], canvases[i].Begin);
            });
        }
        ///<summary>
        /// Change between canvas using a transition effect.
        ///</summary>
        ///<param name="canvas">Canvas to be open (must exist in the canvases list).</param>
        public void ChangeCanvas(MenuCanvas canvas)
        {
            for (int i = 0; i < canvases.Count; i++)
            {
                if (canvas == canvases[i]) {
                    ChangeCanvas(i);
                    return;
                }
            }
            throw new System.ArgumentOutOfRangeException("canvas", canvas, "Canvas is not in the canvases list.");
        }

        ///<summary>
        /// Return to the previous canvas using the transition effect.
        ///</summary>
        public void ReturnCanvas(System.Action action)
        {
            if (IsHandlerRun(handler)) return;
            handler = fader.FadeOut(canvases[current], () =>
            {
                canvases[current].enabled = false;
                current = history.Pop();
                if (action != null) action();
                canvases[current].enabled = true;
                handler = fader.FadeIn(canvases[current], canvases[current].Begin);
            });
        }

        private bool IsHandlerRun (FadeHandler handler)
        {
            if (handler == null) return false;
            return !handler.isComplete;
        }
        #endregion
        
        public void LoadGame() => GameManager.Instance.LoadGameLevel(1);
        public void TryQuitGame() => GameManager.Instance.QuitGame();
    }
}


