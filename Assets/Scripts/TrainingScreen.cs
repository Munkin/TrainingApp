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

    [SerializeField]
    private Text exerciseName;
    [SerializeField]
    private Text exerciseDescription;
    [SerializeField]
    private VideoPlayer videoPlayer;
    [SerializeField]
    private Button readyButton;
    [SerializeField]
    private Button continueButton;

    [Space(10f)]

    public Exercise[] exercises;

    [Space(10f)]

    public Timer timer;

    // Hidden
    public int actualExercise
    {
        get; private set;
    }

    #endregion

    // TODO Make frame

    #region Class functions

    public void SetupExercise()
    {
        // Increase index
        if (actualExercise >= 0 && actualExercise < exercises.Length - 1)
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
        exerciseName.text = exercises[actualExercise].name;
        exerciseDescription.text = exercises[actualExercise].description;

        videoPlayer.clip = exercises[actualExercise].videoClip;
        videoPlayer.Play();
    } // TODO Optimize video player in mobiles

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
        timer.ExecuteWatch(exercises[actualExercise].time);
    }

    public bool ActualExerciseHasRest()
    {
        if (exercises[actualExercise].restTime < 0)
            exercises[actualExercise].restTime = 0;

        if (exercises[actualExercise].restTime != 0)
            return true;
        else
            return false;
    }

    public bool ActualExerciseHasTime()
    {
        if (exercises[actualExercise].time < 0)
            exercises[actualExercise].time = 0;

        if (exercises[actualExercise].time != 0)
            return true;
        else
            return false;
    }

    #endregion
}
