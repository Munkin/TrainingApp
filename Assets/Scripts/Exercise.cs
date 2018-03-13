// <copyright file="Exercise.cs" company="Up Up Down Studios">
// Copyright (c) 2018 All Rights Reserved
// </copyright>
// <summary>Represent a custom training exercise.</summary>

using UnityEngine;
using UnityEngine.Video;

[System.Serializable]
public class Exercise
{
    #region Properties

    public string name;
    public string description;
    public float time;
    public float restTime;

    [Space(10f)]
    
    public VideoClip videoClip;

    #endregion
}
