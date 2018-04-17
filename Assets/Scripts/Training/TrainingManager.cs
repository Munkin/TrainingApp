// <copyright file="TrainingManager.cs" company="Up Up Down Studios">
// Copyright (c) 2018 All Rights Reserved
// </copyright>
// <summary>Manager for training events.</summary>

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TrainingLevel
{
    Begginer, Rookie, Medium, Advance
}

public enum TrainingDay
{
    One, Two, Three, Four, Five, Six, Seven
}

public enum TrainingStage
{
    WarmingUp, Training, Stretching
}

public class TrainingManager : MonoBehaviour {

    #region Properties

    [SerializeField]
    private bool enableConsoleLog;

    [Space(10f)]

    [SerializeField]
    private float timeToNonTimeExercise;
    [SerializeField]
    private float timeToNonRestExercise;

    [Space(10f)]

    [SerializeField]
    private Screen[] trainingScreens; // TODO Refactor in the future. DO NOT REFACTOR NOW! (Inspector Data lose in the process).

    [Space(10f)]

    [SerializeField]
    private UpdateTraining updateTraining;

    // Hidden
    [HideInInspector]
    public TrainingStage trainingStage;

    // Consts
    public const float fadeFixTime = 0.125f;

    // Cached Components
    public Training cachedTrainingData
    {
        get; private set;
    }
    private Screen targetScreen;

    // Coroutines
    private IEnumerator nonTimeExercise;
    private IEnumerator rest;
    private IEnumerator nonRest;

    // Singleton!
    public static TrainingManager Singleton
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
        Observer.Singleton.onTestResultScreenCallback += SetTrainingData;

        // Training screens events.
        Observer.Singleton.onWarmingUpScreenCallback += SetWarmingUp;
        Observer.Singleton.onTrainingScreenCallback += SetTraining;
        Observer.Singleton.onStretchingScreenCallback += SetStretching;

        // OnTimerDone events.
        foreach (Screen screen in trainingScreens)
        {
            screen.timer.gameObject.SetActive(false);
            
            // Timer events.
            Observer.Singleton.onTimerDone += screen.ActiveContinue;
            Observer.Singleton.onTimerDone += screen.FadeInContinue;
            Observer.Singleton.onTimerDone += StopWatchSound;
        }

        // TouchManager events.
        TouchManager.OnDoubleTap += SkipTimer;
    }

    // ***

    public void Ready()
    {
        if (targetScreen.ReadyWasAlreadyPressed())
            return;

        // Ready execution
        if (targetScreen.ActualExerciseHasTime())
            ExecuteTimeExercise();
        else
            ExecuteNonTimeExercise();
    }

    public void Continue()
    {
        if (targetScreen.ContinueWasAlreadyPressed())
            return;

        // Continue execution
        if (targetScreen.ActualExerciseHasRest())
            ExecuteRest();
        else
            ExecuteNonRest();
    }

    // *** SET TRAINING DATA ***

    public void NotifyNewTrainingData(Training trainingData)
    {
        cachedTrainingData = trainingData;

        if (enableConsoleLog)
            Debug.Log(string.Format("Training :: NotifyNewTrainingData :: {0}", trainingData.name));

        AllocateData(); // *.*
    }

    private void SetTrainingData()
    {
        TrainingLevel trainingLevel = DataManager.Singleton.trainingLevel;

        switch (trainingLevel)
        {
            case TrainingLevel.Begginer:
                SetBegginerFirstDay();
                break;

            case TrainingLevel.Rookie:
                SetRookieFirstDay();
                break;

            case TrainingLevel.Medium:
                SetMediumFirstDay();
                break;

            case TrainingLevel.Advance:
                SetAdvanceFirstDay();
                break;

            default:
                break;
        }
    }

    private void SetBegginerFirstDay()
    {
        // Setting training level data.
        cachedTrainingData = Resources.Load("TrainingData/Begginer/Begginer_Day1") as Training;

        AllocateData();
    }

    private void SetRookieFirstDay()
    {
        // Setting training level data.
        cachedTrainingData = Resources.Load("TrainingData/Rookie/Rookie_Day1") as Training;

        AllocateData();
    }

    private void SetMediumFirstDay()
    {
        // Setting training level data.
        cachedTrainingData = Resources.Load("TrainingData/Medium/Medium_Day1") as Training;

        AllocateData();
    }

    private void SetAdvanceFirstDay()
    {
        // Setting training level data.
        cachedTrainingData = Resources.Load("TrainingData/Advance/Advance_Day1") as Training;

        AllocateData();
    }

    private void AllocateData()
    {
        for (int i = 0; i < trainingScreens.Length; i++)
        {
            trainingScreens[i].data = cachedTrainingData.screensData[i];
        }
    }

    // *** OTHER FUNCTIONS ***

    private void SetWarmingUp()
    {
        if (enableConsoleLog)
            Debug.Log("Training :: SetWarmingUp");

        trainingStage = TrainingStage.WarmingUp;

        targetScreen = trainingScreens[0];
        targetScreen.SetupScreen();
    }

    private void SetTraining()
    {
        if (enableConsoleLog)
            Debug.Log("Training :: SetTraining");

        trainingStage = TrainingStage.Training;

        targetScreen = trainingScreens[1];
        targetScreen.SetupScreen();
    }

    private void SetStretching()
    {
        if (enableConsoleLog)
            Debug.Log("Training :: SetStretching");

        trainingStage = TrainingStage.Stretching;

        targetScreen = trainingScreens[2];
        targetScreen.SetupScreen();
    }

    // ***

    private void ExecuteTimeExercise()
    {
        targetScreen.ExecuteTimer();
    }

    private void ExecuteNonTimeExercise()
    {
        // Coroutine execution
        if (nonTimeExercise != null)
            StopCoroutine(nonTimeExercise);

        nonTimeExercise = NonTimeExercise();

        StartCoroutine(nonTimeExercise);
    }

    private void ExecuteRest()
    {
        // Coroutine execution
        if (rest != null)
            StopCoroutine(rest);

        rest = Rest();

        StartCoroutine(rest);
    }

    private void ExecuteNonRest()
    {
        // Coroutine execution
        if (nonRest != null)
            StopCoroutine(nonRest);

        nonRest = NonRest();

        StartCoroutine(nonRest);
    }

    // ***

    private void StopWatchSound()
    {
        AudioManager.Singleton.effects[0].Play(); // Alarm
    }

    private void RestContinue()
    {
        targetScreen.SetActiveReady(true);
        targetScreen.SetActiveContinue(false);
        targetScreen.SetupExercise();

        // Fade event!
        if (targetScreen.rootParent.activeInHierarchy)
            Fader.Singleton.FadeScreen(targetScreen.rootParent);
    }

    // ***

    private void SkipTimer()
    {
        // Is the training screen active in the hierarchy ?
        if (UIManager.Singleton.Screens[4].activeInHierarchy && targetScreen != null)
        {
            // Is the continue button active ?
            if (targetScreen.timer.gameObject.activeInHierarchy)
            {
                // Do you wanna quit the stop wathc sound ?
                // -- Observer.Singleton.onTimerDone -= StopWatchSound;
                Observer.Singleton.OnTimerDone();
                // -- Observer.Singleton.onTimerDone += StopWatchSound;
            }
        }
    }

    #endregion

    #region Coroutines

    private IEnumerator NonTimeExercise()
    {
        if (enableConsoleLog)
            Debug.Log("Training :: NonTimeExercise");

        targetScreen.FadeOutReady();

        yield return new WaitForSeconds(timeToNonTimeExercise);

        targetScreen.SetActiveReady(false);
        targetScreen.SetActiveContinue(true);

        targetScreen.FadeInContinue();
    }

    private IEnumerator Rest()
    {
        if (enableConsoleLog)
            Debug.Log("Training :: Rest");

        targetScreen.FadeOutContinue();

        yield return new WaitForSeconds(timeToNonRestExercise);

        Observer.Singleton.OnRestStart();

        yield return new WaitForSeconds(targetScreen.data.exercises[targetScreen.actualExercise].restTime);

        // ***

        bool isTheLastExercise;

        // Is the last exercise of the practice sequence ?
        isTheLastExercise = (targetScreen.actualExercise < targetScreen.data.exercises.Length - 1) ? false : true;

        if (isTheLastExercise)
            Observer.Singleton.onRestEnd += UIManager.Singleton.OnLastExerciseRest;
        else
            Observer.Singleton.onRestEnd += UIManager.Singleton.OnCommonExerciseRest;

        // Event execution!
        if (isTheLastExercise)
            Observer.Singleton.OnRestEnd();

        yield return null;

        // Event execution!
        if (!isTheLastExercise)
            Observer.Singleton.OnRestEnd();
        else
            yield return new WaitForSeconds(fadeFixTime); // Fix a fade error

        if (isTheLastExercise)
            Observer.Singleton.onRestEnd -= UIManager.Singleton.OnLastExerciseRest;
        else
            Observer.Singleton.onRestEnd -= UIManager.Singleton.OnCommonExerciseRest;

        RestContinue();
    }

    private IEnumerator NonRest()
    {
        if (enableConsoleLog)
            Debug.Log("Training :: NonRest");

        targetScreen.FadeOutContinue();

        yield return new WaitForSeconds(timeToNonRestExercise);

        RestContinue();
    }

    #endregion
}
