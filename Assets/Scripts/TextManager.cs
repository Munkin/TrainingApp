using UnityEngine;

public enum FadeState
{
    None, FadeIn, Interlude, FadeOut
}

public class TextManager : MonoBehaviour {

    #region Properties

    // Fade Control
    private bool isFirstTimeFading = true;

    // Cached Components
    public FadeState fadeState
    {
        get; private set;
    }

    #endregion

    #region Unity functions

    private void Start()
    {
        
    }

    #endregion

    #region Class functions

    private void Setup()
    {

    }

    private void Suscribe()
    {

    }

    private void ResetFadeValues()
    {
        fadeState = FadeState.None;

        isFirstTimeFading = true;
    } // Control function

    #endregion

    #region Fade functions

    #region Fade: Test Introduction

    private void FadeOutIntroduction()
    {
        if (enableConsoleLog)
            Debug.Log("UIManager :: FadeOutInterlude");

        fadeState = FadeState.FadeOut;

        interludeText.DOFade(0.0f, timeToFadeIn).OnComplete(SetIntroductionText);
    }

    private void FadeInterludeIntroduction()
    {
        fadeState = FadeState.Interlude;

        interludeText.DOFade(interludeText.color.a, timeToFadeOut).OnComplete(FadeOutInterlude);
    }

    private void FadeInIntroduction()
    {
        if (enableConsoleLog)
            Debug.Log("UIManager :: FadeInInterlude");

        fadeState = FadeState.FadeIn;

        // Is the first fade of the entire fade sequence ?
        if (isFirstTimeFading)
        {
            interludeText.text = interludeTexts[0];

            isFirstTimeFading = false;
        }

        interludeText.DOFade(0.8745f, timeToFadeOut).OnComplete(FadeInterlude);
    } // 1.

    private void SetIntroductionText()
    {
        if (textIndex < interludeTexts.Length - 1)
            textIndex++;
        else
            Observer.Singleton.OnDataScreen();

        // Is the interlude screen active in hierarchy ?
        if (screens[0].activeInHierarchy)
        {
            interludeText.text = interludeTexts[textIndex];

            FadeInIntroduction();
        }
    }

    private void ResetIntroductionFadeValues()
    {
        fadeState = FadeState.None;

        isFirstTimeFading = true;
    } // Control function

    #endregion

    #region Fade: Test Result

    private void FadeOutResult()
    {
        if (enableConsoleLog)
            Debug.Log("UIManager :: FadeOutResult");

        fadeState = FadeState.FadeOut;

        interludeText.DOFade(0.0f, timeToFadeIn).OnComplete(SetResultText);
    }

    private void FadeInterludeResult()
    {
        fadeState = FadeState.Interlude;

        interludeText.DOFade(interludeText.color.a, timeToFadeOut).OnComplete(FadeOutResult);
    }

    private void FadeInResult()
    {
        if (enableConsoleLog)
            Debug.Log("UIManager :: FadeInResult");

        fadeState = FadeState.FadeIn;

        // Is the first fade of the entire fade sequence ?
        if (isFirstTimeFading)
        {
            interludeText.text = completeTestTexts[0];

            fadeResultFirstTime = false;
        }

        interludeText.DOFade(0.8745f, timeToFadeOut).OnComplete(FadeInterludeResult);
    } // 1.

    private void SetResultText()
    {
        if (textIndex < completeTestTexts.Length - 1)
            textIndex++;
        else
            Observer.Singleton.OnTestEnd();

        // Is the interlude screen active in hierarchy ?
        if (screens[0].activeInHierarchy)
        {
            interludeText.text = completeTestTexts[textIndex];

            FadeInResult();
        }
    }

    #endregion

    #region Fade: Training Screen, WarmingUp

    private void FadeOutToWarmingUp()
    {
        if (enableConsoleLog)
            Debug.Log("UIManager :: FadeOutToWarmingUp");

        fadeState = FadeState.FadeOut;

        interludeText.DOFade(0.0f, timeToFadeIn).OnComplete(SetToWarmingUp);
    }

    private void FadeInterludeToWarmingUp()
    {
        fadeState = FadeState.Interlude;

        interludeText.DOFade(interludeText.color.a, timeToFadeOut).OnComplete(FadeOutToWarmingUp);
    }

    private void FadeInToWarmingUp()
    {
        if (enableConsoleLog)
            Debug.Log("UIManager :: FadeInToWarmingUp");

        fadeState = FadeState.FadeIn;

        // Is the first fade of the entire fade sequence ?
        if (fadeWarmingUpFirstTime)
        {
            interludeText.text = warmUpTexts[0];

            fadeWarmingUpFirstTime = false;
        }

        interludeText.DOFade(0.8745f, timeToFadeOut).OnComplete(FadeInterludeToWarmingUp);
    } // 1.

    private void SetToWarmingUp()
    {
        if (textIndex < warmUpTexts.Length - 1)
            textIndex++;
        else
            Observer.Singleton.OnWarmingUpScreenEnd();

        // Is the interlude screen active in hierarchy ?
        if (screens[0].activeInHierarchy)
        {
            interludeText.text = warmUpTexts[textIndex];

            FadeInToWarmingUp();
        }
    }

    #endregion

    #region Fade: Training Screen, Training

    private void FadeOutToTraining()
    {
        if (enableConsoleLog)
            Debug.Log("UIManager :: FadeOutToTraining");

        fadeState = FadeState.FadeOut;

        interludeText.DOFade(0.0f, timeToFadeIn).OnComplete(SetToTraining);
    }

    private void FadeInterludeToTraining()
    {
        fadeState = FadeState.Interlude;

        interludeText.DOFade(interludeText.color.a, timeToFadeOut).OnComplete(FadeOutToTraining);
    }

    private void FadeInToTraining()
    {
        if (enableConsoleLog)
            Debug.Log("UIManager :: FadeInToTraining");

        fadeState = FadeState.FadeIn;

        // Is the first fade of the entire fade sequence ?
        if (fadeTrainingFirstTime)
        {
            interludeText.text = trainingTexts[0];

            fadeTrainingFirstTime = false;
        }

        interludeText.DOFade(0.8745f, timeToFadeOut).OnComplete(FadeInterludeToTraining);
    } // 1.

    private void SetToTraining()
    {
        if (textIndex < trainingTexts.Length - 1)
            textIndex++;
        else
            Observer.Singleton.OnTrainingScreenEnd();

        // Is the interlude screen active in hierarchy ?
        if (screens[0].activeInHierarchy)
        {
            interludeText.text = trainingTexts[textIndex];

            FadeInToTraining();
        }
    }

    #endregion

    #region Fade: Training Screen, Stretching

    private void FadeOutToStretching()
    {
        if (enableConsoleLog)
            Debug.Log("UIManager :: FadeOutToStretching");

        fadeState = FadeState.FadeOut;

        interludeText.DOFade(0.0f, timeToFadeIn).OnComplete(SetToStretching);
    }

    private void FadeInterludeToStretching()
    {
        fadeState = FadeState.Interlude;

        interludeText.DOFade(interludeText.color.a, timeToFadeOut).OnComplete(FadeOutToStretching);
    }

    private void FadeInToStretching()
    {
        if (enableConsoleLog)
            Debug.Log("UIManager :: FadeInToStretching");

        fadeState = FadeState.FadeIn;

        // Is the first fade of the entire fade sequence ?
        if (fadeStretchingFirstTime)
        {
            interludeText.text = stretchingTexts[0];

            fadeStretchingFirstTime = false;
        }

        interludeText.DOFade(0.8745f, timeToFadeOut).OnComplete(FadeInterludeToStretching);
    } // 1.

    private void SetToStretching()
    {
        if (textIndex < stretchingTexts.Length - 1)
            textIndex++;
        else
            Observer.Singleton.OnStretchingScreenEnd();

        // Is the interlude screen active in hierarchy ?
        if (screens[0].activeInHierarchy)
        {
            interludeText.text = stretchingTexts[textIndex];

            FadeInToStretching();
        }
    }

    #endregion

    #region Fade: Rest

    private void FadeOutRest()
    {
        if (enableConsoleLog)
            Debug.Log("UIManager :: FadeOutRest");

        fadeState = FadeState.FadeOut;

        interludeText.DOFade(0.0f, timeToFadeIn).OnComplete(SetRestText);
    }

    private void FadeInterludeRest()
    {
        fadeState = FadeState.Interlude;

        interludeText.DOFade(interludeText.color.a, timeToFadeOut).OnComplete(FadeOutRest);
    }

    private void FadeInRest()
    {
        if (enableConsoleLog)
            Debug.Log("UIManager :: FadeInRest");

        fadeState = FadeState.FadeIn;

        // Is the first fade of the entire fade sequence ?
        if (fadeRestFirstTime)
        {
            interludeText.text = restTexts[0];

            fadeRestFirstTime = false;
        }

        interludeText.DOFade(0.8745f, timeToFadeOut).OnComplete(FadeInterludeRest);
    } // 1.

    private void SetRestText()
    {
        if (textIndex < restTexts.Length - 1)
            textIndex++;
        else
            textIndex = restTexts.Length - 1;

        // Is the interlude screen active in hierarchy ?
        if (screens[0].activeInHierarchy)
        {
            interludeText.text = restTexts[textIndex];

            FadeInRest();
        }
    }

    #endregion

    #region Fade: Training End

    private void FadeOutTrainingEnd()
    {
        if (enableConsoleLog)
            Debug.Log("UIManager :: FadeOutTrainingEnd");

        fadeState = FadeState.FadeOut;

        interludeText.DOFade(0.0f, timeToFadeIn).OnComplete(SetTrainingEndText);
    }

    private void FadeInterludeTrainingEnd()
    {
        fadeState = FadeState.Interlude;

        interludeText.DOFade(interludeText.color.a, timeToFadeOut).OnComplete(FadeOutTrainingEnd);
    }

    private void FadeInTrainingEnd()
    {
        if (enableConsoleLog)
            Debug.Log("UIManager :: FadeInTrainingEnd");

        fadeState = FadeState.FadeIn;

        // Is the first fade of the entire fade sequence ?
        if (fadeTrainingEnd)
        {
            interludeText.text = trainingEndTexts[0];

            fadeTrainingEnd = false;
        }

        interludeText.DOFade(0.8745f, timeToFadeOut).OnComplete(FadeInterludeTrainingEnd);
    } // 1.

    private void SetTrainingEndText()
    {
        if (textIndex < trainingEndTexts.Length - 2)
            textIndex++;
        else
        {
            textIndex++;

            interludeText.text = trainingEndTexts[textIndex];

            interludeText.DOFade(0.8745f, timeToFadeOut);

            Observer.Singleton.OnAppEnd();

            return; // End of the application
        }

        // Is the interlude screen active in hierarchy ?
        if (screens[0].activeInHierarchy)
        {
            interludeText.text = trainingEndTexts[textIndex];

            FadeInTrainingEnd();
        }
    }

    #endregion

    #region Fade: OnAlreadyOpened

    private void FadeOutAlreadyOpened()
    {
        if (enableConsoleLog)
            Debug.Log("UIManager :: FadeOutAlreadyOpened");

        fadeState = FadeState.FadeOut;

        interludeText.DOFade(0.0f, timeToFadeIn).OnComplete(SetAlreadyOpenedText);
    }

    private void FadeInterludeAlreadyOpened()
    {
        fadeState = FadeState.Interlude;

        interludeText.DOFade(interludeText.color.a, timeToFadeOut).OnComplete(FadeOutAlreadyOpened);
    }

    private void FadeInAlreadyOpened()
    {
        if (enableConsoleLog)
            Debug.Log("UIManager :: FadeInAlreadyOpened");

        fadeState = FadeState.FadeIn;

        // Is the first fade of the entire fade sequence ?
        if (fadeAlreadyOpened)
        {
            interludeText.text = alreadyOpenedTexts[0];

            fadeAlreadyOpened = false;
        }

        interludeText.DOFade(0.8745f, timeToFadeOut).OnComplete(FadeInterludeAlreadyOpened);
    } // 1.

    private void SetAlreadyOpenedText()
    {
        if (textIndex < alreadyOpenedTexts.Length - 2)
            textIndex++;
        else
        {
            textIndex++;

            interludeText.text = alreadyOpenedTexts[textIndex];

            interludeText.DOFade(0.8745f, timeToFadeOut);

            Observer.Singleton.OnAppWasAlreadyOpenedTodayEnd();

            return; // End of the application.
        }

        // Is the interlude screen active in hierarchy ?
        if (screens[0].activeInHierarchy)
        {
            interludeText.text = alreadyOpenedTexts[textIndex];

            FadeInAlreadyOpened();
        }
    }

    // First Text
    private void SetAlreadyOpenedFirstText()
    {
        alreadyOpenedTexts[0] = string.Format("Hola {0}", DataManager.Singleton.userName);
    }

    #endregion

    #region Fade: OnDailyTraining

    private void FadeOutDailyTraining()
    {
        if (enableConsoleLog)
            Debug.Log("UIManager :: FadeOutDailyTraining");

        fadeState = FadeState.FadeOut;

        interludeText.DOFade(0.0f, timeToFadeIn).OnComplete(SetDailyTrainingText);
    }

    private void FadeInterludeDailyTraining()
    {
        fadeState = FadeState.Interlude;

        interludeText.DOFade(interludeText.color.a, timeToFadeOut).OnComplete(FadeOutDailyTraining);
    }

    private void FadeInDailyTraining()
    {
        if (enableConsoleLog)
            Debug.Log("UIManager :: FadeInDailyTraining");

        fadeState = FadeState.FadeIn;

        // Is the first fade of the entire fade sequence ?
        if (fadeDailyTraining)
        {
            interludeText.text = dailyTrainingTexts[0];

            fadeDailyTraining = false;
        }

        interludeText.DOFade(0.8745f, timeToFadeOut).OnComplete(FadeInterludeDailyTraining);
    } // 1.

    private void SetDailyTrainingText()
    {
        if (textIndex < dailyTrainingTexts.Length - 1)
            textIndex++;
        else
        {
            Observer.Singleton.OnDailyTrainingEnd();
            Observer.Singleton.OnWarmingUpScreenStart();
            return;
        }

        // Is the interlude screen active in hierarchy ?
        if (screens[0].activeInHierarchy)
        {
            interludeText.text = dailyTrainingTexts[textIndex];

            FadeInDailyTraining();
        }
    }

    // First Text
    private void SetDailyTrainingFirstText()
    {
        dailyTrainingTexts[0] = string.Format("Hola {0}", DataManager.Singleton.userName);
    }

    #endregion

    #endregion
}
