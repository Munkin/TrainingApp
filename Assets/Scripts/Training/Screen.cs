// <copyright file="Screen.cs" company="Up Up Down Studios">
// Copyright (c) 2018 All Rights Reserved
// </copyright>
// <summary>Represent a training screen object.</summary>

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

[System.Serializable]
public class Screen
{
    #region Properties

    [SerializeField]
    private bool enableConsoleLog;

    [Space(10f)]

    public GameObject rootParent;
    // ***
    public Text exerciseName;
    public Text exerciseDescription;
    public Text exerciseDescriptionBig;
    // ***
    public VideoPlayer videoPlayer;

    public Button readyButton;
    public Button continueButton;

    [Space(10f)]

    public Timer timer;

    // Hidden
    [HideInInspector]
    public bool readyIsAlreadyPressed;
    [HideInInspector]
    public bool continueIsAlreadyPressed;
    [HideInInspector]
    public ScreenData data;

    public int actualExercise
    {
        get; private set;
    }

    #endregion

    #region Class functions

    public void SetupExercise()
    {
        // Increase index.
        if (actualExercise >= 0 && actualExercise < data.exercises.Length - 1)
            actualExercise++;
        else
        {
            switch (TrainingManager.Singleton.trainingStage)
            {
                case TrainingStage.WarmingUp:
                    Observer.Singleton.OnTrainingScreen();
                    break;

                case TrainingStage.Training:
                    Observer.Singleton.OnStretchingScreen();
                    break;

                case TrainingStage.Stretching:
                    Observer.Singleton.OnInfoScreen();
                    break;

                default:
                    break;
            }

            actualExercise = 0;
        }

        SetupScreen();
    }

    public void SetupScreen()
    {
        exerciseName.text = data.exercises[actualExercise].name;

        ResetContinue();
        ResetReady();

        if (data.exercises[actualExercise].videoClip != null)
            SetupVideoExercise();
        else
            SetupNoVideoExercise();
    }

    public void ExecuteTimer()
    {
        SetActiveReady(false);

        timer.gameObject.SetActive(true);
        timer.ExecuteWatch(data.exercises[actualExercise].time);
    }

    private void SetupVideoExercise()
    {
        if (enableConsoleLog)
            Debug.Log("Screen :: SetupVideoExercise");

        // Turn-On the big text.
        if (exerciseDescriptionBig.gameObject.activeInHierarchy)
            exerciseDescriptionBig.gameObject.SetActive(false);

        // Turn-Off the normal text.
        if (!exerciseDescription.gameObject.activeInHierarchy)
            exerciseDescription.gameObject.SetActive(true);

        // Turn-Off the video.
        videoPlayer.gameObject.SetActive(true);

        // Screen setup
        exerciseDescription.text = data.exercises[actualExercise].description;
        exerciseDescriptionBig.text = "";

        videoPlayer.clip = data.exercises[actualExercise].videoClip;

        // Is there a video asigned ?
        if (!videoPlayer.isPlaying)
            videoPlayer.Play();

        SetActiveReady(true);
        SetActiveContinue(false);
    }

    private void SetupNoVideoExercise()
    {
        if (enableConsoleLog)
            Debug.Log("Screen :: SetupNoVideoExercise");

        // Turn-On the big text.
        if (!exerciseDescriptionBig.gameObject.activeInHierarchy)
            exerciseDescriptionBig.gameObject.SetActive(true);

        // Turn-Off the normal text.
        if (exerciseDescription.gameObject.activeInHierarchy)
            exerciseDescription.gameObject.SetActive(false);

        // Turn-Off the video.
        videoPlayer.gameObject.SetActive(false);

        // Screen setup
        exerciseDescription.text = "";
        exerciseDescriptionBig.text = data.exercises[actualExercise].description;

        SetActiveReady(false);
        SetActiveContinue(true);
    }

    // ***

    public void ActiveContinue()
    {
        SetActiveContinue(true);
    }

    public void SetActiveReady(bool state)
    {
        readyButton.interactable = state;
        readyButton.transform.parent.gameObject.SetActive(state);
    }

    public void SetActiveContinue(bool state)
    {
        continueButton.interactable = state;
        continueButton.transform.parent.gameObject.SetActive(state);
    }

    // ***

    public void FadeOutReady()
    {
        // Fade event!
        Fader.Singleton.FadeOutButton(readyButton.gameObject);
    }

    public void FadeInContinue()
    {
        // Fade event!
        Fader.Singleton.FadeInButton(continueButton.gameObject);
    }

    public void FadeOutContinue()
    {
        // Fade event!
        Fader.Singleton.FadeOutButton(continueButton.gameObject);
    }

    // ***

    public bool ActualExerciseHasRest()
    {
        if (data.exercises[actualExercise].restTime < 0)
            data.exercises[actualExercise].restTime = 0;

        // Has the current exercise rest time setted ?
        if (data.exercises[actualExercise].restTime != 0)
            return true;
        else
            return false;
    }

    public bool ActualExerciseHasTime()
    {
        if (data.exercises[actualExercise].time < 0)
            data.exercises[actualExercise].time = 0;

        // Has the current exercise rest time setted ?
        if (data.exercises[actualExercise].time != 0)
            return true;
        else
            return false;
    }

    // ***

    public bool ReadyWasAlreadyPressed()
    {
        bool returnedValue = readyIsAlreadyPressed;

        if (!readyIsAlreadyPressed)
            readyIsAlreadyPressed = true;

        return returnedValue;
    }

    public bool ContinueWasAlreadyPressed()
    {
        bool returnedValue = continueIsAlreadyPressed;

        if (!continueIsAlreadyPressed)
            continueIsAlreadyPressed = true;

        return returnedValue;
    }

    public void ResetReady()
    {
        if (readyIsAlreadyPressed)
            readyIsAlreadyPressed = false;
    }

    public void ResetContinue()
    {
        if (continueIsAlreadyPressed)
            continueIsAlreadyPressed = false;
    }

    #endregion
}
