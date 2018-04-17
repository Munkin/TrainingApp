// <copyright file="Observer.cs" company="Up Up Down Studios">
// Copyright (c) 2018 All Rights Reserved
// </copyright>
// <summary>Observer class to perform different game events.</summary>

using System;
using UnityEngine;

public class Observer : MonoBehaviour {

    #region Properties

    [SerializeField]
    private bool enableConsoleLog;
    [SerializeField]
    private bool enableConsoleLogForFadeCallbacks;

    // ***

    public Action onAppStart;
    public Action onAppEnd;

    // ***

    public Action onIntroduction;
    public Action onDataScreen;
    public Action onTestScreen;
    public Action onTestResultScreen;
    public Action onTestResultScreenCallback;

    public Action<GameObject, float, float> onDataScreenFade;
    public Action<GameObject, float, float> onTestScreenFade;
    public Action<GameObject, float, float> onTestResultScreenFade;

    // ***

    public Action onTrainingStart;
    public Action onTrainingEnd;

    // ***

    public Action onWarmingUpScreen;
    public Action onWarmingUpScreenCallback;
    public Action onTrainingScreen;
    public Action onTrainingScreenCallback;
    public Action onStretchingScreen;
    public Action onStretchingScreenCallback;

    public Action<GameObject, float, float> onWarmingUpScreenFade;
    public Action<GameObject, float, float> onTrainingScreenFade;
    public Action<GameObject, float, float> onStretchingScreenFade;

    // ***

    public Action onRestStart;
    public Action onRestEnd;

    // ***

    public Action onDailyTraining;
    public Action onDailyTrainingCallback;
    public Action onAppWasAlreadyOpenedToday;
    public Action onAppWasAlreadyOpenedTodayCallback;

    // ***

    public Action onScreenFadeCallback;
    public Action onButtonFadeCallback;

    public Action onTimerDone;

    // Singleton!
    public static Observer Singleton
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

    private void Start()
    {
        OnAppStart();
    }

    #endregion

    #region Class functions

    private void Suscribe()
    {
        onAppStart += InitializeApp;
    }

    // ***

    public void OnAppStart()
    {
        if (enableConsoleLog)
            Debug.Log("UIManager :: OnAppStart");

        // Event call!
        if (onAppStart != null)
            onAppStart();
    }

    public void OnAppEnd()
    {
        if (enableConsoleLog)
            Debug.Log("UIManager :: OnAppEnd");

        // Event call!
        if (onAppEnd != null)
            onAppEnd();
    }

    // ***

    public void OnIntroduction()
    {
        if (enableConsoleLog)
            Debug.Log("Observer :: OnIntroduction");

        // Event call!
        if (onIntroduction != null)
            onIntroduction();
    }

    public void OnDataScreen()
    {
        if (enableConsoleLog)
            Debug.Log("Observer :: OnDataScreen");

        // Fade event!
        if (onDataScreenFade != null)
            onDataScreenFade(
                UIManager.Singleton.Screens[1],
                Fader.Singleton.screenFadeDuration,
                Fader.Singleton.screenFadeEndValue);

        // Event call!
        if (onDataScreen != null)
            onDataScreen();
    }

    public void OnTestScreen()
    {
        if (enableConsoleLog)
            Debug.Log("Observer :: OnTestScreen");

        // Fade event!
        if (onTestScreenFade != null)
            onTestScreenFade(
                UIManager.Singleton.Screens[2],
                Fader.Singleton.screenFadeDuration,
                Fader.Singleton.screenFadeEndValue);

        // Event call!
        if (onTestScreen != null)
            onTestScreen();
    }

    public void OnTestResultScreen()
    {
        if (enableConsoleLog)
            Debug.Log("Observer :: OnTestResultScreen");

        // Event call!
        if (onTestResultScreen != null)
            onTestResultScreen();
    }

    public void OnTestResultScreenCallback()
    {
        if (enableConsoleLog)
            Debug.Log("Observer :: OnTestResultScreenCallback");

        // Fade event!
        if (onTestResultScreenFade != null)
            onTestResultScreenFade(
                UIManager.Singleton.Screens[3],
                Fader.Singleton.screenFadeDuration,
                Fader.Singleton.screenFadeEndValue);

        // Event call!
        if (onTestResultScreenCallback != null)
            onTestResultScreenCallback();
    }

    // ***

    public void OnTrainingStart()
    {
        if (enableConsoleLog)
            Debug.Log("Observer :: OnTrainingStart");

        // Event call!
        if (onTrainingStart != null)
            onTrainingStart();
    }

    public void OnTrainingEnd()
    {
        if (enableConsoleLog)
            Debug.Log("Observer :: OnTrainingEnd");

        // Event call!
        if (onTrainingEnd != null)
            onTrainingEnd();
    }

    // ***

    public void OnWarmingUpScreen()
    {
        if (enableConsoleLog)
            Debug.Log("Observer :: OnWarmingUpScreen");

        // Event call!
        if (onWarmingUpScreen != null)
            onWarmingUpScreen();
    }

    public void OnWarmingUpScreenCallback()
    {
        if (enableConsoleLog)
            Debug.Log("Observer :: OnWarmingUpScreenCallback");

        // Fade event!
        if (onWarmingUpScreenFade != null)
            onWarmingUpScreenFade(
                UIManager.Singleton.trainingScreens[0],
                Fader.Singleton.screenFadeDuration,
                Fader.Singleton.screenFadeEndValue);

        // Event call!
        if (onWarmingUpScreenCallback != null)
            onWarmingUpScreenCallback();
    }

    public void OnTrainingScreen()
    {
        if (enableConsoleLog)
            Debug.Log("Observer :: OnTrainingScreen");

        // Event call!
        if (onTrainingScreen != null)
            onTrainingScreen();
    }

    public void OnTrainingScreenCallback()
    {
        if (enableConsoleLog)
            Debug.Log("Observer :: OnTrainingScreenCallback");

        // Fade event!
        if (onTrainingScreenFade != null)
            onTrainingScreenFade(
                UIManager.Singleton.trainingScreens[1],
                Fader.Singleton.screenFadeDuration,
                Fader.Singleton.screenFadeEndValue);

        // Event call!
        if (onTrainingScreenCallback != null)
            onTrainingScreenCallback();
    }
    
    public void OnStretchingScreen()
    {
        if (enableConsoleLog)
            Debug.Log("Observer :: OnStretchingScreen");

        // Event call!
        if (onStretchingScreen != null)
            onStretchingScreen();
    }

    public void OnStretchingScreenCallback()
    {
        if (enableConsoleLog)
            Debug.Log("Observer :: OnStretchingScreenCallback");

        // Fade event!
        if (onStretchingScreenFade != null)
            onStretchingScreenFade(
                UIManager.Singleton.trainingScreens[2],
                Fader.Singleton.screenFadeDuration,
                Fader.Singleton.screenFadeEndValue);

        // Event call!
        if (onStretchingScreenCallback != null)
            onStretchingScreenCallback();
    }

    // ***

    public void OnRestStart()
    {
        if (enableConsoleLog)
            Debug.Log("Observer :: OnRestStart");

        // Event call!
        if (onRestStart != null)
            onRestStart();
    }

    public void OnRestEnd()
    {
        if (enableConsoleLog)
            Debug.Log("Observer :: OnRestEnd");

        // Event call!
        if (onRestEnd != null)
            onRestEnd();
    }

    // ***

    public void OnDailyTraining()
    {
        if (enableConsoleLog)
            Debug.Log("Observer :: OnDailyTraining");

        // Event call!
        if (onDailyTraining != null)
            onDailyTraining();
    }

    public void OnDailyTrainingCallback()
    {
        if (enableConsoleLog)
            Debug.Log("Observer :: OnDailyTrainingCallback");

        // Event call!
        if (onDailyTrainingCallback != null)
            onDailyTrainingCallback();
    }

    public void OnAppWasAlreadyOpenedToday()
    {
        if (enableConsoleLog)
            Debug.Log("Observer :: OnAppWasAlreadyOpenedToday");

        // Event call!
        if (onAppWasAlreadyOpenedToday != null)
            onAppWasAlreadyOpenedToday();
    }

    public void OnAppWasAlreadyOpenedTodayCallback()
    {
        if (enableConsoleLog)
            Debug.Log("Observer :: OnAppWasAlreadyOpenedTodayCallback");

        // Event call!
        if (onAppWasAlreadyOpenedTodayCallback != null)
            onAppWasAlreadyOpenedTodayCallback();
    }

    // ***

    public void OnScreenFadeCallback()
    {
        if (enableConsoleLogForFadeCallbacks)
            Debug.Log("Observer :: OnScreenFadeCallback");

        // Event call!
        if (onScreenFadeCallback != null)
            onScreenFadeCallback();
    }

    public void OnButtonFadeCallback()
    {
        if (enableConsoleLogForFadeCallbacks)
            Debug.Log("Observer :: OnButtonFadeCallback");

        // Event call!
        if (onButtonFadeCallback != null)
            onButtonFadeCallback();
    }

    // ***

    public void OnTimerDone()
    {
        if (enableConsoleLog)
            Debug.Log("Observer :: OnTimerDone");

        // Event call!
        if (onTimerDone != null)
            onTimerDone();
    }

    // ***

    private void InitializeApp()
    {
        DateManager.Singleton.CheckDate();

        // Can the user do the test ?
        if (DataManager.Singleton.CanTheUserDoTheTest())
            OnIntroduction();
        else
        {
            // Did the user trained today ?
            if (DateManager.Singleton.HasPassOneDaySinceLastTraining())
                OnDailyTraining();
            else
                OnAppWasAlreadyOpenedToday();
        }
    }

    #endregion
}
