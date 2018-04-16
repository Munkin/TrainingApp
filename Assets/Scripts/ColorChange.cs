using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class ColorChange
{
    #region Properties

    [Header("Color Change")]

    [SerializeField]
    private Color warmingUpColor;
    [SerializeField]
    private Color trainingColor;
    [SerializeField]
    private Color stretchingColor;

    [Space(10f)]

    [SerializeField]
    private Color normalColor;

    #endregion

    #region Class functions

    public void ChangeColor(Text text, TrainingStage stage)
    {
        switch (stage)
        {
            case TrainingStage.WarmingUp:
                CaseWarmingUp(text);
                break;

            case TrainingStage.Training:
                CaseTraining(text);
                break;

            case TrainingStage.Stretching:
                CaseStretching(text);
                break;

            default:
                break;
        }
    }

    public void ResetToNormalColor(Text text)
    {
        text.color = normalColor;
    }

    private void CaseWarmingUp(Text text)
    {
        text.color = warmingUpColor;
    }
    
    private void CaseTraining(Text text)
    {
        text.color = trainingColor;
    }

    private void CaseStretching(Text text)
    {
        text.color = stretchingColor;
    }

    #endregion
}
