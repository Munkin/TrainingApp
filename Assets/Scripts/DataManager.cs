// <copyright file="DataManager.cs" company="Up Up Down Studios">
// Copyright (c) 2018 All Rights Reserved
// </copyright>
// <summary>User data handler class.</summary>

using UnityEngine;
using UnityEngine.UI;

public enum Complexion
{
    Thin, AcceptableThin, Normal, Overweight, NormalObesity, MorbidObesity
}

public enum TrainingLevel
{
    Begginer, Rookie, Medium, Advance
}

public class DataManager : MonoBehaviour {

    #region Properties

    [SerializeField]
    private bool enableConsoleLog;

    [Space(10f)]

    [SerializeField]
    private Data data;

    [Space(10f)]

    [SerializeField]
    private InputField nameInputField;
    [SerializeField]
    private Text nameText;

    [Space(10f)]

    [SerializeField]
    private InputField ageInputField;
    [SerializeField]
    private Text ageText;

    [Space(10f)]

    [SerializeField]
    private InputField heightInputField;
    [SerializeField]
    private Text heightText;

    [Space(10f)]

    [SerializeField]
    private InputField weightInputField;
    [SerializeField]
    private Text weightText;

    // Hidden
    private string userName; private bool inputFieldHasCorrectUserName;
    private int age; private bool inputFieldHasCorrectAge;
    private float height; private bool inputFieldHasCorrectHeight;
    private float weight; private bool inputFieldHasCorrectWeight;

    public float imc
    {
        get; private set;
    }
    public int score
    {
        get; private set;
    }

    // Const
    public const int minAge = 10;
    public const int maxAge = 75;
    public const float minHeight = 100f;
    public const float maxHeight = 215f;
    public const float minWeight = 20.0f;
    public const float maxWeight = 200.0f;

    // Enums
    public Complexion complexion
    {
        get; private set;
    }
    public TrainingLevel trainingLevel
    {
        get; private set;
    }

    // Singelton!
    public static DataManager Singleton
    {
        get; private set;
    }

    #endregion

    #region Unity fucntions

    private void Start()
    {
        Setup();
    }

    private void Awake()
    {
        if (Singleton != null)
            DestroyImmediate(gameObject);
        else
            Singleton = this;

        Suscribe();
    }

    #endregion

    #region Class functions

    private void Suscribe()
    {
        Observer.Singleton.onExerciseDataScreen += EstimateIMC;
        // On test result events
        Observer.Singleton.onTestResult += EstimateTotalScore;
        Observer.Singleton.onTestResult += SetTraining;
        Observer.Singleton.onTestResult += SaveData;

        // Data event
        if (!data.isFirstTime)
            LoadData();
    }

    private void Setup()
    {
        if (nameInputField == null || ageInputField == null || weightInputField == null || heightInputField == null)
            return;

        nameInputField.characterLimit = 22;
        nameInputField.onValidateInput += delegate (string input, int charIndex, char addedChar)
        {
            return ValidateLetterChar(addedChar);
        };

        ageInputField.characterLimit = 2;
        ageInputField.onValidateInput += delegate (string input, int charIndex, char addedChar)
        {
            return ValidateNumberChar(addedChar);
        };

        weightInputField.characterLimit = 3;
        weightInputField.onValidateInput += delegate (string input, int charIndex, char addedChar)
        {
            return ValidateNumberChar(addedChar);
        };

        heightInputField.characterLimit = 3;
        heightInputField.onValidateInput += delegate (string input, int charIndex, char addedChar)
        {
            return ValidateNumberChar(addedChar);
        };
    }

    private void SaveData()
    {
        if (enableConsoleLog)
            Debug.Log("DataManager :: Save");

        data.Save(userName, age, height, weight, imc, score, complexion, trainingLevel);
    }

    private void LoadData()
    {
        if (enableConsoleLog)
            Debug.Log("DataManager :: Load");

        userName = data.userName;
        age = data.age;
        height = data.height;
        weight = data.weight;
        imc = data.imc;
        score = data.score;

        complexion = data.complexion;
        trainingLevel = data.training;
    }

    public void EstimateIMC()
    {
        imc = weight / (height * height);

        if (imc <= 16.99f)
            complexion = Complexion.Thin;
        else if (imc >= 17.00f && imc <= 18.49f)
            complexion = Complexion.AcceptableThin;
        else if (imc >= 18.50f && imc <= 24.99f)
            complexion = Complexion.Normal;
        else if (imc >= 24.50f && imc <= 29.99f)
            complexion = Complexion.Overweight;
        else if (imc >= 30.00f && imc <= 34.99f)
            complexion = Complexion.NormalObesity;
        else
            complexion = Complexion.MorbidObesity;

        switch (complexion)
        {
            case Complexion.Thin:
                SubstractScore(4);
                break;

            case Complexion.AcceptableThin:
                SubstractScore(2);
                break;

            case Complexion.Overweight:
                SubstractScore(2);
                break;

            case Complexion.NormalObesity:
                SubstractScore(4);
                break;

            case Complexion.MorbidObesity:
                SubstractScore(5);
                break;

            default:
                break;
        }

        if (enableConsoleLog)
            Debug.Log(string.Format("DataManager :: EstimateIMC :: {0}", imc.ToString()));
    }

    public void EstimateTotalScore()
    {
        // Age score estimation.
        if (age <= 20)
            AddScore(1);
        else if (age >= 31 && age <= 40)
            SubstractScore(1);
        else if (age >= 41)
            SubstractScore(2);

        if (enableConsoleLog)
            Debug.Log(string.Format("DataManager :: EstimateTotalScore :: {0}", score.ToString()));
    }

    public void AddScore(int amount)
    {
        if (amount <= 0)
            return;

        score += amount;
    }

    public void SubstractScore(int amount)
    {
        if (amount <= 0)
            return;

        score -= amount;
    }

    public void OnNameEndEdit(string value)
    {
        if (enableConsoleLog)
            Debug.Log("DataManager :: OnNameEndEdit");

        if (value == null)
            return;

        userName = CheckName(value);

        // Is the name input field empty ?
        if (!string.IsNullOrEmpty(userName))
            inputFieldHasCorrectUserName = true;
        else
            inputFieldHasCorrectUserName = false;

        CheckData();

        nameText.text = userName;
    } // TODO Mobile InputField fix.

    public void OnAgeEndEdit(string value)
    {
        if (enableConsoleLog)
            Debug.Log("DataManager :: OnAgeEndEdit");

        if (value == null)
            return;
        else
        {
            try
            {
                age = Mathf.Clamp(int.Parse(value), minAge, maxAge);
                ageText.text = age.ToString();
            }
            catch (System.Exception)
            {
                if (enableConsoleLog)
                    Debug.Log("DataManager :: OnAgeEndEdit :: EmptyValue");
            }
        }

        // Is the age in thecorrect range ?
        if (age >= minAge && age <= maxAge)
            inputFieldHasCorrectAge = true;
        else
            inputFieldHasCorrectAge = false;

        CheckData();
    } // TODO Mobile InputField fix.

    public void OnHeightEndEdit(string value)
    {
        if (enableConsoleLog)
            Debug.Log("DataManager :: OnHeightEndEdit");

        if (value == null)
            return;
        else
        {
            try
            {
                height = Mathf.Clamp(float.Parse(value), minHeight, maxHeight) * 0.01f;
                heightText.text = string.Format("{0}.0 cm", (height * 100.0f).ToString());
            }
            catch (System.Exception)
            {
                if (enableConsoleLog)
                    Debug.Log("DataManager :: OnHeightEndEdit :: EmptyValue");
            }
        }

        // Is the height input field non a number ?
        if (float.IsNaN(height))
            inputFieldHasCorrectHeight = false;
        else
        {
            if (height > 0)
                inputFieldHasCorrectHeight = true;
            else
                inputFieldHasCorrectHeight = false;
        }

        CheckData();
    } // TODO Mobile InputField fix.

    public void OnWeightEndEdit(string value)
    {
        if (enableConsoleLog)
            Debug.Log("DataManager :: OnWeightEndEdit");

        if (value == null)
            return;
        else
        {
            try
            {
                weight = Mathf.Clamp(float.Parse(value), minWeight, maxWeight);
                weightText.text = string.Format("{0}.0 kg", weight.ToString());
            }
            catch (System.Exception)
            {
                if (enableConsoleLog)
                    Debug.Log("DataManager :: OnWeightEndEdit :: EmptyValue");
            }
        }

        // Is the height input field non a number ?
        if (float.IsNaN(weight))
            inputFieldHasCorrectWeight = false;
        else
        {
            if (weight > 0)
                inputFieldHasCorrectWeight = true;
            else
                inputFieldHasCorrectWeight = false;
        }

        CheckData();
    } // TODO Mobile InputField fix.

    private void SetTraining()
    {
        // Training estimation.
        if (score <= 14)
            trainingLevel = TrainingLevel.Begginer;
        else if (score >= 15 && score <= 24)
            trainingLevel = TrainingLevel.Rookie;
        else if (score >= 25 && score <= 34)
            trainingLevel = TrainingLevel.Medium;
        else
            trainingLevel = TrainingLevel.Advance;

        if (enableConsoleLog)
            Debug.Log(string.Format("DataManager :: SetTraining :: {0}", trainingLevel.ToString()));
    }

    private string CheckName(string value)
    {
        string[] words = value.Split(' ');

        string name = null;

        for (int i = 0; i < words.Length; i++)
        {
            if (words[i].Length <= 0)
                break;

            for (int j = 0; j < words[i].Length; j++)
            {
                if (j == 0)
                {
                    if (!char.IsUpper(words[i][j]))
                        name += char.ToUpper(words[i][j]);
                    else
                        name += words[i][j];
                }
                else
                {
                    if (!char.IsWhiteSpace(words[i][j]))
                    {
                        if (!char.IsLower(words[i][j]))
                            name += char.ToLower(words[i][j]);
                        else
                            name += words[i][j];
                    }
                }
            }

            if (i != words.Length - 1)
                name += " ";
        }

        return name;
    } // Name validation

    private void CheckData()
    {
        // Are all the input fields with the correct data ?
        if (inputFieldHasCorrectUserName && inputFieldHasCorrectAge && inputFieldHasCorrectWeight && inputFieldHasCorrectHeight)
            UIManager.Singleton.EnableContinueButton();
        else
            UIManager.Singleton.DisableContinueButton();
    }

    private char ValidateLetterChar(char charToValidate)
    {
        if (!char.IsLetter(charToValidate) && !char.IsWhiteSpace(charToValidate))
            charToValidate = ' ';

        return charToValidate;
    }

    private char ValidateNumberChar(char charToValidate)
    {
        if (!char.IsNumber(charToValidate))
            charToValidate = '0';

        return charToValidate;
    }

    #endregion
}
