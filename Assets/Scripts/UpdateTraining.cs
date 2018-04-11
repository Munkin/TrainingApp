using UnityEngine;

public class UpdateTraining : MonoBehaviour {

    #region Properties

    private int level;
    private TrainingLevel trainingLevel;
    private int day;
    private TrainingDay trainingDay;

    #endregion

    #region Unity function

    private void Awake()
    {
        Setup();
        Suscribe();
    }

    #endregion

    #region Class function

    private void Setup()
    {
        level = PlayerPrefs.GetInt("Level", 0);
        day = PlayerPrefs.GetInt("Day", 0);
    }

    private void Suscribe()
    {
        Observer.Singleton.onDailyTraining += SetNewDayTraining;
    }

    // ***

    public void SetNewDayTraining()
    {
        ParseSavedTrainingData();

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
    }

    // ***

    private void ParseSavedTrainingData()
    {
        CastTrainingLevel();
        CastTrainingDay();
    }

    private void CastTrainingLevel()
    {
        switch (level)
        {
            case 1:
                trainingLevel = TrainingLevel.Begginer;
                break;
            case 2:
                trainingLevel = TrainingLevel.Rookie;
                break;
            case 3:
                trainingLevel = TrainingLevel.Medium;
                break;
            case 4:
                trainingLevel = TrainingLevel.Advance;
                break;
            default:
                break;
        }
    }

    private void CastTrainingDay()
    {
        switch (day)
        {
            case 1:
                trainingDay = TrainingDay.One;
                break;
            case 2:
                trainingDay = TrainingDay.Two;
                break;
            case 3:
                trainingDay = TrainingDay.Three;
                break;
            case 4:
                trainingDay = TrainingDay.Four;
                break;
            case 5:
                trainingDay = TrainingDay.Five;
                break;
            case 6:
                trainingDay = TrainingDay.Six;
                break;
            case 7:
                trainingDay = TrainingDay.Seven;
                break;
            default:
                break;
        }
    }

    #endregion
}
