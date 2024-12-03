using UnityEngine;
using UnityEngine.UI;

public class FruitIcons : MonoBehaviour
{
    private int temp;
    public GameObject[] fruitsUI;
    public Color imageHighlightColor = new Color(); 
    public Color textHighlightColor = new Color(); 

    public void HighlightUI(int maxLevel)
    {
        temp = maxLevel;
        HighlightImage();
        HighlightText();    
    }

    private void HighlightImage()
    {
        Image image = fruitsUI[temp - 1].GetComponentInChildren<Image>();
        image.color = imageHighlightColor;
    }

    private void HighlightText()
    {
        Text text = fruitsUI[temp - 1].GetComponentInChildren<Text>();
        text.color = textHighlightColor;
    }
}
