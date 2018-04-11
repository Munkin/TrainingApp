using UnityEngine;

public class UpdateTraining : MonoBehaviour {

    #region Properties

    private TrainingLevel trainingLevel;
    private TrainingDay trainingDay;

    #endregion

    #region Unity function

    private void Awake()
    {
        Suscribe();
    }

    private void Start()
    {
        Setup();
    }

    #endregion

    #region Class function

    private void Setup()
    {
        LoadTrainingData();
    }

    private void Suscribe()
    {
        Observer.Singleton.onDailyTraining += SetNewDayTraining;
    }

    // ***

    public void SetNewDayTraining()
    {
        LoadTrainingData();

        switch (trainingDay)
        {
            case TrainingDay.One:
                DayTwoWeight();
                break;

            case TrainingDay.Two:
                DayThreeWeight();
                break;

            case TrainingDay.Three:
                DayFourWeight();
                break;

            case TrainingDay.Four:
                DayFiveWeight();
                break;

            case TrainingDay.Five:
                DaySixWeight();
                break;

            case TrainingDay.Six:
                DaySevenWeight();
                break;

            case TrainingDay.Seven:
                DoTestAgain();
                break;

            default:
                break;
        }

        Observer.Singleton.OnTrainingLoadCallback();
    }

    private void DoTestAgain()
    {
        DataManager.Singleton.GetData().canDoTest = true;

        // Reset the training.
        Observer.Singleton.OnIntroductionScreen();
    }

    private void DayTwoWeight()
    {
        // Day two weight.
        switch (trainingLevel)
        {
            case TrainingLevel.Begginer:
                Training.Singleton.NotifyNewTrainingData(Resources.Load("TrainingData/Begginer/Begginer_Day2") as TrainingData);
                break;

            case TrainingLevel.Rookie:
                Training.Singleton.NotifyNewTrainingData(Resources.Load("TrainingData/Rookie/Rookie_Day2") as TrainingData);
                break;

            case TrainingLevel.Medium:
                Training.Singleton.NotifyNewTrainingData(Resources.Load("TrainingData/Medium/Medium_Day2") as TrainingData);
                break;

            case TrainingLevel.Advance:
                Training.Singleton.NotifyNewTrainingData(Resources.Load("TrainingData/Advance/Advance_Day2") as TrainingData);
                break;

            default:
                break;
        }

        SaveTrainingData(trainingLevel, TrainingDay.Two);
    }

    private void DayThreeWeight()
    {
        // Day three weight.
        switch (trainingLevel)
        {
            case TrainingLevel.Begginer:
                Training.Singleton.NotifyNewTrainingData(Resources.Load("TrainingData/Begginer/Begginer_Day3") as TrainingData);
                break;

            case TrainingLevel.Rookie:
                Training.Singleton.NotifyNewTrainingData(Resources.Load("TrainingData/Rookie/Rookie_Day3") as TrainingData);
                break;

            case TrainingLevel.Medium:
                Training.Singleton.NotifyNewTrainingData(Resources.Load("TrainingData/Medium/Medium_Day3") as TrainingData);
                break;

            case TrainingLevel.Advance:
                Training.Singleton.NotifyNewTrainingData(Resources.Load("TrainingData/Advance/Advance_Day3") as TrainingData);
                break;

            default:
                break;
        }

        SaveTrainingData(trainingLevel, TrainingDay.Three);
    }

    private void DayFourWeight()
    {
        // Day four weight.
        switch (trainingLevel)
        {
            case TrainingLevel.Begginer:
                Training.Singleton.NotifyNewTrainingData(Resources.Load("TrainingData/Begginer/Begginer_Day4") as TrainingData);
                break;

            case TrainingLevel.Rookie:
                Training.Singleton.NotifyNewTrainingData(Resources.Load("TrainingData/Rookie/Rookie_Day4") as TrainingData);
                break;

            case TrainingLevel.Medium:
                Training.Singleton.NotifyNewTrainingData(Resources.Load("TrainingData/Medium/Medium_Day4") as TrainingData);
                break;

            case TrainingLevel.Advance:
                Training.Singleton.NotifyNewTrainingData(Resources.Load("TrainingData/Advance/Advance_Day4") as TrainingData);
                break;

            default:
                break;
        }

        SaveTrainingData(trainingLevel, TrainingDay.Four);
    }

    private void DayFiveWeight()
    {
        // Day five weight.
        switch (trainingLevel)
        {
            case TrainingLevel.Begginer:
                Training.Singleton.NotifyNewTrainingData(Resources.Load("TrainingData/Begginer/Begginer_Day5") as TrainingData);
                break;

            case TrainingLevel.Rookie:
                Training.Singleton.NotifyNewTrainingData(Resources.Load("TrainingData/Rookie/Rookie_Day5") as TrainingData);
                break;

            case TrainingLevel.Medium:
                Training.Singleton.NotifyNewTrainingData(Resources.Load("TrainingData/Medium/Medium_Day5") as TrainingData);
                break;

            case TrainingLevel.Advance:
                Training.Singleton.NotifyNewTrainingData(Resources.Load("TrainingData/Advance/Advance_Day5") as TrainingData);
                break;

            default:
                break;
        }

        SaveTrainingData(trainingLevel, TrainingDay.Five);
    }

    private void DaySixWeight()
    {
        // Day six weight.
        switch (trainingLevel)
        {
            case TrainingLevel.Begginer:
                Training.Singleton.NotifyNewTrainingData(Resources.Load("TrainingData/Begginer/Begginer_Day6") as TrainingData);
                break;

            case TrainingLevel.Rookie:
                Training.Singleton.NotifyNewTrainingData(Resources.Load("TrainingData/Rookie/Rookie_Day6") as TrainingData);
                break;

            case TrainingLevel.Medium:
                Training.Singleton.NotifyNewTrainingData(Resources.Load("TrainingData/Medium/Medium_Day6") as TrainingData);
                break;

            case TrainingLevel.Advance:
                Training.Singleton.NotifyNewTrainingData(Resources.Load("TrainingData/Advance/Advance_Day6") as TrainingData);
                break;

            default:
                break;
        }

        SaveTrainingData(trainingLevel, TrainingDay.Six);
    }

    private void DaySevenWeight()
    {
        // Day seven weight.
        switch (trainingLevel)
        {
            case TrainingLevel.Begginer:
                Training.Singleton.NotifyNewTrainingData(Resources.Load("TrainingData/Begginer/Begginer_Day7") as TrainingData);
                break;

            case TrainingLevel.Rookie:
                Training.Singleton.NotifyNewTrainingData(Resources.Load("TrainingData/Rookie/Rookie_Day7") as TrainingData);
                break;

            case TrainingLevel.Medium:
                Training.Singleton.NotifyNewTrainingData(Resources.Load("TrainingData/Medium/Medium_Day7") as TrainingData);
                break;

            case TrainingLevel.Advance:
                Training.Singleton.NotifyNewTrainingData(Resources.Load("TrainingData/Advance/Advance_Day7") as TrainingData);
                break;

            default:
                break;
        }

        SaveTrainingData(trainingLevel, TrainingDay.Seven);
    }

    // ***

    private void SaveTrainingData(TrainingLevel trainingLevel, TrainingDay trainingDay)
    {
        this.trainingLevel = trainingLevel;
        this.trainingDay = trainingDay;

        DataManager.Singleton.NotifySavedData(trainingLevel, trainingDay);
    }

    private void LoadTrainingData()
    {
        trainingLevel = DataManager.Singleton.GetData().trainingLevel;
        trainingDay = DataManager.Singleton.GetData().trainingDay;
    }

    #endregion
}
