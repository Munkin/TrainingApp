// <copyright file="Fader.cs" company="Up Up Down Studios">
// Copyright (c) 2018 All Rights Reserved
// </copyright>
// <summary>Class that hadle fade events.</summary>

using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Fader : MonoBehaviour {

    #region Properties

    [SerializeField]
    private bool enableConsoleLog;

    [Space(10f)]

    public float screenFadeDuration;
    [Tooltip("0, 1 Value")] [Range(0,1)]
    public float screenFadeEndValue;
    [Tooltip("0, 1 Value")] [Range(0, 1)] [SerializeField]
    private float alphaInitialValue;

    [Space(10f)]

    [SerializeField]
    private Color toggleColor;

    // Cached Components
    private float cachedTextAlphaInitialValue;
    private float cachedVideoAlphaInitialValue;

    private Image[] images;
    private Text[] texts;
    private Text cachedText;

    private Material video;

    // Coroutines
    private IEnumerator videoFade;

    // Singleton!
    public static Fader Singleton
    {
        get; private set;
    }

    #endregion

    #region Unity functions

    private void Awake()
    {
        if (Singleton != null)
            DestroyImmediate(gameObject);
        else
            Singleton = this;
    }

    private void Start()
    {
        Setup();
        Suscribe();
    }

    #endregion

    #region Class functions

    private void Setup()
    {
        // Is the screen fade end value between 1 and 0 ?
        screenFadeEndValue = (screenFadeEndValue < 0 || screenFadeEndValue > 1) ? Mathf.Clamp01(screenFadeEndValue) : screenFadeEndValue;
        // Is the alpha initial value between 1 and 0 ?
        alphaInitialValue = (alphaInitialValue < 0 || alphaInitialValue > 1) ? Mathf.Clamp01(alphaInitialValue) : alphaInitialValue;
    }

    private void Suscribe()
    {
        // Fade events
        Observer.Singleton.onDataScreenFade += FadeScreen;
        Observer.Singleton.onExerciseDataScreenFade += FadeScreen;
        Observer.Singleton.onTestEndScreenFade += FadeScreen;
        // Training fade events
        Observer.Singleton.onWarmingUpScreenEndFade += FadeScreen;
        Observer.Singleton.onTrainingScreenEndFade += FadeScreen;
        Observer.Singleton.onStretchingScreenEndFade += FadeScreen;
        // Button fade events
        Observer.Singleton.onButtonFadeCallback += ResetButtonAlpha;
    }

    public void FadeScreen(GameObject parent, float fadeDuration = 1.0f, float fadeEndValue = 1.0f)
    {
        if (enableConsoleLog)
            Debug.Log("Fader :: FadeScreen");

        // Data temp allocation
        float tempFadeDuration = screenFadeDuration;
        float tempFadeEndValue = screenFadeEndValue;

        // Comparing parameters with class values
        if (screenFadeDuration != fadeDuration)
            screenFadeDuration = fadeDuration;

        if (screenFadeEndValue != fadeEndValue)
            screenFadeEndValue = fadeEndValue;

        // Getting components from screen parent
        images = parent.GetComponentsInChildren<Image>();
        texts = parent.GetComponentsInChildren<Text>();

        try
        {
            video = parent.GetComponentInChildren<RawImage>().material;
        }
        catch (Exception)
        {
            Debug.LogWarning("No RawImage attached to this object.");
        }

        CheckAlphaStatus(parent);

        // Control variable
        bool isFirstTimeSuscribing = true;

        // Is there some image on the screen ?
        if (images != null && images.Length > 0)
        {
            // Tweens the alpha value for all the images of the screen
            foreach (Image image in images)
            {
                // Is the image a non fade element ?
                if (!image.CompareTag("NonFadeElement") && !image.CompareTag("MaterialButtonLayer"))
                {
                    // Suscribe only ones in the screen fade callback
                    if (isFirstTimeSuscribing)
                    {
                        // Is the text a toggle element ?
                        if (image.CompareTag("Toggle"))
                            image.DOFade(toggleColor.a, screenFadeDuration).OnComplete(OnScreenFadeCallback);
                        else
                            image.DOFade(screenFadeEndValue, screenFadeDuration).OnComplete(OnScreenFadeCallback);

                        isFirstTimeSuscribing = false;
                    }
                    else
                    {
                        // Is the text a toggle element ?
                        if (image.CompareTag("Toggle"))
                            image.DOFade(toggleColor.a, screenFadeDuration);
                        else
                            image.DOFade(screenFadeEndValue, screenFadeDuration);
                    }
                }
            }
        }

        // Is there some text on the screen ?
        if (texts != null && texts.Length > 0)
        {
            // Tweens the alpha value for all the texts of the screen
            foreach (Text text in texts)
            {
                // Suscribe only ones in the screen fade callback
                if (isFirstTimeSuscribing)
                {
                    // Is the text a toggle element ?
                    if (text.CompareTag("Toggle"))
                        text.DOFade(toggleColor.a, screenFadeDuration).OnComplete(OnScreenFadeCallback);
                    else
                        text.DOFade(screenFadeEndValue, screenFadeDuration).OnComplete(OnScreenFadeCallback);

                    isFirstTimeSuscribing = false;
                }
                else
                {
                    // Is the text a toggle element ?
                    if (text.CompareTag("Toggle"))
                        text.DOFade(toggleColor.a, screenFadeDuration);
                    else
                        text.DOFade(screenFadeEndValue, screenFadeDuration);
                }                    
            }
        }

        // Is there some video atached to the screen ?
        if (video != null)
            ExecuteVideoFade();

        // Setting class values again
        screenFadeDuration = tempFadeDuration;
        screenFadeEndValue = tempFadeEndValue;
    }

    public void FadeInButton(GameObject button)
    {
        if (enableConsoleLog)
            Debug.Log("Fader :: FadeInButton");

        cachedText = button.GetComponentInChildren<Text>();

        cachedTextAlphaInitialValue = cachedText.color.a;

        // Reset to the oposite alpha value of the object to do the fade correct.
        cachedText.color = new Color(
            cachedText.color.r,
            cachedText.color.g,
            cachedText.color.b,
            1 - Mathf.Clamp01(screenFadeEndValue));

        cachedText.DOFade(Mathf.Clamp01(screenFadeEndValue), screenFadeDuration).OnComplete(OnButtonFadeCallback);
    }

    public void FadeOutButton(GameObject button)
    {
        if (enableConsoleLog)
            Debug.Log("Fader :: FadeOutButton");

        cachedText = button.GetComponentInChildren<Text>();

        cachedTextAlphaInitialValue = cachedText.color.a;

        cachedText.DOFade(1 - Mathf.Clamp01(screenFadeEndValue), screenFadeDuration).OnComplete(OnButtonFadeCallback);
    }

    private void CheckAlphaStatus(GameObject parent)
    {
        // Is there some image on the screen ?
        if (images != null && images.Length > 0)
        {
            // Checking values for images
            foreach (Image image in images)
            {
                // Is the image a non fade element ?
                if (!image.CompareTag("NonFadeElement") && !image.CompareTag("MaterialButtonLayer"))
                {
                    if (image.color.a != alphaInitialValue)
                        image.color = new Color(
                            image.color.r,
                            image.color.g,
                            image.color.b,
                            alphaInitialValue);
                }
            }
        }

        // Is there some text on the screen ?
        if (texts != null && texts.Length > 0)
        {
            // Checking values for texts
            foreach (Text text in texts)
            {
                if (text.color.a != alphaInitialValue)
                    text.color = new Color(
                        text.color.r,
                        text.color.g,
                        text.color.b,
                        alphaInitialValue);
            }
        }

        // Is there some video atached to the screen ?
        if (video != null)
        {
            cachedVideoAlphaInitialValue = video.GetFloat("_Alpha");

            if (cachedVideoAlphaInitialValue != alphaInitialValue)
                video.SetFloat("_Alpha", alphaInitialValue);
        }
    }

    private void OnScreenFadeCallback()
    {
        Observer.Singleton.OnScreenFadeCallback();
    }

    private void OnButtonFadeCallback()
    {
        Observer.Singleton.OnButtonFadeCallback();
    }

    private void ResetButtonAlpha()
    {
        // Reseting alpha
        cachedText.color = new Color(
            cachedText.color.r,
            cachedText.color.g,
            cachedText.color.b,
            cachedTextAlphaInitialValue);
    }

    private void ExecuteVideoFade()
    {
        // Coroutine execution!
        StopVideoFade();

        videoFade = VideoFade();

        StartCoroutine(videoFade);
    }

    private void StopVideoFade()
    {
        if (videoFade != null)
            StopCoroutine(videoFade);
    }

    #endregion

    #region Coroutines

    private IEnumerator VideoFade()
    {
        // Time Control
        float fadeTime = 0;
        float fadeValue;

        while (fadeTime < screenFadeDuration)
        {
            fadeValue = Mathf.Clamp01(fadeTime / screenFadeDuration) * screenFadeEndValue;

            video.SetFloat("_Alpha", fadeValue);

            fadeTime += Time.deltaTime;

            yield return null;
        }

        video = null;
    }

    #endregion
}
