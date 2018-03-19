using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Fader : MonoBehaviour {

    #region Properties

    [SerializeField]
    private bool enableConsoleLog;

    [Space(10f)]

    [SerializeField]
    private float screenFadeDuration;
    [SerializeField]
    private float screenFadeEndValue;
    [SerializeField]
    private float alphaTargetValue;

    // Hidden
    public Action onFadeCallback;

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
            DestroyImmediate(Singleton);
        else
            Singleton = this;
    }

    #endregion

    #region Class functions

    private void FadeScreen(GameObject parent)
    {
        // Getting components from screen parent
        Image[] images = parent.GetComponentsInChildren<Image>();
        Text[] texts = parent.GetComponentsInChildren<Text>();
        Button[] buttons = parent.GetComponentsInChildren<Button>();

        CheckAlphaStatus(parent, images, texts, buttons);

        // Tweens the alpha value for all the images of the screen
        foreach (Image image in images)
        {
            image.DOFade(screenFadeEndValue, screenFadeDuration).OnComplete(OnFadeCallback);
        }

        // Tweens the alpha value for all the texts of the screen
        foreach (Text text in texts)
        {
            text.DOFade(screenFadeEndValue, screenFadeDuration).OnComplete(OnFadeCallback);
        }

        // Tweens the alpha value for all the buttons of the screen
        foreach (Button button in buttons)
        {
            button.image.DOFade(screenFadeEndValue, screenFadeDuration).OnComplete(OnFadeCallback);
        }
    }

    private void CheckAlphaStatus(GameObject parent, Image[] images, Text[] texts, Button[] buttons)
    {
        // Checking values for images
        foreach (Image image in images)
        {
            if (image.color.a != alphaTargetValue)
                image.color = new Color(
                    image.color.r,
                    image.color.g,
                    image.color.b,
                    alphaTargetValue);
        }

        // Checking values for texts
        foreach (Text text in texts)
        {
            if (text.color.a != alphaTargetValue)
                text.color = new Color(
                    text.color.r,
                    text.color.g,
                    text.color.b,
                    alphaTargetValue);
        }

        // Checking values for buttons
        foreach (Button button in buttons)
        {
            if (button.image.color.a != alphaTargetValue)
                button.image.color = new Color(
                    button.image.color.r,
                    button.image.color.g,
                    button.image.color.b,
                    alphaTargetValue);
        }
    }

    private void OnFadeCallback()
    {
        if (enableConsoleLog)
            Debug.Log("Fader :: OnCallback");

        if (onFadeCallback != null)
            onFadeCallback();
    }

    #endregion
}
