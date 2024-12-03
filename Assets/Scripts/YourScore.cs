using UnityEngine;
using UnityEngine.UI;

public class YourScore : MonoBehaviour
{
    public Text yourScore;

    private void Update()
    {
        yourScore.text = "Your score: " + ScoreManager.sharedScore.ToString("D6"); //lấy biến tĩnh sharedScore từ class ScoreManager để hiển thị
    }
}
