// <copyright file="DeveloperTools.cs" company="Up Up Down Studios">
// Copyright (c) 2018 All Rights Reserved
// </copyright>
// <summary>Class for development purposes.</summary>

using UnityEngine;

public class DeveloperTools : MonoBehaviour {

    #region Properties

    [SerializeField]
    private float timeScale;

    // Hidden
    private float lastTimeScale;

    #endregion

    #region Unity functions

    private void Awake()
    {
        SetScale();
    }

    private void Update()
    {
        if (timeScale != lastTimeScale)
        {
            SetScale();

            lastTimeScale = timeScale;
        }
    }

    #endregion

    #region Class functions

    private void SetScale()
    {
        Time.timeScale = timeScale;
    }

    #endregion
}
