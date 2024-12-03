using UnityEngine;
using UnityEngine.SceneManagement;

public class WinningSceneButton : MonoBehaviour
{
    public void MainMenu()
    {
        SceneManager.LoadSceneAsync(0);
    }

}
