// <copyright file="Training.cs" company="Up Up Down Studios">
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

public class Training : MonoBehaviour {

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
    private Screen[] trainingScreens; // TODO Refactor in the future. DO NOT REFACTOR NOW! (Inspector Data lose in the process ): ).

    [Space(10f)]

    [SerializeField]
    private UpdateTraining updateTraining;

    // Hidden
    [HideInInspector]
    public TrainingStage trainingStage;

    // Consts
    public const float fadeFixTime = 0.125f;

    // Cached Components
    public TrainingData cachedTrainingData
    {
        get; private set;
    }
    private Screen targetScreen;

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

        // Training screens events.
        Observer.Singleton.onWarmingUpScreenEnd += SetWarmingUp;
        Observer.Singleton.onTrainingScreenEnd += SetTraining;
        Observer.Singleton.onStretchingScreenEnd += SetStretching;

        // OnTimerDone events.
        foreach (Screen screen in trainingScreens)
        {
            screen.timer.gameObject.SetActive(false);
            
            // Timer events.
            Observer.Singleton.onTimerDone += screen.ActiveContinue;
            Observer.Singleton.onTimerDone += screen.FadeInContinue;
            Observer.Singleton.onTimerDone += StopWatchSound;
        }
    }

    // ***

    public void Ready()
    {
        if (ReadyIsAlreadyPressed())
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

    public void NotifyNewTrainingData(TrainingData trainingData)
    {
        cachedTrainingData = trainingData;

        if (enableConsoleLog)
            Debug.Log(string.Format("Training :: NotifyNewTrainingData :: {0}", trainingData.name));
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
        cachedTrainingData = Resources.Load("TrainingData/Begginer/Begginer_Day1") as TrainingData;

        AllocateData();
    }

    private void SetRookieFirstDay()
    {
        // Setting training level data.
        cachedTrainingData = Resources.Load("TrainingData/Rookie/Rookie_Day1") as TrainingData;

        AllocateData();
    }

    private void SetMediumFirstDay()
    {
        // Setting training level data.
        cachedTrainingData = Resources.Load("TrainingData/Medium/Medium_Day1") as TrainingData;

        AllocateData();
    }

    private void SetAdvanceFirstDay()
    {
        // Setting training level data.
        cachedTrainingData = Resources.Load("TrainingData/Advance/Advance_Day1") as TrainingData;

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

    private bool ReadyIsAlreadyPressed()
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
        targetScreen.SetActiveReady(true);
        targetScreen.SetActiveContinue(false);
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

    // ***

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
