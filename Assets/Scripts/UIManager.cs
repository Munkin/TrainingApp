﻿// <copyright file="UIManager.cs" company="Up Up Down Studios">
// Copyright (c) 2018 All Rights Reserved
// </copyright>
// <summary>Manager for Unity user interface events.</summary>

using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class UIManager : MonoBehaviour {

    #region Properties

    [SerializeField]
    private bool enableConsoleLog;

    [Space(10f)] [Header("Screens")]

    public GameObject[] screens;
    /*
     * 0. Interlude
     * 1. Data
     * 2. Exercise Data
     * 3. Complete Test
     * 4. Training
     * 5. Rest
     * 6. Menu
    */

    [Space(10f)]

    [SerializeField]
    private float timeToFadeIn;
    [SerializeField]
    private float timeToFadeOut;
    [SerializeField]
    private float timeToWaitForFade;

    [Space(10f)] [Header("Screen: Interlude")]

    [SerializeField]
    private Text interludeText;
    [SerializeField]
    private string[] interludeTexts;

    [Space(10f)] [Header("Screen: Data Screen")]

    [SerializeField]
    private Button continueButton;

    [Space(10f)] [Header("Screen: Exercise Data")]

    [SerializeField]
    private Text questionText;
    [SerializeField]
    private Text[] optionTexts;
    [SerializeField]
    private Toggle[] optionToggles;

    [Space(10f)] [Header("Screen: Complete Test")]

    [SerializeField]
    private Text complexionText;
    [SerializeField]
    private Text trainingText;
    [SerializeField]
    private string[] completeTestTexts;

    [Space(10f)] [Header("Screen: Training")]

    public GameObject[] trainingScreens;
    [SerializeField]
    private string[] warmUpTexts;
    [SerializeField]
    private string[] trainingTexts;
    [SerializeField]
    private string[] stretchingTexts;

    [Space(10f)] [Header("Screen: Rest")]

    [SerializeField]
    private string[] restTexts;

    [Space(10f)] [Header("Interlude: Training End")]

    [SerializeField]
    private string[] trainingEndTexts;

    [Space(10f)] [Header("Interlude: Already Opened")]

    [SerializeField]
    private string[] alreadyOpenedTexts;

    [Space(10f)] [Header("Interlude: Daily Training")]

    [SerializeField]
    private string[] dailyTrainingTexts;

    // Hidden
    private int textIndex = 0;
    private int question = 0;

    // Button State Control
    private bool continueIsAlreadyPressed;
    private bool nextQuestionIsAlreadyPressed;
    private bool continueToTrainingIsAlreadyPressed;

    // Singleton!
    public static UIManager Singleton
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

        Suscribe();
    }

    private void Start()
    {
        Setup();
    }

    #endregion

    #region Class functions

    private void Setup()
    {
        EnableScreen(0);
    }

    private void Suscribe()
    {
        // *** GENERAL EVENTS ***

        Observer.Singleton.onIntroductionScreen += FadeInInterlude;
        Observer.Singleton.onDataScreen += EnableDataScreen;
        Observer.Singleton.onDataScreen += ResetInterludeFadeValues;
        Observer.Singleton.onExerciseDataScreen += EnableExerciseDataScreen;
        // OnTestResult events.
        Observer.Singleton.onTestResult += EnableIntroductionScreen;
        Observer.Singleton.onTestResult += FadeInResult;
        // OnTestEnd events.
        Observer.Singleton.onTestEnd += EnableCompleteTestScreen;
        Observer.Singleton.onTestEnd += ShowResults;
        Observer.Singleton.onTestEnd += ResetResultFadeValues;
        // OnAppWasAlreadyOpenedToday events.
        Observer.Singleton.onAppWasAlreadyOpenedToday += SetAlreadyOpenedFirstText;
        Observer.Singleton.onAppWasAlreadyOpenedToday += FadeInAlreadyOpened;
        Observer.Singleton.onAppWasAlreadyOpenedTodayEnd += ResetAlreadyOpenedFadeValues;
        // OnDailyTraining events.
        Observer.Singleton.onDailyTraining += SetDailyTrainingFirstText;
        Observer.Singleton.onDailyTraining += FadeInDailyTraining;
        Observer.Singleton.onDailyTrainingEnd += ResetDailyTrainingFadeValues;
        // OnDailyTraining special events.
        Observer.Singleton.onDailyTrainingEnd += () => StartCoroutine(WaitForTraining());

        // *** TRAINING EVENTS ***

        //OnTrainingEnd events.
        Observer.Singleton.onTrainingEnd += EnableIntroductionScreen;
        Observer.Singleton.onTrainingEnd += FadeInTrainingEnd;
        Observer.Singleton.onAppEnd += ResetTrainingEndFadeValues;

        // OnTrainingStart events.
        Observer.Singleton.onWarmingUpScreenStart += EnableIntroductionScreen;
        Observer.Singleton.onWarmingUpScreenStart += FadeInToWarmingUp;
        Observer.Singleton.onWarmingUpScreenEnd += EnableWarmingUp;
        Observer.Singleton.onWarmingUpScreenEnd += ResetWarmingUpFadeValues;
        // OnWarmingUpScreen events.
        Observer.Singleton.onTrainingScreenStart += EnableIntroductionScreen;
        Observer.Singleton.onTrainingScreenStart += FadeInToTraining;
        Observer.Singleton.onTrainingScreenEnd += EnableTraining;
        Observer.Singleton.onTrainingScreenEnd += ResetTrainingFadeValues;
        // OnTrainingScreen events.
        Observer.Singleton.onStretchingScreenStart += EnableIntroductionScreen;
        Observer.Singleton.onStretchingScreenStart += FadeInToStretching;
        Observer.Singleton.onStretchingScreenEnd += EnableStretching;
        Observer.Singleton.onStretchingScreenEnd += ResetStretchingFadeValues;

        // *** REST EVENTS ***

        Observer.Singleton.onRestStart += EnableIntroductionScreen;
        Observer.Singleton.onRestStart += FadeInRest;
        Observer.Singleton.onRestEnd += ResetRestFadeValues;

        // *** BUTTON CONTROL EVENTS ***

        Observer.Singleton.onExerciseDataScreen += ContinueCanBePressedAgain;
        Observer.Singleton.onWarmingUpScreenStart += ContinueToTrainingCanBePressedAgain;
    }
    
    // *** DATA SCREEN ***

    public void Continue()
    {
        // Is the button already pressed ?
        if (continueIsAlreadyPressed)
            return;

        continueIsAlreadyPressed = true;

        Observer.Singleton.OnExerciseDataScreen();
    }

    public void EnableContinueButton()
    {
        if (!screens[1].activeInHierarchy)
            return;

        if (!continueButton.interactable)
            continueButton.interactable = true;
    }

    public void DisableContinueButton()
    {
        if (!screens[1].activeInHierarchy)
            return;

        if (continueButton.interactable)
            continueButton.interactable = false;
    }

    // *** EXERCISE DATA SCREEN ***

    public void NextQuestion()
    {
        // Is the button already pressed ?
        if (nextQuestionIsAlreadyPressed)
            return;

        nextQuestionIsAlreadyPressed = true;

        // Suscribing in to fade event
        Observer.Singleton.onScreenFadeCallback += NextQuestionCanBePressedAgain;

        Fader.Singleton.FadeScreen(screens[2]);

        CheckSelectedOption();

        // Setting new question
        if (question >= 4)
        {
            Observer.Singleton.OnTestResult();

            // Unsuscribing from fade event
            Observer.Singleton.onScreenFadeCallback -= NextQuestionCanBePressedAgain;
        }
        else
        {
            question++;

            SetQuestion();
        }
    }

    private void SetQuestion()
    {
        switch (question)
        {
            case 0:
                questionText.text = Question("DOMINADAS", 10);
                SetQuestionOptions(1, new int[] { 2, 3 }, new int[] { 4, 9 }, 10);
                break;

            case 1:
                questionText.text = Question("SENTADILLAS", 10);
                SetQuestionOptions(15, new int[] { 16, 30 }, new int[] { 31, 45 }, 46);
                break;

            case 2:
                questionText.text = Question("FLEXIONES", 10);
                SetQuestionOptions(3, new int[] { 4, 8 }, new int[] { 9, 15 }, 16);
                break;

            case 3:
                questionText.text = Question("PLANCHAS", 10);
                SetQuestionOptions(15, new int[] { 16, 30 }, new int[] { 31, 45 }, 46);
                break;

            case 4:
                questionText.text = Question("TRICEPS", 10, false);
                SetQuestionOptions(4, new int[] { 5, 10 }, new int[] { 11, 20 }, 21);
                break;

            default:
                break;
        }
    }

    private void SetQuestionOptions(int firstOption, int[] secondOption, int[] thirdOption, int fourthOption)
    {
        if (secondOption.Length != 2 || thirdOption.Length != 2)
            return;

        for (int i = 0; i < optionTexts.Length; i++)
        {
            switch (i)
            {
                case 0:
                    optionTexts[i].text = string.Format("{0} o menos", firstOption.ToString());
                    break;

                case 1:
                    optionTexts[i].text = string.Format("Entre {0} y {1}", secondOption[0].ToString(), secondOption[1].ToString());
                    break;

                case 2:
                    optionTexts[i].text = string.Format("Entre {0} y {1}", thirdOption[0].ToString(), thirdOption[1].ToString());
                    break;

                case 3:
                    optionTexts[i].text = string.Format("{0} o más", fourthOption.ToString());
                    break;

                default:
                    break;
            }
        }
    }

    private void CheckSelectedOption()
    {
        for (int i = 0; i < optionToggles.Length; i++)
        {
            if (optionToggles[i].isOn)
            {
                switch (i)
                {
                    case 0:
                        DataManager.Singleton.AddScore(1);
                        break;
                    case 1:
                        DataManager.Singleton.AddScore(3);
                        break;
                    case 2:
                        DataManager.Singleton.AddScore(5);
                        break;
                    case 3:
                        DataManager.Singleton.AddScore(7);
                        break;
                }

                // Reset the toogles
                optionToggles[i].isOn = false;
                optionToggles[0].isOn = true;

                break;
            }
        }
    }

    private string Question(string exercise, int time, bool isGrammaticalFemale = true)
    {
        if (isGrammaticalFemale)
            return string.Format("¿Cuantas {0} haces en {1} segundos?", exercise, time.ToString());
        else
            return string.Format("¿Cuantos {0} haces en {1} segundos?", exercise, time.ToString());
    }

    // *** COMPLETE TEST SCREEN ***

    public void ContinueToTraining()
    {
        // Is the button already pressed ?
        if (continueToTrainingIsAlreadyPressed)
            return;

        continueToTrainingIsAlreadyPressed = true;

        Observer.Singleton.OnWarmingUpScreenStart();
    }

    private void ShowResults()
    {
        switch (DataManager.Singleton.complexion)
        {
            case Complexion.Thin:
                complexionText.text = "DELGADO EXT.";
                break;

            case Complexion.AcceptableThin:
                complexionText.text = "DELGADO";
                break;

            case Complexion.Normal:
                complexionText.text = "NORMAL";
                break;

            case Complexion.Overweight:
                complexionText.text = "SOBREPESO";
                break;

            case Complexion.NormalObesity:
                complexionText.text = "OBESIDAD";
                break;

            case Complexion.MorbidObesity:
                complexionText.text = "OBESIDAD EXT.";
                break;

            default:
                break;
        }

        switch (DataManager.Singleton.trainingLevel)
        {
            case TrainingLevel.Begginer:
                trainingText.text = "NOVATO";
                break;

            case TrainingLevel.Rookie:
                trainingText.text = "BÁSICO";
                break;

            case TrainingLevel.Medium:
                trainingText.text = "INTERMEDIO";
                break;

            case TrainingLevel.Advance:
                trainingText.text = "AVANZADO";
                break;

            default:
                break;
        }
    }

    // *** TRAINING ***

    public void TrainingReady()
    {
        Training.Singleton.Ready();
    }

    public void TrainingContinue()
    {
        Training.Singleton.Continue();
    }

    // *** REST FUNCTIONS ***

    public void OnCommonExerciseRest()
    {
        EnableTrainingScreen();
    }

    public void OnLastExerciseRest()
    {
        DisableAllScreens();
    }

    // *** ENABLE SCREEN FUNCTIONS ***

    private void EnableScreen(int index)
    {
        // Loop through every single screen
        for (int i = 0; i < screens.Length; i++)
        {
            if (i == index)
                screens[i].SetActive(true);
            else
            {
                // Is other screen active in the scene ?
                if (screens[i].activeInHierarchy)
                    screens[i].SetActive(false);
            }
        }
    }

    private void EnableIntroductionScreen()
    {
        // Reset the text index
        textIndex = 0;

        EnableScreen(0);
    }

    private void EnableDataScreen()
    {
        EnableScreen(1);
    }

    private void EnableExerciseDataScreen()
    {
        question = 0;

        EnableScreen(2);
    }

    private void EnableCompleteTestScreen()
    {
        EnableScreen(3);
    }

    private void EnableTrainingScreen()
    {
        EnableScreen(4);
    }

    private void EnableRestScreen()
    {
        EnableScreen(5);
    }

    private void DisableAllScreens()
    {
        // Loop through every single screen
        for (int i = 0; i < screens.Length; i++)
            screens[i].SetActive(false);
    }

    // *** ENABLE TRAINING SCREEN FUNCTIONS ***

    private void EnablePractice(int index)
    {
        for (int i = 0; i < trainingScreens.Length; i++)
        {
            if (i == index)
                trainingScreens[i].SetActive(true);
            else
            {
                if (trainingScreens[i].activeInHierarchy)
                    trainingScreens[i].SetActive(false);
            }
        }
    }

    private void EnableWarmingUp()
    {
        if (enableConsoleLog)
            Debug.Log("UIManager :: EnableWarmingUp");

        // Is the training screen active in hierarchy ?
        if (!screens[4].activeInHierarchy)
            EnableTrainingScreen();

        EnablePractice(0);
    }

    private void EnableTraining()
    {
        if (enableConsoleLog)
            Debug.Log("UIManager :: EnableTraining");

        // Is the training screen active in hierarchy ?
        if (!screens[4].activeInHierarchy)
            EnableTrainingScreen();

        EnablePractice(1);
    }

    private void EnableStretching()
    {
        if (enableConsoleLog)
            Debug.Log("UIManager :: EnableStretching");

        // Is the training screen active in hierarchy ?
        if (!screens[4].activeInHierarchy)
            EnableTrainingScreen();

        EnablePractice(2);
    }

    private void DisableAllPracticeScreens()
    {
        for (int i = 0; i < trainingScreens.Length; i++)
            trainingScreens[i].SetActive(false);
    }

    // *** BUTTONS CAN BE PRESSED AGAIN FUNCTIONS ***

    private void ContinueCanBePressedAgain()
    {
        continueIsAlreadyPressed = false;
    }

    private void NextQuestionCanBePressedAgain()
    {
        nextQuestionIsAlreadyPressed = false;
    }

    private void ContinueToTrainingCanBePressedAgain()
    {
        continueToTrainingIsAlreadyPressed = false;
    }

    #endregion

    #region Coroutines

    private IEnumerator WaitForTraining()
    {
        DisableAllScreens();

        yield return null; yield return null; yield return null;

        EnableIntroductionScreen();
    }

    #endregion
}
