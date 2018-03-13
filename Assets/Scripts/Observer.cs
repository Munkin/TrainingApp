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

    // General Actions
    public Action onIntroductionScreen;
    public Action onDataScreen;
    public Action onExerciseDataScreen;
    public Action onTestResult;
    public Action onTestEnd;

    // Training Actions

    // WarmingUp
    public Action onWarmingUpScreenStart;
    public Action onWarmingUpScreenEnd;
    // Training
    public Action onTrainingScreenStart;
    public Action onTrainingScreenEnd;
    // Stretching
    public Action onStretchingScreenStart;
    public Action onStretchingScreenEnd;

    // Data Actions
    public Action onSave;
    public Action onLoad;

    // Others
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
        OnIntroductionScreen();
    }

    #endregion

    #region Class functions

    public void OnIntroductionScreen()
    {
        if (enableConsoleLog)
            Debug.Log("Observer :: OnIntroductionScreen");

        if (onIntroductionScreen != null)
            onIntroductionScreen();
    }

    public void OnDataScreen()
    {
        if (enableConsoleLog)
            Debug.Log("Observer :: OnDataScreen");

        if (onDataScreen != null)
            onDataScreen();
    }

    public void OnExerciseDataScreen()
    {
        if (enableConsoleLog)
            Debug.Log("Observer :: OnExerciseDataScreen");

        if (onExerciseDataScreen != null)
            onExerciseDataScreen();
    }

    public void OnTestResult()
    {
        if (enableConsoleLog)
            Debug.Log("Observer :: OnTestResult");

        if (onTestResult != null)
            onTestResult();
    }

    public void OnTestEnd()
    {
        if (enableConsoleLog)
            Debug.Log("Observer :: OnTestEnd");

        if (onTestEnd != null)
            onTestEnd();
    }

    // ***

    public void OnWarmingUpScreenStart()
    {
        if (enableConsoleLog)
            Debug.Log("Observer :: OnWarmingUpScreenStart");

        if (onWarmingUpScreenStart != null)
            onWarmingUpScreenStart();
    }

    public void OnWarmingUpScreenEnd()
    {
        if (enableConsoleLog)
            Debug.Log("Observer :: OnWarmingUpScreenEnd");

        if (onWarmingUpScreenEnd != null)
            onWarmingUpScreenEnd();
    }

    // ***

    public void OnTrainingScreenStart()
    {
        if (enableConsoleLog)
            Debug.Log("Observer :: OnTrainingScreenStart");

        if (onTrainingScreenStart != null)
            onTrainingScreenStart();
    }

    public void OnTrainingScreenEnd()
    {
        if (enableConsoleLog)
            Debug.Log("Observer :: OnTrainingScreenEnd");

        if (onTrainingScreenEnd != null)
            onTrainingScreenEnd();
    }

    // ***

    public void OnStretchingScreenStart()
    {
        if (enableConsoleLog)
            Debug.Log("Observer :: OnStretchingScreenStart");

        if (onStretchingScreenStart != null)
            onStretchingScreenStart();
    }

    public void OnStretchingScreenEnd()
    {
        if (enableConsoleLog)
            Debug.Log("Observer :: OnStretchingScreenEnd");

        if (onStretchingScreenEnd != null)
            onStretchingScreenEnd();
    }

    // ***

    public void OnSave()
    {
        if (enableConsoleLog)
            Debug.Log("Observer :: OnSave");

        if (onSave != null)
            onSave();
    }

    public void OnLoad()
    {
        if (enableConsoleLog)
            Debug.Log("Observer :: OnLoad");

        if (onLoad != null)
            onLoad();
    }

    public void OnTimerDone()
    {
        if (enableConsoleLog)
            Debug.Log("Observer :: OnTimerDone");

        if (onTimerDone != null)
            onTimerDone();
    }

    #endregion
}
