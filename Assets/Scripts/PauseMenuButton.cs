using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenuButton : MonoBehaviour
{
    private Image soundImage;
    public bool isOn, isActive = false;
    public GameObject soundButton;
    public GameObject pauseMenu;
    public Sprite soundOff;
    public Sprite soundOn;

    public void Start()
    {
        soundImage = soundButton.GetComponent<Image>();
    }

    public void RestartGame()
    {
        ScoreManager.sharedScore = 0;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Time.timeScale = 1;
    }

    public void Pause()
    {
        isActive = !isActive;
        if (isActive)
        {
            pauseMenu.SetActive(true);
            Time.timeScale = 0;
        }
        else
        {
            pauseMenu.SetActive(false);
            Time.timeScale = 1;
        }
    }

    public void Resume()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1;
        isActive = false;
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