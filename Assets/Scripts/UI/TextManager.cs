// <copyright file="TextManager.cs" company="Up Up Down Studios">
// Copyright (c) 2018 All Rights Reserved
// </copyright>
// <summary>Manager for interlude text events.</summary>

using System;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public enum TextFadeState
{
    None, FadeIn, Interlude, FadeOut
}

public class TextManager : MonoBehaviour {

    #region Properties

    [SerializeField]
    private bool enableConsoleLog;

    [Space(10f)]

    [SerializeField]
    private Text mainText;

    [Space(10f)] [Header("Fade Options")]

    [SerializeField]
    private float timeToFadeIn;
    [SerializeField]
    private float timeToFadeOut;
    [SerializeField]
    private float timeToWaitForFade;

    [Space(10f)] [Header("Texts")]

    [SerializeField]
    private string[] introductionTexts;

    [Space(10f)]

    [SerializeField]
    private string[] testResultTexts;

    [Space(10f)]

    [SerializeField]
    private string[] warmingUpTexts;
    [SerializeField]
    private string[] trainingTexts;
    [SerializeField]
    private string[] stretchingTexts;

    [Space(10f)]

    [SerializeField]
    private string[] restTexts;

    [Space(10f)]

    [SerializeField]
    private string[] trainingEndTexts;

    [Space(10f)]

    [SerializeField]
    private string[] dailyTrainingTexts;
    [SerializeField]
    private string[] alreadyOpenedTexts;

    // Hidden
    private int textIndex = 0;

    // Cached Components
    private bool isFirstFadeOfTheSequence = true;

    public TextFadeState textFadeState
    {
        get; private set;
    }

    private Tweener cachedTweener;
    private TweenCallback cachedTweenCallback;

    // Singleton
    public static TextManager Singleton
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

        Suscribe();
    }

    #endregion

    #region Class functions

    private void Suscribe()
    {
        Observer.Singleton.onAppStart += FadeInIntroduction;
        Observer.Singleton.onAppEnd += ResetFadeValues;
        Observer.Singleton.onDataScreen += ResetFadeValues;
        Observer.Singleton.onTestResultScreen += FadeInResult;
        Observer.Singleton.onTestResultScreenCallback += ResetFadeValues;

        // ***

        // OnWarmingUp events.
        Observer.Singleton.onWarmingUpScreen += FadeInToWarmingUp;
        Observer.Singleton.onWarmingUpScreenCallback += ResetFadeValues;
        // OnTraining events.
        Observer.Singleton.onTrainingScreen += FadeInToTraining;
        Observer.Singleton.onTrainingScreenCallback += ResetFadeValues;
        // OnStretching events.
        Observer.Singleton.onStretchingScreen += FadeInToStretching;
        Observer.Singleton.onStretchingScreenCallback += ResetFadeValues;

        // ***

        //OnTrainingEnd events.
        Observer.Singleton.onTrainingEnd += FadeInTrainingEnd;

        // ***

        // Rest events.
        Observer.Singleton.onRestStart += FadeInRest;
        Observer.Singleton.onRestEnd += ResetFadeValues;

        // ***

        // OnDailyTraining events.
        Observer.Singleton.onDailyTraining += SetFirstTextDailyTraining;
        Observer.Singleton.onDailyTraining += FadeInDailyTraining;
        Observer.Singleton.onDailyTrainingCallback += ResetFadeValues;
        // OnAppWasAlreadyOpenedToday events.
        Observer.Singleton.onAppWasAlreadyOpenedToday += SetFirstTextAlreadyOpened;
        Observer.Singleton.onAppWasAlreadyOpenedToday += FadeInAlreadyOpened;
        Observer.Singleton.onAppWasAlreadyOpenedTodayCallback += ResetFadeValues;

        // ***

        // Touch events.
        TouchManager.OnTap += StopFade;
    }

    public void ResetTextIndex()
    {
        textIndex = 0;

        textFadeState = (textFadeState != TextFadeState.None) ? TextFadeState.None : textFadeState;
    }

    #endregion

    #region Fade functions

    private void FadeIn(string screenFirstText, TweenCallback action)
    {
        if (enableConsoleLog)
            Debug.Log("TextManager :: FadeIn");

        textFadeState = TextFadeState.FadeIn;

        // Is the first fade of the entire fade sequence ?
        if (isFirstFadeOfTheSequence)
        {
            mainText.text = screenFirstText;

            isFirstFadeOfTheSequence = false;
        }

        mainText.DOFade(0.8745f, timeToFadeIn).OnComplete(action);
    } // 1.

    private void FadeInterlude(TweenCallback action)
    {
        if (enableConsoleLog)
            Debug.Log("TextManager :: FadeInterlude");

        textFadeState = TextFadeState.Interlude;

        cachedTweener = mainText.DOFade(mainText.color.a, timeToWaitForFade).OnComplete(action);
        cachedTweenCallback = action;
    }

    private void FadeOut(TweenCallback action)
    {
        if (enableConsoleLog)
            Debug.Log("TextManager :: FadeOut");

        textFadeState = TextFadeState.FadeOut;

        mainText.DOFade(0.0f, timeToFadeOut).OnComplete(action);
    }

    private void SetText(string[] screenTexts, TweenCallback action)
    {
        if (textIndex < screenTexts.Length - 1)
            textIndex++;

        // Is the interlude screen active in hierarchy ?
        if (UIManager.Singleton.IsScreenActiveInHierarchy(0))
        {
            mainText.text = screenTexts[textIndex];

            FadeIn(screenTexts[0], action);
        }
    }

    private void SetText(string[] screenTexts, TweenCallback action, Action fadeEndCallback)
    {
        if (textIndex < screenTexts.Length - 1)
            textIndex++;
        else
        {
            if (fadeEndCallback != null)
                fadeEndCallback();
        }

        // Is the interlude screen active in hierarchy ?
        if (UIManager.Singleton.IsScreenActiveInHierarchy(0))
        {
            mainText.text = screenTexts[textIndex];

            FadeIn(screenTexts[0], action);
        }
    }

    private void SetTextNoFinalFlicker(string[] screenTexts, TweenCallback action)
    {
        if (textIndex < screenTexts.Length - 2)
            textIndex++;
        else
        {
            textIndex++;

            mainText.text = screenTexts[textIndex];
            // FadeIn
            mainText.DOFade(0.8745f, timeToFadeIn);

            textFadeState = TextFadeState.None;

            return;
        }

        // Is the interlude screen active in hierarchy ?
        if (UIManager.Singleton.IsScreenActiveInHierarchy(0))
        {
            mainText.text = screenTexts[textIndex];

            FadeIn(screenTexts[0], action);

            mainText.DOFade(0.8745f, timeToFadeIn).OnComplete(action);
        }
    }

    private void ResetFadeValues()
    {
        textFadeState = TextFadeState.None;

        isFirstFadeOfTheSequence = true;
    } // Control function

    private void StopFade()
    {
        // Is the interlude screen active ?
        if (UIManager.Singleton.Screens[0].activeInHierarchy)
        {
            // Is the fade sequence in the interlude state ?
            if (textFadeState == TextFadeState.Interlude)
            {
                // Stop!
                if (enableConsoleLog)
                    Debug.Log("TextManager :: StopFade");

                cachedTweener.Kill();

                cachedTweenCallback();
            }
        }
    }

    // ***

    #region Fade: Introduction

    private void FadeInIntroduction()
    {
        FadeIn(introductionTexts[0], FadeInterludeIntroduction);
    } // 1.

    private void FadeInterludeIntroduction()
    {
        FadeInterlude(FadeOutIntroduction);
    }

    private void FadeOutIntroduction()
    {
        FadeOut(SetTextIntroduction);
    }

    private void SetTextIntroduction()
    {
        SetText(introductionTexts, FadeInterludeIntroduction, Observer.Singleton.OnDataScreen);
    }

    #endregion

    #region Fade: Test result

    private void FadeInResult()
    {
        FadeIn(testResultTexts[0], FadeInterludeResult);
    } // 1.

    private void FadeInterludeResult()
    {
        FadeInterlude(FadeOutResult);
    }

    private void FadeOutResult()
    {
        FadeOut(SetTextResult);
    }

    private void SetTextResult()
    {
        SetText(testResultTexts, FadeInterludeResult, Observer.Singleton.OnTestResultScreenCallback);
    }

    #endregion

    #region Fade: Training screen, warming up

    private void FadeInToWarmingUp()
    {
        FadeIn(warmingUpTexts[0], FadeInterludeToWarmingUp);
    } // 1.

    private void FadeInterludeToWarmingUp()
    {
        FadeInterlude(FadeOutToWarmingUp);
    }

    private void FadeOutToWarmingUp()
    {
        FadeOut(SetTextToWarmingUp);
    }

    private void SetTextToWarmingUp()
    {
        SetText(warmingUpTexts, FadeInterludeToWarmingUp, Observer.Singleton.OnWarmingUpScreenCallback);
    }

    #endregion

    #region Fade: Training screen, training

    private void FadeInToTraining()
    {
        FadeIn(trainingTexts[0], FadeInterludeToTraining);
    } // 1.

    private void FadeInterludeToTraining()
    {
        FadeInterlude(FadeOutToTraining);
    }

    private void FadeOutToTraining()
    {
        FadeOut(SetTextToTraining);
    }

    private void SetTextToTraining()
    {
        SetText(trainingTexts, FadeInterludeToTraining, Observer.Singleton.OnTrainingScreenCallback);
    }

    #endregion

    #region Fade: Training screen, stretching

    private void FadeInToStretching()
    {
        FadeIn(stretchingTexts[0], FadeInterludeToStretching);
    } // 1.

    private void FadeInterludeToStretching()
    {
        FadeInterlude(FadeOutToStretching);
    }

    private void FadeOutToStretching()
    {
        FadeOut(SetTextToStretching);
    }

    private void SetTextToStretching()
    {
        SetText(stretchingTexts, FadeInterludeToStretching, Observer.Singleton.OnStretchingScreenCallback);
    }

    #endregion

    #region Fade: Rest

    private void FadeInRest()
    {
        FadeIn(restTexts[0], FadeInterludeRest);
    } // 1.

    private void FadeInterludeRest()
    {
        FadeInterlude(FadeOutRest);
    }

    private void FadeOutRest()
    {
        FadeOut(SetTextRest);
    }

    private void SetTextRest()
    {
        SetTextNoFinalFlicker(restTexts, FadeInterludeRest);
    }

    #endregion

    #region Fade: Training end

    private void FadeInTrainingEnd()
    {
        FadeIn(trainingEndTexts[0], FadeInterludeTrainingEnd);
    } // 1.

    private void FadeInterludeTrainingEnd()
    {
        FadeInterlude(FadeOutTrainingEnd);
    }

    private void FadeOutTrainingEnd()
    {
        FadeOut(SetTextTrainingEnd);
    }

    private void SetTextTrainingEnd()
    {
        SetText(trainingEndTexts, FadeInterludeTrainingEnd, Observer.Singleton.OnAppEnd);
    }

    #endregion

    #region Fade: Daily training

    private void FadeInDailyTraining()
    {
        FadeIn(dailyTrainingTexts[0], FadeInterludeDailyTraining);
    } // 1.

    private void FadeInterludeDailyTraining()
    {
        FadeInterlude(FadeOutDailyTraining);
    }

    private void FadeOutDailyTraining()
    {
        FadeOut(SetTextDailyTraining);
    }

    private void SetTextDailyTraining()
    {
        SetText(dailyTrainingTexts, FadeInterludeDailyTraining, Observer.Singleton.OnDailyTrainingCallback);
        //Observer.Singleton.OnWarmingUpScreenStart();
    }

    // First Text
    private void SetFirstTextDailyTraining()
    {
        dailyTrainingTexts[0] = string.Format("Hola {0}", DataManager.Singleton.userName);
    }

    #endregion

    #region Fade: Already opened

    private void FadeInAlreadyOpened()
    {
        FadeIn(alreadyOpenedTexts[0], FadeInterludeAlreadyOpened);
    } // 1.

    private void FadeInterludeAlreadyOpened()
    {
        FadeInterlude(FadeOutAlreadyOpened);
    }

    private void FadeOutAlreadyOpened()
    {
        FadeOut(SetTextAlreadyOpened);
    }

    private void SetTextAlreadyOpened()
    {
        SetText(alreadyOpenedTexts, FadeInterludeAlreadyOpened, Observer.Singleton.OnAppWasAlreadyOpenedTodayCallback);
    }

    // First Text
    private void SetFirstTextAlreadyOpened()
    {
        alreadyOpenedTexts[0] = string.Format("Hola {0}", DataManager.Singleton.userName);
    }

    #endregion

    // ***

    #endregion
}
