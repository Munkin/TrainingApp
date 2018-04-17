// <copyright file="TextManager.cs" company="Up Up Down Studios">
// Copyright (c) 2018 All Rights Reserved
// </copyright>
// <summary>Manager for interlude text events.</summary>

using System;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public enum FadeState
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
    [SerializeField]
    private string[] testResultTexts;
    [SerializeField]
    private string[] warmingUpTexts;
    [SerializeField]
    private string[] trainingTexts;
    [SerializeField]
    private string[] stretchingTexts;
    [SerializeField]
    private string[] restTexts;
    [SerializeField]
    private string[] trainingEndTexts;
    [SerializeField]
    private string[] dailyTrainingTexts;
    [SerializeField]
    private string[] alreadyOpenedTexts;

    // Hidden
    private int textIndex = 0;

    // Cached Components
    private bool isFirstFadeOfTheSequence = true;

    public FadeState fadeState
    {
        get; private set;
    }

    #endregion

    #region Unity functions

    private void Awake()
    {
        Suscribe();
    }

    #endregion

    #region Class functions

    private void Suscribe()
    {
        Observer.Singleton.onAppStart += FadeInIntroduction;
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

        // OnAppWasAlreadyOpenedToday events.
        Observer.Singleton.onAppWasAlreadyOpenedToday += SetFirstTextAlreadyOpened;
        Observer.Singleton.onAppWasAlreadyOpenedToday += FadeInAlreadyOpened;
        Observer.Singleton.onAppWasAlreadyOpenedTodayCallback += ResetFadeValues;
        // OnDailyTraining events.
        Observer.Singleton.onDailyTraining += SetFirstTextDailyTraining;
        Observer.Singleton.onDailyTraining += FadeInDailyTraining;
        Observer.Singleton.onDailyTrainingCallback += ResetFadeValues;

        //OnTrainingEnd events.
        Observer.Singleton.onTrainingEnd += FadeInTrainingEnd;

        // ***

        Observer.Singleton.onAppEnd += ResetFadeValues;

        Observer.Singleton.onRestStart += FadeInRest;
        Observer.Singleton.onRestEnd += ResetFadeValues;
    }

    public void ResetTextIndex()
    {
        textIndex = 0;
    }

    #endregion

    #region Fade functions

    private void FadeIn(string screenFirstText, TweenCallback action)
    {
        if (enableConsoleLog)
            Debug.Log("UIManager :: FadeIn");

        fadeState = FadeState.FadeIn;

        // Is the first fade of the entire fade sequence ?
        if (isFirstFadeOfTheSequence)
        {
            mainText.text = screenFirstText;

            isFirstFadeOfTheSequence = false;
        }

        mainText.DOFade(0.8745f, timeToFadeOut).OnComplete(action);
    } // 1.

    private void FadeInterlude(TweenCallback action)
    {
        if (enableConsoleLog)
            Debug.Log("UIManager :: FadeInterlude");

        fadeState = FadeState.Interlude;

        mainText.DOFade(mainText.color.a, timeToWaitForFade).OnComplete(action);
    }

    private void FadeOut(TweenCallback action)
    {
        if (enableConsoleLog)
            Debug.Log("UIManager :: FadeOut");

        fadeState = FadeState.FadeOut;

        mainText.DOFade(0.0f, timeToFadeIn).OnComplete(action);
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

    private void ResetFadeValues()
    {
        fadeState = FadeState.None;

        isFirstFadeOfTheSequence = true;
    } // Control function

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

    #region Fade: Test Result

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
        SetText(introductionTexts, FadeInterludeResult, Observer.Singleton.OnTestResultScreenCallback);
    }

    #endregion

    #region Fade: Training Screen, Warming Up

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
        SetText(introductionTexts, FadeInterludeToWarmingUp, Observer.Singleton.OnWarmingUpScreenCallback);
    }

    #endregion

    #region Fade: Training Screen, Training

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
        SetText(introductionTexts, FadeInterludeToTraining, Observer.Singleton.OnTrainingScreenCallback);
    }

    #endregion

    #region Fade: Training Screen, Stretching

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
        SetText(introductionTexts, FadeInterludeToStretching, Observer.Singleton.OnStretchingScreenCallback);
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
        SetText(introductionTexts, FadeInterludeRest);
    }

    #endregion

    #region Fade: Training End

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
        SetText(introductionTexts, FadeInterludeTrainingEnd, Observer.Singleton.OnAppEnd);
    }

    #endregion

    #region Fade: Already Opened

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
        SetText(introductionTexts, FadeInterludeAlreadyOpened, Observer.Singleton.OnAppWasAlreadyOpenedTodayCallback);
    }

    // First Text
    private void SetFirstTextAlreadyOpened()
    {
        alreadyOpenedTexts[0] = string.Format("Hola {0}", DataManager.Singleton.userName);
    }

    #endregion

    #region Fade: Daily Training

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
        SetText(introductionTexts, FadeInterludeDailyTraining, Observer.Singleton.OnDailyTrainingCallback);
        //Observer.Singleton.OnWarmingUpScreenStart();
    }

    // First Text
    private void SetFirstTextDailyTraining()
    {
        dailyTrainingTexts[0] = string.Format("Hola {0}", DataManager.Singleton.userName);
    }

    #endregion

    // ***

    #endregion
}
