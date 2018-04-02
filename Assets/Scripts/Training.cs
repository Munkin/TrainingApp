// <copyright file="Training.cs" company="Up Up Down Studios">
// Copyright (c) 2018 All Rights Reserved
// </copyright>
// <summary>Manager for training events.</summary>

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TrainingStage
{
    WarmingUp, Training, Stretching
}

public enum TrainingDay
{
    One, Two, Three, Four, Five, Six, Seven
}

public class Training : MonoBehaviour { //TODO Training instantiate.

    #region Properties

    [SerializeField]
    private bool enableConsoleLog;

    [Space(10f)]

    [SerializeField]
    private float timeToNonTimeExercise;
    [SerializeField]
    private float timeToNonRestExercise;
    [SerializeField]
    private TrainingScreen[] trainingScreens;

    [Space(10f)]

    [SerializeField]
    private TrainingData[] data;

    // Hidden
    [HideInInspector]
    public TrainingStage trainingStage;

    // Cached Components
    private TrainingScreen targetScreen;
    private TrainingData cachedData;

    // Coroutines
    private IEnumerator nonTimeExercise;
    private IEnumerator rest;
    private IEnumerator nonRest;

    // Singleton!
    public static Training Singleton
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
        Observer.Singleton.onTestEnd += SetTrainingData;

        // Training screens Events
        Observer.Singleton.onWarmingUpScreenEnd += SetWarmingUp;
        Observer.Singleton.onTrainingScreenEnd += SetTraining;
        Observer.Singleton.onStretchingScreenEnd += SetStretching;

        // OnTimerDone Events
        foreach (TrainingScreen screen in trainingScreens)
        {
            screen.timer.gameObject.SetActive(false);
            
            // Timer events
            Observer.Singleton.onTimerDone += screen.ActiveContinue;
            Observer.Singleton.onTimerDone += screen.FadeInContinue;
            Observer.Singleton.onTimerDone += StopWatchSound;
        }
    }

    public void Ready()
    {
        if (ReadyIsAlreadyressed())
            return;

        // Ready execution
        if (targetScreen.ActualExerciseHasTime())
            ExecuteTimeExercise();
        else
            ExecuteNonTimeExercise();
    }

    public void Continue()
    {
        if (ContinueIsAlreadyPressed())
            return;

        // Continue execution
        if (targetScreen.ActualExerciseHasRest())
            ExecuteRest();
        else
            ExecuteNonRest();
    }

    // *** SET TRAINING DATA ***

    public void SetTrainingData()
    {
        TrainingLevel level = DataManager.Singleton.trainingLevel;

        switch (level)
        {
            case TrainingLevel.Begginer:
                SetBegginerLevel();
                break;

            case TrainingLevel.Rookie:
                SetRookieLevel();
                break;

            case TrainingLevel.Medium:
                SetMediumLevel();
                break;

            case TrainingLevel.Advance:
                SetAdavanceLevel();
                break;

            default:
                break;
        }
    }

    private void SetBegginerLevel()
    {
        // Setting training level data
        cachedData = data[0];

        AllocateData();
    }

    private void SetRookieLevel()
    {
        cachedData = data[1];

        AllocateData();
    }

    private void SetMediumLevel()
    {
        cachedData = data[2];

        AllocateData();
    }

    private void SetAdavanceLevel()
    {
        cachedData = data[3];

        AllocateData();
    }

    private void AllocateData()
    {
        for (int i = 0; i < trainingScreens.Length; i++)
        {
            trainingScreens[i].data = cachedData.trainingScreens[i];
        }
    }

    // *** GENERAL FUNCTIONS ***

    private void SetWarmingUp()
    {
        if (enableConsoleLog)
            Debug.Log("Training :: SetWarmingUp");

        trainingStage = TrainingStage.WarmingUp;

        targetScreen = trainingScreens[0];
        targetScreen.SetupScreen();

        CheckButtonsStatus();
    }

    private void SetTraining()
    {
        if (enableConsoleLog)
            Debug.Log("Training :: SetTraining");

        trainingStage = TrainingStage.Training;

        targetScreen = trainingScreens[1];
        targetScreen.SetupScreen();

        CheckButtonsStatus();
    }

    private void SetStretching()
    {
        if (enableConsoleLog)
            Debug.Log("Training :: SetStretching");

        trainingStage = TrainingStage.Stretching;

        targetScreen = trainingScreens[2];
        targetScreen.SetupScreen();

        CheckButtonsStatus();
    }

    private bool ReadyIsAlreadyressed()
    {
        // Constraints
        if (targetScreen.readyIsAlreadyPressed)
            return true;

        if (targetScreen.continueIsAlreadyPressed)
            targetScreen.continueIsAlreadyPressed = false;

        targetScreen.readyIsAlreadyPressed = true;

        return false;
    }

    private bool ContinueIsAlreadyPressed()
    {
        // Constraints
        if (targetScreen.continueIsAlreadyPressed)
            return true;

        if (targetScreen.readyIsAlreadyPressed)
            targetScreen.readyIsAlreadyPressed = false;

        targetScreen.continueIsAlreadyPressed = true;

        return false;
    }

    private void CheckButtonsStatus()
    {
        //Setting buttons
        if (!targetScreen.readyButton.gameObject.activeInHierarchy)
            targetScreen.SetActiveReady(true);

        if (targetScreen.continueButton.gameObject.activeInHierarchy)
            targetScreen.SetActiveContinue(false);
    }

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
    } // TODO Execute Rest.

    private void ExecuteNonRest()
    {
        // Coroutine execution
        if (nonRest != null)
            StopCoroutine(nonRest);

        nonRest = NonRest();

        StartCoroutine(nonRest);
    }

    private void StopWatchSound()
    {
        AudioManager.Singleton.effects[0].Play(); // Alarm
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

        yield return null;

        Observer.Singleton.OnRestStart();

        targetScreen.SetActiveReady(true);
        targetScreen.SetActiveContinue(false);
        targetScreen.SetupExercise();

        // Fade event!
        if (targetScreen.rootParent.activeInHierarchy)
            Fader.Singleton.FadeScreen(targetScreen.rootParent);
    }

    private IEnumerator NonRest()
    {
        if (enableConsoleLog)
            Debug.Log("Training :: NonRest");

        targetScreen.FadeOutContinue();

        yield return new WaitForSeconds(timeToNonRestExercise);

        targetScreen.SetActiveReady(true);
        targetScreen.SetActiveContinue(false);
        targetScreen.SetupExercise();

        // Fade event!
        if (targetScreen.rootParent.activeInHierarchy)
            Fader.Singleton.FadeScreen(targetScreen.rootParent);
    }

    #endregion
}
