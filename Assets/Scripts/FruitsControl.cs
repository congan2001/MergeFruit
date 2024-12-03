using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FruitsControl : MonoBehaviour
{
    public int level;
    public int newLevel;
    public int maxLevel;
    public int score;

    public string[] fruitTypes = new string[]
    {
        "lychee", "kiwi", "orange", "apple", "lime", "dragonfruit", "cucumber", "passionfruit", "papaya", "grapefruit", "coconut", "watermelon"
    };

    public GameObject[] fruitPrefabs;
    public bool isCombining = false;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("fruits"))
        {
            FruitsControl otherFruit = collision.gameObject.GetComponent<FruitsControl>();
            if (this.level == otherFruit.level && !this.isCombining && !otherFruit.isCombining) // thêm điều kiện biến isCombining để chặn việc lặp lại CombineFruit() liên tục do Collider vẫn chồng nhau
            {
                this.isCombining = true;
                otherFruit.isCombining = true;
                Score(otherFruit);
                CombineFruit(otherFruit);
            }
        }
    }

    private void CombineFruit(FruitsControl otherFruit)
    {
        newLevel = this.level + 1;
        if (newLevel > maxLevel)
        {
            maxLevel = newLevel;
            FindObjectOfType<FruitIcons>().HighlightUI(maxLevel);
        }
        if (newLevel < fruitTypes.Length)
        {
            Combination(otherFruit);
            FindObjectOfType<FruitsDrop>().OnFruitCombined();
        }
        else if (newLevel == fruitTypes.Length)
        {
            Combination(otherFruit);
            StartCoroutine(Winning());
        }
    }

    private void Combination(FruitsControl otherFruit)
    {
        GameObject newFruitPrefab = fruitPrefabs[newLevel - 1];
        newFruitPrefab.transform.position = (this.transform.position + otherFruit.transform.position) / 2;
        Destroy(this.gameObject);
        Destroy(otherFruit.gameObject);
        GameObject newFruit = Instantiate(newFruitPrefab);
        Vector2 targetScale = newFruit.transform.localScale;                            // Gán targetScale đúng bằng scale của quả trong prefab
        newFruit.transform.localScale = Vector2.zero;                                   // Đặt Scale của quả lúc mới Instantiate ra về 0
        newFruit.transform.DOScale(targetScale, 0.8f).SetEase(Ease.OutBounce);          // Phóng to lên targetScale với thời gian là 1f
        newFruit.name = fruitPrefabs[newLevel - 1].name;
        newFruit.tag = "fruits";
    }

    private IEnumerator Winning()
    {
        yield return new WaitForSeconds(2f);
        SceneManager.LoadSceneAsync(2);
    }

    public void Score(FruitsControl otherFruit)
    {
        int tempScore = this.score + otherFruit.score;
        ScoreManager.sharedScore += tempScore;
    }
}