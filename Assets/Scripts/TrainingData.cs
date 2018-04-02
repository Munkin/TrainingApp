using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TrainingData", menuName = "Assets/TrainingData", order = 2)]
public class TrainingData : ScriptableObject {

    #region Properties

    public TrainingDay day;
    public TrainingLevel level;

    [Space(10f)]

    public TrainingScreenData[] trainingScreens;

    #endregion

    #region Unity function

    #endregion
}
