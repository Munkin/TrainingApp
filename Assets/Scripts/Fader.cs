using System;
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
    [SerializeField] [Tooltip("0, 1 Value")]
    private float screenFadeEndValue;
    [SerializeField] [Tooltip("0, 1 Value")]
    private float alphaInitialValue;

    // Events
    public Action onFadeCallback;

    // Cached Components
    private Image[] images;
    private Text[] texts;

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
        screenFadeEndValue = (screenFadeEndValue < 0 || screenFadeEndValue > 1) ? Mathf.Clamp01(screenFadeEndValue) : screenFadeEndValue;

        alphaInitialValue = (alphaInitialValue < 0 || alphaInitialValue > 1) ? Mathf.Clamp01(alphaInitialValue) : alphaInitialValue;

        // Fade events
        Observer.Singleton.onDataFade += FadeScreen;
        Observer.Singleton.onExerciseDataFade += FadeScreen;
        Observer.Singleton.onTestEndFade += FadeScreen;
    }

    #endregion

    #region Class functions

    public void FadeScreen(GameObject parent)
    {
        if (enableConsoleLog)
            Debug.Log("Fader :: FadeScreen");

        // Getting components from screen parent
        images = parent.GetComponentsInChildren<Image>();
        texts = parent.GetComponentsInChildren<Text>();

        CheckAlphaStatus(parent);

        // Tweens the alpha value for all the images of the screen
        foreach (Image image in images)
        {
            // Is the image a non fade element ?
            if (!image.CompareTag("Toggle") &&
                !image.CompareTag("MaterialButtonLayer") &&
                !image.CompareTag("MaterialText") &&
                !image.CompareTag("Shadow"))
                image.DOFade(screenFadeEndValue, screenFadeDuration).OnComplete(OnFadeCallback);
        }

        // Tweens the alpha value for all the texts of the screen
        foreach (Text text in texts)
        {
            text.DOFade(screenFadeEndValue, screenFadeDuration).OnComplete(OnFadeCallback);
        }
    }

    private void CheckAlphaStatus(GameObject parent)
    {
        // Checking values for images
        foreach (Image image in images)
        {
            // Is the image a non fade element ?
            if (!image.CompareTag("Toggle") &&
                !image.CompareTag("MaterialButtonLayer") &&
                !image.CompareTag("MaterialText") &&
                !image.CompareTag("Shadow"))
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
            Debug.Log("Fader :: OnCallback");

        // Event call!
        if (onFadeCallback != null)
            onFadeCallback();
    }

    #endregion
}
