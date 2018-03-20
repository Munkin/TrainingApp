﻿// <copyright file="Fader.cs" company="Up Up Down Studios">
// Copyright (c) 2018 All Rights Reserved
// </copyright>
// <summary>Class that hadle fade events.</summary>

using System;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Fader : MonoBehaviour {

    #region Properties

    [SerializeField]
    private bool enableConsoleLog;

    [Space(10f)]

    public float screenFadeDuration;
    [Tooltip("0, 1 Value")]
    public float screenFadeEndValue;
    [Tooltip("0, 1 Value")] [SerializeField]
    private float alphaInitialValue;

    // Events
    public Action onFadeCallback;

    // Cached Components
    private float cachedTextAlphaInitialValue;

    private Image[] images;
    private Text[] texts;

    private Text cachedText;

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
        Suscribe();

        screenFadeEndValue = (screenFadeEndValue < 0 || screenFadeEndValue > 1) ? Mathf.Clamp01(screenFadeEndValue) : screenFadeEndValue;

        alphaInitialValue = (alphaInitialValue < 0 || alphaInitialValue > 1) ? Mathf.Clamp01(alphaInitialValue) : alphaInitialValue;
    }

    #endregion

    #region Class functions

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
    }

    public void FadeScreen(GameObject parent, float fadeDuration = 1.0f, float fadeEndValue = 1.0f)
    {
        if (enableConsoleLog)
            Debug.Log("Fader :: FadeScreen");

        float tempFadeDuration = screenFadeDuration;
        float tempFadeEndValue = screenFadeEndValue;

        // Comparing parameters
        if (fadeDuration != screenFadeDuration)
            screenFadeDuration = fadeDuration;
        if (fadeEndValue != screenFadeEndValue)
            screenFadeEndValue = fadeEndValue;

        // Getting components from screen parent
        images = parent.GetComponentsInChildren<Image>();
        texts = parent.GetComponentsInChildren<Text>();

        CheckAlphaStatus(parent);

        // Tweens the alpha value for all the images of the screen
        foreach (Image image in images)
        {
            // Is the image a non fade element ?
            if (!image.CompareTag("NonFadeElement") && !image.CompareTag("MaterialButtonLayer"))
                image.DOFade(screenFadeEndValue, screenFadeDuration).OnComplete(OnFadeCallback);
        }

        // Tweens the alpha value for all the texts of the screen
        foreach (Text text in texts)
        {
            text.DOFade(screenFadeEndValue, screenFadeDuration).OnComplete(OnFadeCallback);
        }

        // Setting star values
        screenFadeDuration = tempFadeDuration;
        screenFadeEndValue = tempFadeEndValue;
    }

    public void FadeInButton(GameObject button)
    {
        if (enableConsoleLog)
            Debug.Log("Fader :: FadeInButton");

        cachedText = button.GetComponentInChildren<Text>();

        cachedTextAlphaInitialValue = cachedText.color.a;

        cachedText.color = new Color(
            cachedText.color.r,
            cachedText.color.g,
            cachedText.color.b,
            1 - Mathf.Clamp01(screenFadeEndValue));

        cachedText.DOFade(Mathf.Clamp01(screenFadeEndValue), screenFadeDuration).OnComplete(OnFadeButtonCallback);
    }

    public void FadeOutButton(GameObject button)
    {
        if (enableConsoleLog)
            Debug.Log("Fader :: FadeOutButton");

        cachedText = button.GetComponentInChildren<Text>();

        cachedTextAlphaInitialValue = cachedText.color.a;

        cachedText.DOFade(1 - Mathf.Clamp01(screenFadeEndValue), screenFadeDuration).OnComplete(OnFadeButtonCallback);
    }

    private void CheckAlphaStatus(GameObject parent)
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

    private void OnFadeCallback()
    {
        if (enableConsoleLog)
            Debug.Log("Fader :: OnFadeCallback");

        // Event call!
        if (onFadeCallback != null)
            onFadeCallback();
    }

    private void OnFadeButtonCallback()
    {
        if (enableConsoleLog)
            Debug.Log("Fader :: OnFadeButtonCallback");

        // Reseting alpha
        cachedText.color = new Color(
            cachedText.color.r,
            cachedText.color.g,
            cachedText.color.b,
            cachedTextAlphaInitialValue);
    }
    
    #endregion
}
