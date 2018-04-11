﻿// <copyright file="TrainingData.cs" company="Up Up Down Studios">
// Copyright (c) 2018 All Rights Reserved
// </copyright>
// <summary>Training data container.</summary>

using UnityEngine;

[CreateAssetMenu(fileName = "New TrainingData", menuName = "Assets/TrainingData", order = 2)]
public class TrainingData : ScriptableObject {

    #region Properties

    [Tooltip("Set the training level")]
    public TrainingLevel trainingLevel;
    [Tooltip("Set the training day")]
    public TrainingDay trainingDay;

    [Space(10f)]

    public ScreenData[] screensData;

    #endregion
}
