// <copyright file="TrainingScreen.cs" company="Up Up Down Studios">
// Copyright (c) 2018 All Rights Reserved
// </copyright>
// <summary>Represent a training screen object.</summary>

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

[System.Serializable]
public class TrainingScreen
{
    #region Properties

    public GameObject rootParent;
    [SerializeField]
    private Text exerciseName;
    [SerializeField]
    private Text exerciseDescription;
    [SerializeField]
    private VideoPlayer videoPlayer;
    
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
    public TrainingScreenData data;

    public int actualExercise
    {
        get; private set;
    }

    #endregion

    #region Class functions

    public void SetupExercise()
    {
        // Increase index
        if (actualExercise >= 0 && actualExercise < data.exercises.Length - 1)
            actualExercise++;
        else
        {
            switch (Training.Singleton.trainingStage)
            {
                case TrainingStage.WarmingUp:
                    Observer.Singleton.OnTrainingScreenStart();
                    break;

                case TrainingStage.Training:
                    Observer.Singleton.OnStretchingScreenStart();
                    break;

                case TrainingStage.Stretching:
                    Observer.Singleton.OnWarmingUpScreenEnd();
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
        exerciseDescription.text = data.exercises[actualExercise].description;

        videoPlayer.clip = data.exercises[actualExercise].videoClip;

        if (!videoPlayer.isPlaying)
            videoPlayer.Play();
    }

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

    public void ExecuteTimer()
    {
        SetActiveReady(false);

        timer.gameObject.SetActive(true);
        timer.ExecuteWatch(data.exercises[actualExercise].time);
    }

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

    #endregion
}
