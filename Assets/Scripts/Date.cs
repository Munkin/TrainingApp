// <copyright file="DateManager.cs" company="Up Up Down Studios">
// Copyright (c) 2018 All Rights Reserved
// </copyright>
// <summary>Date data container.</summary>

using System;
using UnityEngine;

[CreateAssetMenu(fileName = "New Date", menuName = "Assets/Date", order = 3)]
public class Date : ScriptableObject { // NOTE DateTime default values is : 1/1/0001 12:00:00 AM

    #region Properties

    public string currentDateChunck;
    public string lastDateChunck;

    // Date Control
    public DateTime currentDate;
    public DateTime lastDate;

    // Hidden
    private bool enableWarningConsoleLog;

    #endregion

    #region Class functions

    public void SetDate(DateTime currentDate, DateTime lastDate)
    {
        SetCurrentDate(currentDate);
        SetLastDate(lastDate);

        Observer.Singleton.OnDateSet();
    }

    public void SetCurrentDate(DateTime currentDate)
    {
        currentDateChunck = currentDate.ToString();

        this.currentDate = currentDate;
    }

    public void SetLastDate(DateTime lastDate)
    {
        lastDateChunck = lastDate.ToString();

        this.lastDate = lastDate;
    }

    public Date GetDate()
    {
        if (currentDateChunck == "")
            TryParseCurrentDate();

        if (lastDateChunck == "")
            TryParseLastDate();

        Observer.Singleton.OnDateGet();

        return this;
    }

    public DateTime GetCurrentDate()
    {
        TryParseCurrentDate();

        return currentDate;
    }

    public DateTime GetLastDate()
    {
        TryParseLastDate();

        return lastDate;
    }

    private void TryParseCurrentDate()
    {
        try
        {
            DateTime.TryParse(currentDateChunck, out currentDate);
        }
        catch (Exception)
        {
            if (enableWarningConsoleLog)
                Debug.LogWarning("Date :: TryParseCurrentDate :: Canot convert currentDateChunck to currentDate");
        }
    }

    private void TryParseLastDate()
    {
        try
        {
            DateTime.TryParse(lastDateChunck, out lastDate);
        }
        catch (Exception)
        {
            if (enableWarningConsoleLog)
                Debug.LogWarning("Date :: TryParseLastDate :: Canot convert lastDateChunck to lastDate");
        }
    }

    #endregion
}
