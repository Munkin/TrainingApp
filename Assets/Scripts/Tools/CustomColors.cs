using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;

public class CustomColors : MonoBehaviour {

    [Button]
    public void ChangeColor()
    {
        Image[] images = FindObjectsOfType<Image>();
        Text[] texts = FindObjectsOfType<Text>();

        foreach (Image image in images)
        {
            if (image.color.r >= Color.white.r &&
                image.color.g >= Color.white.g &&
                image.color.b >= Color.white.b)
            {
                image.color = new Color(
                    255 - image.color.r,
                    255 - image.color.g,
                    255 - image.color.b,
                    image.color.a);
            }
        }

        foreach (Text text in texts)
        {
            if (text.color.r >= Color.white.r &&
                text.color.g >= Color.white.g &&
                text.color.b >= Color.white.b)
            {
                text.color = new Color(
                    255 - text.color.r,
                    255 - text.color.g,
                    255 - text.color.b,
                    text.color.a);
            }
        }
    }
}
