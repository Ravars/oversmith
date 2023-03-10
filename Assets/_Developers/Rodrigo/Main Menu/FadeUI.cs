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
    [Space(20)]
    [Header("Events")]
    public UnityEvent onFadeInEnd;
    public UnityEvent onFadeOutEnd;

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
            FadeIn();
            if(!isFadingIn) onFadeInEnd.Invoke();
        }
        else if (isFadingOut)
        {
            FadeOut();
            if(!isFadingOut) onFadeOutEnd.Invoke();
        }
    }

    public void BeginFadeIn()
    {
        isFadingIn = true;
    }

    public void BeginFadeOut()
    {
        isFadingOut = true;
    }

    private void FadeIn()
    {
        if (counter >= duration)
        {
            counter = 0f;
            isFadingIn = false;
        }
        else
        {
            canvasGr.alpha = Mathf.Lerp(0, 1, counter / duration);

            counter += Time.deltaTime;
        }
    }

    private void FadeOut()
    {
        if (counter >= duration)
        {
            counter = 0f;
            isFadingOut = false;
        }
        else
        {
            canvasGr.alpha = Mathf.Lerp(1, 0, counter / duration);

            counter += Time.deltaTime;
        }
    }
}
