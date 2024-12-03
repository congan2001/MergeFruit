using UnityEngine;
using UnityEngine.UI;

public class ScoreDisplay : MonoBehaviour
{
    public Text totalScore;
    
    private void Update()
    {
        totalScore.text = ScoreManager.sharedScore.ToString("D6");   //ép kiểu về string D6 (định dạng chuỗi 6 chữ số), sau đó gán giá trị lên UI
    }
}
