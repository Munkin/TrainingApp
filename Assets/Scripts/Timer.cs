// <copyright file="Timer.cs" company="Up Up Down Studios">
// Copyright (c) 2018 All Rights Reserved
// </copyright>
// <summary>Custom stopwatch for time count behaviours.</summary>

using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour {

    #region Properties

    public float seconds;
    public float minutes;

    [Space(10f)]

    [SerializeField]
    private Text timeText;

    // Time control
    private float time;
    private float timeUpdate;
    private float timeLimit;

    // Coroutines
    private IEnumerator timeCoroutine;

    #endregion

    #region Class functions

    public void ExecuteWatch(float timeLimit = float.PositiveInfinity)
    {
        ResetWatch();

        this.timeLimit = timeLimit;

        // Coroutine execution
        if (timeCoroutine != null)
            StopCoroutine(timeCoroutine);

        timeCoroutine = TimeCoroutine();

        StartCoroutine(timeCoroutine);
    }

    public void StopWatch()
    {
        ResetWatch();

        if (timeCoroutine != null)
            StopCoroutine(timeCoroutine);

        gameObject.SetActive(false);
    }

    private void ResetWatch()
    {
        time = Time.time - timeUpdate;

        timeUpdate = 0;
        seconds = 0;
        minutes = 0;
    }

    #endregion

    #region Coroutines

    private IEnumerator TimeCoroutine()
    {
        while (isActiveAndEnabled)
        {
            timeUpdate = Time.time - time;

            minutes = (int)timeUpdate / 60;
            seconds = (int)timeUpdate % 60;

            timeText.text = minutes.ToString("00") + ":" + seconds.ToString("00");

            if (timeUpdate >= timeLimit)
            {
                Observer.Singleton.OnTimerDone();

                yield return null;

                StopWatch();
            }

            yield return null;
        }
    }

    #endregion
}
