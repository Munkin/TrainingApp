// <copyright file="Data.cs" company="Up Up Down Studios">
// Copyright (c) 2018 All Rights Reserved
// </copyright>
// <summary>User data container.</summary>

using UnityEngine;

[CreateAssetMenu(fileName = "New Data", menuName = "Assets/Data", order = 1)]
public class Data : ScriptableObject {

    #region Properties

    public bool enableTest = true;

    [Space(10f)]

    public string userName;
    public int age;
    public float height;
    public float weight;
    public float imc;
    public int score;

    [Space(10f)]

    public Complexion complexion;
    
    public TrainingLevel trainingLevel;
    public TrainingDay trainingDay;

    #endregion

    #region Class functions

    public void Save(string userName, int age, float height, float weight, float imc, int score, Complexion complexion, TrainingLevel trainingLevel, TrainingDay trainingDay)
    {
        // Can the user do the test ?
        if (enableTest)
            enableTest = false;

        this.userName = userName;
        this.age = age;
        this.height = height;
        this.weight = weight;
        this.imc = imc;
        this.score = score;

        this.complexion = complexion;
        this.trainingLevel = trainingLevel;
        this.trainingDay = trainingDay;
    }

    public Data Load()
    {
        return this;
    }

    // ***

    public void SaveName(string userName)
    {
        this.userName = userName;
    }

    public void SaveAge(int age)
    {
        this.age = age;
    }

    public void SaveHeight(float height)
    {
        this.height = height;
    }

    public void SaveWeight(float weight)
    {
        this.weight = weight;
    }

    public void SaveIMC(float imc)
    {
        this.imc = imc;
    }

    public void SaveScore(int score)
    {
        this.score = score;
    }

    public void SaveComplexion(Complexion complexion)
    {
        this.complexion = complexion;
    }

    public void SaveTrainingLevel(TrainingLevel trainingLevel)
    {
        this.trainingLevel = trainingLevel;
    }

    public void SaveTrainingDay(TrainingDay trainingDay)
    {
        this.trainingDay = trainingDay;
    }

    #endregion
}
