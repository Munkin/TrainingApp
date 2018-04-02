// <copyright file="TrainingData.cs" company="Up Up Down Studios">
// Copyright (c) 2018 All Rights Reserved
// </copyright>
// <summary>Training data container.</summary>

using UnityEngine;

[CreateAssetMenu(fileName = "TrainingData", menuName = "Assets/TrainingData", order = 2)]
public class TrainingData : ScriptableObject {

    #region Properties

    [Tooltip("Set the training day")]
    public TrainingDay day;
    [Tooltip("Set the training level")]
    public TrainingLevel level;

    [Space(10f)]

    public TrainingScreenData[] trainingScreens;

    #endregion

    #region Unity function

    public void SetNewDay(TrainingDay day)
    {
        this.day = day;

        switch (day)
        {
            case TrainingDay.One:
                DayOneWeight();
                break;

            case TrainingDay.Two:
                DayTwoWeight();
                break;

            case TrainingDay.Three:
                DayThreeWeight();
                break;

            case TrainingDay.Four:
                DayFourWeight();
                break;

            case TrainingDay.Five:
                DayFiveWeight();
                break;

            case TrainingDay.Six:
                DaySixWeight();
                break;

            case TrainingDay.Seven:
                DaySevenWeight();
                break;

            default:
                break;
        }
    }

    private void DayOneWeight()
    {
        // TODO Day one weight
    }

    private void DayTwoWeight()
    {
        // TODO Day two weight
    }

    private void DayThreeWeight()
    {
        // TODO Day three weight
    }

    private void DayFourWeight()
    {
        // TODO Day four weight
    }

    private void DayFiveWeight()
    {
        // TODO Day five weight
    }

    private void DaySixWeight()
    {
        // TODO Day six weight
    }

    private void DaySevenWeight()
    {
        // TODO Day seven weight
    }

    #endregion
}
