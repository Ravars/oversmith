using System.Collections.Generic;
using UnityEngine;

namespace MadSmith.Scripts.Menu.Old
{
    ///<summary>
    /// Manage fade in/out of canvas group.
    ///</summary>
    [AddComponentMenu("Menu/Effects/Fader")]
    public class Fader : MonoBehaviour
    {
        [SerializeField, Tooltip("Duration of fade in/out effect in seconds.")]
        private float duration = 0.2f;
        ///<value>
        /// Duration of fade in/out effect in seconds.
        ///</value>
        public float Duration { 
            get => Mathf.Max(0f, duration);
            set { duration = Mathf.Max(0f, value); }
        }

        // Handlers under fade effect.
        private List<FadeHandler> handlers;

        private void Awake()
        {
            handlers = new List<FadeHandler>();
        }

        private void Update()
        {
            if(handlers.Count == 0) return; // There are no canvas fading in/out.

            List<FadeHandler> completed = new();
            foreach (FadeHandler handler in handlers.ToArray())
            {
                if(handler.Update())        // Update fade effect in canvas.
                    handlers.Remove(handler); // If fading is complete, remove it.
            }
        }

        ///<summary>
        /// Begin the effect of fading menu canvas in. When complete, it executes the action.
        ///</summary>
        ///<param name="canvas">The menu canvas wich receive the fading effect.</param>
        ///<param name="action">The action that is executed after fading effect.</param>
        public FadeHandler FadeIn (MenuCanvas canvas, System.Action action)
        {
            return _fade(canvas, action, 1);
        }
        ///<summary>
        /// Begin the effect of fading menu canvas in.
        ///</summary>
        ///<param name="canvas">The menu canvas wich receive the fading effect.</param>
        public FadeHandler FadeIn (MenuCanvas canvas) => FadeIn(canvas, null);

        ///<summary>
        /// Begin the effect of fading menu canvas out. When complete, it executes the action.
        ///</summary>
        ///<param name="canvas">The menu canvas wich receive the fading effect.</param>
        ///<param name="action">The action that is executed after fading effect.</param>
        public FadeHandler FadeOut(MenuCanvas canvas, System.Action action)
        {
            return _fade(canvas, action, -1);
        }
        ///<summary>
        /// Begin the effect of fading menu canvas out.
        ///</summary>
        ///<param name="canvas">The menu canvas wich receive the fading effect.</param>
        public FadeHandler FadeOut (MenuCanvas canvas) => FadeOut(canvas, null);

        protected FadeHandler _fade(MenuCanvas canvas, System.Action action, int direction)
        {
            CanvasGroup group = canvas.GetComponent<CanvasGroup>();
            if(group == null)
            {
                //canvas.gameObject.SetActive(true);
                if(action != null) action();
                return null;
            }

            FadeHandler handler = new FadeHandler(Duration, direction, group, action);
            handlers.Add(handler);
            return handler;
        }
    }

    ///<summary>
    /// Handler of fade in/out effect.
    ///</summary>
    public class FadeHandler
    {
        ///<value>
        /// Whether the effect of fade in/out can run.
        ///</value>
        public bool enable = true;
        public bool isComplete = false;
        private float duration;
        ///<value>
        /// Duration of fade in/out effect in seconds.
        ///</value>
        public float Duration { 
            get => Mathf.Max(0f, duration);
            set { duration = Mathf.Max(0f, value); }
        }
        private float step;
        private int direction;
        ///<summary>
        /// Represents whether it is fade in or fade out.
        ///</summary>
        ///<value>
        /// 1 is fade in. <br/>
        /// -1 is fade out.
        ///</value>
        public int Direction {
            get => direction;
            set { direction = value < 0 ? -1 : 1; }
        }

        private CanvasGroup group;
        private System.Action action;

        public FadeHandler(float duration, int direction, CanvasGroup group, System.Action action)
        {
            Duration = duration;
            Direction = direction;
            this.group = group;
            this.action = action;
            step = 1 / Duration;
        }

        ///<summary>
        /// Update the effect of fade in/out.
        ///</summary>
        ///<returns>Whether the effect is completed.</returns>
        public bool Update()
        {
            if (!enable) return false;
            group.alpha += Direction * Time.deltaTime * step;
            group.alpha = Mathf.Clamp(group.alpha, 0, 1);
            if (group.alpha != 0 && group.alpha != 1) return false;
            if(action != null) action();
            isComplete = true;
            return true;
        }
    }
}

