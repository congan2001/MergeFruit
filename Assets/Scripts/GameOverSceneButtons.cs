using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverSceneButtons : MonoBehaviour
{
    public void RestartGame()
    {
        ScoreManager.sharedScore = 0;
        SceneManager.LoadSceneAsync(1);
    }

    public void MainMenu()
    {
        SceneManager.LoadSceneAsync(0);
    }
}