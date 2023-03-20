using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class FadeUI : MonoBehaviour
{
    [Header("Fade Control")]
    [Range(0, 5)]
    public float duration = 0.6f;
    private float counter = 0f;
    private bool isFadingIn = false;
    private bool isFadingOut = false;
    private CanvasGroup canvasGr;
    [Space(10)]
    [Header("Events")]
    public UnityEvent onFadeInEnd;
    public UnityEvent onFadeOutEnd;
    private delegate void OnFadeHandler();

    // Start is called before the first frame update
    void Start()
    {
        canvasGr = GetComponent<CanvasGroup>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isFadingIn)
        {
            _FadeIn();
            if(!isFadingIn) onFadeInEnd.Invoke();
        }
        else if (isFadingOut)
        {
            _FadeOut();
            if(!isFadingOut) onFadeOutEnd.Invoke();
        }
    }

    public void BeginFadeIn()
    {
        isFadingOut = false;
        isFadingIn = true;
    }

    public void FadeIn(float time, UnityAction ender)
    {
        duration = time;
        onFadeInEnd.RemoveAllListeners();
        if(ender != null) onFadeInEnd.AddListener(ender);
        isFadingOut = false;
        isFadingIn = true;
    }
    public void FadeIn(float time) { FadeIn(time, null); }
    public void FadeIn(UnityAction ender) { FadeIn(duration, ender); }
    public void FadeIn() { FadeIn(duration, null); }

    public void BeginFadeOut()
    {
        isFadingIn = false;
        isFadingOut = true;
    }

    public void FadeOut(float time, UnityAction ender)
    {
        duration = time;
        onFadeOutEnd.RemoveAllListeners();
        if (ender != null) onFadeOutEnd.AddListener(ender);
        isFadingIn = false;
        isFadingOut = true;
    }
    public void FadeOut(float time) { FadeOut(time, null); }
    public void FadeOut(UnityAction ender) { FadeOut(duration, ender); }
    public void FadeOut() { FadeOut(duration, null); }

    private void _FadeIn()
    {
        if (counter >= duration)
        {
            counter = 0f;
            isFadingIn = false;
            canvasGr.alpha = 1;
        }
        else
        {
            canvasGr.alpha = Mathf.Lerp(0, 1, counter / duration);

            counter += Time.deltaTime;
        }
    }

    private void _FadeOut()
    {
        if (counter >= duration)
        {
            counter = 0f;
            isFadingOut = false;
            canvasGr.alpha = 0;
        }
        else
        {
            canvasGr.alpha = Mathf.Lerp(1, 0, counter / duration);

            counter += Time.deltaTime;
        }
    }
}
