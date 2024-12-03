using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuSceneButtons : MonoBehaviour
{
    private Image soundImage;
    public bool isOn = false;
    public GameObject soundButton;
    public Sprite soundOff;
    public Sprite soundOn;

    public void Start()
    {
        soundImage = soundButton.GetComponent<Image>();
    }
    public void PlayGame()
    {
        ScoreManager.sharedScore = 0;
        SceneManager.LoadSceneAsync(1);
    }

    public void SoundCheck()
    {
        isOn = !isOn;
        if (isOn)
        {
            soundImage.sprite = soundOff;
        }
        else
        {
            soundImage.sprite = soundOn;
        }
    }
}