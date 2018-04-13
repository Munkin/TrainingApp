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
    private bool enableFadeCallbacksConsoleLog;

    // General Actions ***

    // First Time
    public Action onIntroductionScreen;
    public Action onDataScreen;
    public Action onExerciseDataScreen;
    public Action onTestResult;
    public Action onTestEnd;
    public Action onTrainingStart;
    public Action onTrainingEnd;
    public Action onAppEnd;
    // App Already Opened
    public Action onDailyTraining;
    public Action onDailyTrainingEnd;
    public Action onAppWasAlreadyOpenedToday;
    public Action onAppWasAlreadyOpenedTodayEnd;

    // Training Actions ***

    // WarmingUp
    public Action onWarmingUpScreenStart;
    public Action onWarmingUpScreenEnd;
    // Training
    public Action onTrainingScreenStart;
    public Action onTrainingScreenEnd;
    // Stretching
    public Action onStretchingScreenStart;
    public Action onStretchingScreenEnd;

    // Fade Actions ***

    public Action<GameObject, float, float> onDataScreenFade;
    public Action<GameObject, float, float> onExerciseDataScreenFade;
    public Action<GameObject, float, float> onTestEndScreenFade;
    public Action<GameObject, float, float> onWarmingUpScreenEndFade;
    public Action<GameObject, float, float> onTrainingScreenEndFade;
    public Action<GameObject, float, float> onStretchingScreenEndFade;

    // Data Actions ***

    public Action onSave;
    public Action onLoad;
    public Action onTrainingLoadCallback;

    // Date Actions ***

    public Action onDateSet;
    public Action onDateGet;

    // Rest Actions ***

    public Action onRestStart;
    public Action onRestEnd;

    // Other actions ***

    // Fade Actions
    public Action onScreenFadeCallback;
    public Action onButtonFadeCallback;
    // Time Actions
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
    }

    private void Start()
    {
        DateManager.Singleton.CheckDate();
        
        // Can the user do the test ?
        if (DataManager.Singleton.CanTheUserDoTheTest())
            OnIntroductionScreen(); // Implementend
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

    #region Class functions

    public void OnIntroductionScreen()
    {
        if (enableConsoleLog)
            Debug.Log("Observer :: OnIntroductionScreen");

        // Event call!
        if (onIntroductionScreen != null)
            onIntroductionScreen();
    }

    public void OnDataScreen()
    {
        if (enableConsoleLog)
            Debug.Log("Observer :: OnDataScreen");

        // Fade event!
        if (onDataScreenFade != null)
            onDataScreenFade(
                UIManager.Singleton.screens[1],
                Fader.Singleton.screenFadeDuration,
                Fader.Singleton.screenFadeEndValue);

        // Event call!
        if (onDataScreen != null)
            onDataScreen();
    }

    public void OnExerciseDataScreen()
    {
        if (enableConsoleLog)
            Debug.Log("Observer :: OnExerciseDataScreen");

        // Fade event!
        if (onExerciseDataScreenFade != null)
            onExerciseDataScreenFade(
                UIManager.Singleton.screens[2],
                Fader.Singleton.screenFadeDuration,
                Fader.Singleton.screenFadeEndValue);

        // Event call!
        if (onExerciseDataScreen != null)
            onExerciseDataScreen();
    }

    public void OnTestResult()
    {
        if (enableConsoleLog)
            Debug.Log("Observer :: OnTestResult");

        // Event call!
        if (onTestResult != null)
            onTestResult();
    }

    public void OnTestEnd()
    {
        if (enableConsoleLog)
            Debug.Log("Observer :: OnTestEnd");

        // Fade event!
        if (onTestEndScreenFade != null)
            onTestEndScreenFade(
                UIManager.Singleton.screens[3],
                Fader.Singleton.screenFadeDuration,
                Fader.Singleton.screenFadeEndValue);

        // Event call!
        if (onTestEnd != null)
            onTestEnd();
    }

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

    public void OnAppEnd()
    {
        if (enableConsoleLog)
            Debug.Log("UIManager :: OnAppEnd");

        // Event call!
        if (onAppEnd != null)
            onAppEnd();
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

    public void OnDailyTrainingEnd()
    {
        if (enableConsoleLog)
            Debug.Log("Observer :: OnDailyTrainingEnd");

        // Event call!
        if (onDailyTrainingEnd != null)
            onDailyTrainingEnd();
    }

    public void OnAppWasAlreadyOpenedToday()
    {
        if (enableConsoleLog)
            Debug.Log("Observer :: OnAppWasAlreadyOpenedToday");

        // Event call!
        if (onAppWasAlreadyOpenedToday != null)
            onAppWasAlreadyOpenedToday();
    }

    public void OnAppWasAlreadyOpenedTodayEnd()
    {
        if (enableConsoleLog)
            Debug.Log("Observer :: OnAppWasAlreadyOpenedTodayEnd");

        // Event call!
        if (onAppWasAlreadyOpenedTodayEnd != null)
            onAppWasAlreadyOpenedTodayEnd();
    }

    // ***

    public void OnWarmingUpScreenStart()
    {
        if (enableConsoleLog)
            Debug.Log("Observer :: OnWarmingUpScreenStart");

        // Event call!
        if (onWarmingUpScreenStart != null)
            onWarmingUpScreenStart();
    }

    public void OnWarmingUpScreenEnd()
    {
        if (enableConsoleLog)
            Debug.Log("Observer :: OnWarmingUpScreenEnd");

        // Fade event!
        if (onWarmingUpScreenEndFade != null)
            onWarmingUpScreenEndFade(
                UIManager.Singleton.trainingScreens[0],
                Fader.Singleton.screenFadeDuration,
                Fader.Singleton.screenFadeEndValue);

        // Event call!
        if (onWarmingUpScreenEnd != null)
            onWarmingUpScreenEnd();
    }
    
    // ***

    public void OnTrainingScreenStart()
    {
        if (enableConsoleLog)
            Debug.Log("Observer :: OnTrainingScreenStart");

        // Event call!
        if (onTrainingScreenStart != null)
            onTrainingScreenStart();
    }

    public void OnTrainingScreenEnd()
    {
        if (enableConsoleLog)
            Debug.Log("Observer :: OnTrainingScreenEnd");

        // Fade event!
        if (onTrainingScreenEndFade != null)
            onTrainingScreenEndFade(
                UIManager.Singleton.trainingScreens[1],
                Fader.Singleton.screenFadeDuration,
                Fader.Singleton.screenFadeEndValue);

        // Event call!
        if (onTrainingScreenEnd != null)
            onTrainingScreenEnd();
    }
    
    // ***

    public void OnStretchingScreenStart()
    {
        if (enableConsoleLog)
            Debug.Log("Observer :: OnStretchingScreenStart");

        // Event call!
        if (onStretchingScreenStart != null)
            onStretchingScreenStart();
    }

    public void OnStretchingScreenEnd()
    {
        if (enableConsoleLog)
            Debug.Log("Observer :: OnStretchingScreenEnd");

        // Fade event!
        if (onStretchingScreenEndFade != null)
            onStretchingScreenEndFade(
                UIManager.Singleton.trainingScreens[2],
                Fader.Singleton.screenFadeDuration,
                Fader.Singleton.screenFadeEndValue);

        // Event call!
        if (onStretchingScreenEnd != null)
            onStretchingScreenEnd();
    }
    
    // *** DATA EVENTS ***

    public void OnSave()
    {
        if (enableConsoleLog)
            Debug.Log("Observer :: OnSave");

        // Event call!
        if (onSave != null)
            onSave();
    }

    public void OnLoad()
    {
        if (enableConsoleLog)
            Debug.Log("Observer :: OnLoad");

        // Event call!
        if (onLoad != null)
            onLoad();
    }

    public void OnTrainingLoadCallback()
    {
        if (enableConsoleLog)
            Debug.Log("Observer :: OnTrainingCallback");

        // Event call!
        if (onTrainingLoadCallback != null)
            onTrainingLoadCallback();
    }

    // *** DATE EVENTS ***

    public void OnDateSet()
    {
        if (enableConsoleLog)
            Debug.Log("Observer :: OnDateSave");

        // Event call!
        if (onDateSet != null)
            onDateSet();
    }

    public void OnDateGet()
    {
        if (enableConsoleLog)
            Debug.Log("Observer :: OnDateLoad");

        // Event call!
        if (onDateGet != null)
            onDateGet();
    }

    // *** REST EVENTS ***

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

    // *** OTHER EVENTS ***

    public void OnScreenFadeCallback()
    {
        if (enableFadeCallbacksConsoleLog)
            Debug.Log("Observer :: OnFadeCallback");

        // Event call!
        if (onScreenFadeCallback != null)
            onScreenFadeCallback();
    }

    public void OnButtonFadeCallback()
    {
        if (enableFadeCallbacksConsoleLog)
            Debug.Log("Observer :: OnButtonFadeCallback");

        // Event call!
        if (onButtonFadeCallback != null)
            onButtonFadeCallback();
    }

    public void OnTimerDone()
    {
        if (enableConsoleLog)
            Debug.Log("Observer :: OnTimerDone");

        // Event call!
        if (onTimerDone != null)
            onTimerDone();
    }

    #endregion
}
