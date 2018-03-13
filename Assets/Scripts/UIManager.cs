// <copyright file="UIManager.cs" company="Up Up Down Studios">
// Copyright (c) 2018 All Rights Reserved
// </copyright>
// <summary>Manager for Unity user interface events.</summary>

using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class UIManager : MonoBehaviour {

    #region Properties

    [Header("Screens")]

    [SerializeField]
    private GameObject[] screens;

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

    [SerializeField]
    private GameObject[] trainingScreens;
    [SerializeField]
    private string[] warmUpTexts;
    [SerializeField]
    private string[] trainingTexts;
    [SerializeField]
    private string[] stretchingTexts;

    [Space(10f)]

    [SerializeField]
    private bool enableConsoleLog;

    // Hidden
    private int textIndex = 0;
    private int question = 0;

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

    private void Suscribe()
    {
        // *** GENERAL EVENTS ***

        Observer.Singleton.onIntroductionScreen += FadeIn;
        Observer.Singleton.onDataScreen += EnableDataScreen;
        Observer.Singleton.onExerciseDataScreen += EnableExerciseDataScreen;
        // OnTestResult Events
        Observer.Singleton.onTestResult += EnableIntroductionScreen;
        Observer.Singleton.onTestResult += FadeInResult;
        // OnTestEnd Events
        Observer.Singleton.onTestEnd += EnableCompleteTestScreen;
        Observer.Singleton.onTestEnd += ShowResults;

        // *** TRAINING EVENTS ***

        // OnTrainingStart Events
        Observer.Singleton.onWarmingUpScreenStart += EnableIntroductionScreen;
        Observer.Singleton.onWarmingUpScreenStart += FadeInToWarmingUp;
        Observer.Singleton.onWarmingUpScreenEnd += EnableWarmingUp;
        // OnWarmingUpScreen Events
        Observer.Singleton.onTrainingScreenStart += EnableIntroductionScreen;
        Observer.Singleton.onTrainingScreenStart += FadeInToTraining;
        Observer.Singleton.onTrainingScreenEnd += EnableTraining;
        // OnTrainingScreen Events
        Observer.Singleton.onStretchingScreenStart += EnableIntroductionScreen;
        Observer.Singleton.onStretchingScreenStart += FadeInToStretching;
        Observer.Singleton.onStretchingScreenEnd += EnableStretching;
    }

    private void Setup()
    {
        interludeText.text = interludeTexts[textIndex];

        for (int i = 0; i < screens.Length; i++)
        {
            if (i != 0)
                screens[i].SetActive(false);
            else
                screens[i].SetActive(true);
        }
    }

    // *** INTERLUDE FUNCTIONS ***

    private void FadeOut()
    {
        if (enableConsoleLog)
            Debug.Log("UIManager :: FadeOut");

        interludeText.DOFade(0.0f, timeToFadeIn).OnComplete(SetIntroductionText);
    }

    private void FadeInterlude()
    {
        interludeText.DOFade(interludeText.color.a, timeToFadeOut).OnComplete(FadeOut);
    }

    private void FadeIn()
    {
        if (enableConsoleLog)
            Debug.Log("UIManager :: FadeIn");

        interludeText.DOFade(0.8745f, timeToFadeOut).OnComplete(FadeInterlude);
    }

    private void SetIntroductionText()
    {
        if (textIndex < interludeTexts.Length - 1)
            textIndex++;
        else
            Observer.Singleton.OnDataScreen();

        if (screens[0].activeInHierarchy)
        {
            interludeText.text = interludeTexts[textIndex];

            FadeIn();
        }
    }

    // ***

    private void FadeOutResult()
    {
        if (enableConsoleLog)
            Debug.Log("UIManager :: FadeOutResult");

        interludeText.DOFade(0.0f, timeToFadeIn).OnComplete(SetResultText);
    }

    private void FadeInterludeResult()
    {
        interludeText.DOFade(interludeText.color.a, timeToFadeOut).OnComplete(FadeOutResult);
    }

    private void FadeInResult()
    {
        if (enableConsoleLog)
            Debug.Log("UIManager :: FadeInResult");

        if (interludeText.text == interludeTexts[interludeTexts.Length - 1])
            interludeText.text = completeTestTexts[0];

        interludeText.DOFade(0.8745f, timeToFadeOut).OnComplete(FadeInterludeResult);
    }

    private void SetResultText()
    {
        if (textIndex < completeTestTexts.Length - 1)
            textIndex++;
        else
            Observer.Singleton.OnTestEnd();

        if (screens[0].activeInHierarchy)
        {
            interludeText.text = completeTestTexts[textIndex];

            FadeInResult();
        }
    }
    
    // ***

    private void FadeOutToWarmingUp()
    {
        if (enableConsoleLog)
            Debug.Log("UIManager :: FadeOutToWarmingUp");

        interludeText.DOFade(0.0f, timeToFadeIn).OnComplete(SetToWarmingUp);
    }

    private void FadeInterludeToWarmingUp()
    {
        interludeText.DOFade(interludeText.color.a, timeToFadeOut).OnComplete(FadeOutToWarmingUp);
    }

    private void FadeInToWarmingUp()
    {
        if (enableConsoleLog)
            Debug.Log("UIManager :: FadeInToWarmingUp");

        if (interludeText.text == completeTestTexts[completeTestTexts.Length - 1])
            interludeText.text = warmUpTexts[0];

        interludeText.DOFade(0.8745f, timeToFadeOut).OnComplete(FadeInterludeToWarmingUp);
    }

    private void SetToWarmingUp()
    {
        if (textIndex < warmUpTexts.Length - 1)
            textIndex++;
        else
            Observer.Singleton.OnWarmingUpScreenEnd();

        if (screens[0].activeInHierarchy)
        {
            interludeText.text = warmUpTexts[textIndex];

            FadeInToWarmingUp();
        }
    }

    // ***

    private void FadeOutToTraining()
    {
        if (enableConsoleLog)
            Debug.Log("UIManager :: FadeOutToTraining");

        interludeText.DOFade(0.0f, timeToFadeIn).OnComplete(SetToTraining);
    }

    private void FadeInterludeToTraining()
    {
        interludeText.DOFade(interludeText.color.a, timeToFadeOut).OnComplete(FadeOutToTraining);
    }

    private void FadeInToTraining()
    {
        if (enableConsoleLog)
            Debug.Log("UIManager :: FadeInToTraining");

        if (interludeText.text == warmUpTexts[warmUpTexts.Length - 1])
            interludeText.text = trainingTexts[0];

        interludeText.DOFade(0.8745f, timeToFadeOut).OnComplete(FadeInterludeToTraining);
    }

    private void SetToTraining()
    {
        if (textIndex < trainingTexts.Length - 1)
            textIndex++;
        else
            Observer.Singleton.OnTrainingScreenEnd();

        if (screens[0].activeInHierarchy)
        {
            interludeText.text = trainingTexts[textIndex];

            FadeInToTraining();
        }
    }

    // ***

    private void FadeOutToStretching()
    {
        if (enableConsoleLog)
            Debug.Log("UIManager :: FadeOutToStretching");

        interludeText.DOFade(0.0f, timeToFadeIn).OnComplete(SetToStretching);
    }

    private void FadeInterludeToStretching()
    {
        interludeText.DOFade(interludeText.color.a, timeToFadeOut).OnComplete(FadeOutToStretching);
    }

    private void FadeInToStretching()
    {
        if (enableConsoleLog)
            Debug.Log("UIManager :: FadeInToStretching");

        if (interludeText.text == trainingTexts[trainingTexts.Length - 1])
            interludeText.text = stretchingTexts[0];

        interludeText.DOFade(0.8745f, timeToFadeOut).OnComplete(FadeInterludeToStretching);
    }

    private void SetToStretching()
    {
        if (textIndex < stretchingTexts.Length - 1)
            textIndex++;
        else
            Observer.Singleton.OnStretchingScreenEnd();

        if (screens[0].activeInHierarchy)
        {
            interludeText.text = stretchingTexts[textIndex];

            FadeInToStretching();
        }
    }

    // *** DATA SCREEN ***

    public void Continue()
    {
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
        CheckSelectedOption();

        if (question >= 4)
            Observer.Singleton.OnTestResult();
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

            case Complexion.ObeseType1:
                complexionText.text = "OBESIDAD";
                break;

            case Complexion.ObeseType2:
                complexionText.text = "OBESIDAD EXT.";
                break;

            default:
                break;
        }

        switch (DataManager.Singleton.training)
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

    // *** WARMING UP ***

    public void TrainingReady()
    {
        Training.Singleton.Ready();
    }

    public void TrainingContinue()
    {
        Training.Singleton.Continue();
    }

    // *** ENABLE SCREEN FUNCTIONS ***

    private void EnableScreen(int index)
    {
        for (int i = 0; i < screens.Length; i++)
        {
            if (i == index)
                screens[i].SetActive(true);
            else
            {
                if (screens[i].activeInHierarchy)
                    screens[i].SetActive(false);
            }
        }
    }

    private void EnableIntroductionScreen()
    {
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

    // *** ENABLE TRAINING SCREEN FUNCTIONS ***

    private void EnableTrainingScreen(int index)
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

        if (!screens[4].activeInHierarchy)
            EnableTrainingScreen();

        EnableTrainingScreen(0);
    }

    private void EnableTraining()
    {
        if (enableConsoleLog)
            Debug.Log("UIManager :: EnableTraining");

        if (!screens[4].activeInHierarchy)
            EnableTrainingScreen();

        EnableTrainingScreen(1);
    }

    private void EnableStretching()
    {
        if (enableConsoleLog)
            Debug.Log("UIManager :: EnableStretching");

        if (!screens[4].activeInHierarchy)
            EnableTrainingScreen();

        EnableTrainingScreen(2);
    }

    #endregion
}
