// <copyright file="DateManager.cs" company="Up Up Down Studios">
// Copyright (c) 2018 All Rights Reserved
// </copyright>
// <summary>DateManger for Unity date events.</summary>

using System;
using UnityEngine;

public class DateManager : MonoBehaviour { // NOTE DateTime default values is : 1/1/0001 12:00:00 AM

    #region Properties

    [SerializeField]
    private Date date;

    // Date variables
    public DateTime currentDate
    {
        get; private set;
    }
    public DateTime lastDate
    {
        get; private set;
    }

    // Singleton!
    public static DateManager Singleton
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

        GetDates();
    }

    #endregion

    #region Class functions

    public void CheckDate()
    {
        SetLastDate(currentDate);
        SetCurrentDate();
    }

    public bool HasPassOneDaySinceLastTraining()
    {
        // Has pass one day since last training ?
        return (currentDate.Year != lastDate.Year || currentDate.DayOfYear != lastDate.DayOfYear);
    }

    private void GetDates()
    {
        currentDate = date.GetCurrentDate();
        lastDate = date.GetLastDate();
    }

    private void SetCurrentDate()
    {
        currentDate = DateTime.UtcNow;

        date.SetCurrentDate(currentDate);
    }

    private void SetLastDate(DateTime lastDate)
    {
        this.lastDate = lastDate;

        date.SetLastDate(this.lastDate);
    }

    #endregion
}
