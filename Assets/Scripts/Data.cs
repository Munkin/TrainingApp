// <copyright file="Data.cs" company="Up Up Down Studios">
// Copyright (c) 2018 All Rights Reserved
// </copyright>
// <summary>User data container.</summary>

using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "Assets/Data", order = 1)]
public class Data : ScriptableObject {

    #region Properties

    public bool isFirstTime = true;

    [Space(10f)]

    public string userName;
    public int age;
    public float height;
    public float weight;
    public float imc;
    public int score;

    [Space(10f)]

    public Complexion complexion;
    public TrainingLevel training;

    #endregion

    #region Class functions

    public void Save(string userName, int age, float height, float weight, float imc, int score, Complexion complexion, TrainingLevel training)
    {
        /*
        if (isFirstTime)
            isFirstTime = false;

        this.userName = userName;
        this.age = age;
        this.height = height;
        this.weight = weight;
        this.imc = imc;
        this.score = score;

        this.complexion = complexion;
        this.training = training;

        Observer.Singleton.OnSave();
        */
    }

    public Data Load()
    {
        Observer.Singleton.OnLoad();

        return this;
    }

    // *** SAVE FUNCTIONS ***

    public void SaveName(string userName)
    {
        this.userName = userName;

        Observer.Singleton.OnSave();
    }

    public void SaveAge(int age)
    {
        this.age = age;

        Observer.Singleton.OnSave();
    }

    public void SaveHeight(float height)
    {
        this.height = height;

        Observer.Singleton.OnSave();
    }

    public void SaveWeight(float weight)
    {
        this.weight = weight;

        Observer.Singleton.OnSave();
    }

    public void SaveIMC(float imc)
    {
        this.imc = imc;

        Observer.Singleton.OnSave();
    }

    public void SaveScore(int score)
    {
        this.score = score;

        Observer.Singleton.OnSave();
    }

    public void SaveComplexion(Complexion complexion)
    {
        this.complexion = complexion;

        Observer.Singleton.OnSave();
    }

    public void SaveTraining(TrainingLevel training)
    {
        this.training = training;

        Observer.Singleton.OnSave();
    }

    #endregion
}
