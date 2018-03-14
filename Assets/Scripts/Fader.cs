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

    [SerializeField]
    private float screenFadeDuration;
    [SerializeField]
    private float screenFadeEndValue;
    [SerializeField]
    private float alphaTargetValue;

    // Hidden
    public Action onCallback;

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
        CheckAlphaStatus(parent);

        // Getting components from screen parent
        Image[] images = parent.GetComponentsInChildren<Image>();
        Text[] texts = parent.GetComponentsInChildren<Text>();
        Button[] buttons = parent.GetComponentsInChildren<Button>();

        // Tweens the alpha value for all the images of the screen
        foreach (Image image in images)
        {
            image.DOFade(screenFadeEndValue, screenFadeDuration).OnComplete(OnCallback);
        }

        // Tweens the alpha value for all the texts of the screen
        foreach (Text text in texts)
        {
            text.DOFade(screenFadeEndValue, screenFadeDuration).OnComplete(OnCallback);
        }

        // Tweens the alpha value for all the buttons of the screen
        foreach (Button button in buttons)
        {
            button.image.DOFade(screenFadeEndValue, screenFadeDuration).OnComplete(OnCallback);
        }
    }

    private void CheckAlphaStatus(GameObject parent)
    {
        // Getting components from screen parent
        Image[] images = parent.GetComponentsInChildren<Image>();
        Text[] texts = parent.GetComponentsInChildren<Text>();
        Button[] buttons = parent.GetComponentsInChildren<Button>();

        foreach (Image image in images)
        {
            if (image.color.a != alphaTargetValue)
                image.color = new Color(
                    image.color.r,
                    image.color.g,
                    image.color.b,
                    alphaTargetValue);
        }

        // TODO Check value for texts

        // TODO Check value for buttons
    }

    private void OnCallback()
    {
        if (enableConsoleLog)
            Debug.Log("Fader :: OnCallback");

        if (onCallback != null)
            onCallback();
    }

    #endregion
}
