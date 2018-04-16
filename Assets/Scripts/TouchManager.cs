using System;
using System.Collections;
using UnityEngine;

public enum TouchGestures
{
    Tap, DoubleTap
}

public class TouchManager : MonoBehaviour {

    #region Properties

    [SerializeField]
    private bool enableConsoleLog = true;

    [Space(10f)] [Header("Tap Settings")]

    [SerializeField]
    private float _doubleTapTime = 0.25f;

    /// <summary>
    /// Represent maximun amount of time to recognize a double tap.
    /// </summary>
    public static float doubleTapTime
    {
        get; private set;
    }

    // Hidden
    private float startTouchTime;

    // Events
    /// <summary>
    /// Static Action for tap events.
    /// </summary>
    public static Action OnTap;
    /// <summary>
    /// Static Action for double taps events.
    /// </summary>
    public static Action OnDoubleTap;

    // Static Elements
    private static UnityEngine.Touch currentTouch;
    private static UnityEngine.Touch lastTouch;
    
    /// <summary>
    /// Current touch reference.
    /// </summary>
    public static UnityEngine.Touch CurrentTouch
    {
        get
        {
            return currentTouch;
        }
    }
    /// <summary>
    /// Last active touch reference.
    /// </summary>
    public static UnityEngine.Touch LastTouch
    {
        get
        {
            return lastTouch;
        }
    }

    // Enums
    /// <summary>
    /// Actual active gesture.
    /// </summary>
    public static TouchGestures Gesture
    {
        get; private set;
    }

    #endregion

    #region Unity functions

    private void Start()
    {
        Setup();
    }

    private void Update()
    {
        #if UNITY_EDITOR
            TapEditor();
        #elif UNITY_ANDROID
            Tap();
        #elif UNITY_IOS
            Tap();
        #endif
    }

    #endregion

    #region Class functions

    private void Setup()
    {
        // Tap settings
        doubleTapTime = _doubleTapTime;
    }

    private void Tap()
    {
        currentTouch = Input.GetTouch(0);

        // Touch detection
        if (currentTouch.phase == TouchPhase.Began)
        {
            // Double touch detection
            if (startTouchTime != 0 && Time.time - startTouchTime < doubleTapTime)
            {
                Gesture = TouchGestures.DoubleTap;

                if (enableConsoleLog)
                    Debug.Log(string.Format("TouchManager :: {0}", Gesture));
                if (OnDoubleTap != null)
                    OnDoubleTap();

                startTouchTime = 0;
            }
            else
            {
                Gesture = TouchGestures.Tap;

                if (enableConsoleLog)
                    Debug.Log(string.Format("TouchManager :: {0}", Gesture));
                if (OnTap != null)
                    OnTap();
            }

            startTouchTime = Time.time;
        }
    }

    private void TapEditor()
    {
        if (Input.GetMouseButtonDown(0))
        {
            // Double touch detection
            if (startTouchTime != 0 && Time.time - startTouchTime < doubleTapTime)
            {
                Gesture = TouchGestures.DoubleTap;

                if (enableConsoleLog)
                    Debug.Log(string.Format("TouchManager :: {0}", Gesture));
                if (OnDoubleTap != null)
                    OnDoubleTap();

                startTouchTime = 0;
            }
            else
            {
                Gesture = TouchGestures.Tap;

                if (enableConsoleLog)
                    Debug.Log(string.Format("TouchManager :: {0}", Gesture));
                if (OnTap != null)
                    OnTap();
            }

            startTouchTime = Time.time;
        }
    }

    #endregion
}
