using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class FruitsDrop : MonoBehaviour
{
    public bool canSpawn = true;
    public bool popActive, pickNextActive, boomActive = false;
    public Button boom, pop, pick, hold;
    public float explodedRadius;
    public float maxX = 2.4f;
    public float minX = -1f;
    public GameObject pickNextMenu;
    public GameObject[] fruitObj;
    public Image currentFruitImage, secondFruitImage, thirdFruitImage, holdFruitImage, nextFruitImage;
    public int boomUsageCount, popUsageCount, pickUsageCount;
    public int combinedThreshold;                                              // số lần combine cần để spawn quả mới
    public int currentIndex, secondIndex, thirdIndex, pickedIndex;
    public int maxSpawnLevel;                                                  // cấp độ spawn tối đa, cộng 1 theo mỗi lần đạt mốc Combine
    public int successfulCombinations;                                         // số lần combine thành công
    public int? holdIndex = null;
    public LayerMask fruitLayer;
    public Sprite currentFruitSprite, secondFruitSprite, thirdFruitSprite, nextFruitSprite;
    public Text boomUsageNumber, popUsageNumber, pickUsageNumber, totalScore;

    //Vị trí và điều kiện spawn quả
    private void Update()
    {
        // Vị trí spawn theo chuột
        Vector2 worldMousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        worldMousePosition.x = Mathf.Clamp(worldMousePosition.x, minX, maxX);
        worldMousePosition.y = transform.position.y;
        transform.position = worldMousePosition;

        // Spawn fruit theo vị trí chuột
        if (Input.GetMouseButtonDown(0) && canSpawn && !CheckTouchUI() && Time.timeScale == 1)
        {
            {
                SpawnFruit();
            }
        }
        SeeNextFruit();

        // Boom items
        if (Input.GetMouseButtonDown(0) && boomActive && !CheckTouchUI() && Time.timeScale == 1)
        {
            Vector2 boomPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Collider2D[] ExplodedObjects = Physics2D.OverlapCircleAll(boomPosition, explodedRadius, fruitLayer);
            foreach (Collider2D hit in ExplodedObjects)
            {
                if (hit.CompareTag("fruits"))
                {
                    Destroy(hit.gameObject);
                }
            }
            UsageCount();
            boomActive = false;
            canSpawn = true;
            pop.interactable = true;
            pick.interactable = true;
            hold.interactable = true;
        }

        // Pop items
        if (Input.GetMouseButtonDown(0) && popActive && !CheckTouchUI() && Time.timeScale == 1)
        {
            Vector2 popPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(popPosition, Vector2.zero);
            if (hit.collider != null)
            {
                string objName = hit.collider.name;
                GameObject[] allFruits = GameObject.FindGameObjectsWithTag("fruits");
                foreach (GameObject fruit in allFruits)
                {
                    if (fruit.name == objName)
                    {
                        Destroy(fruit);
                    }
                }
            }
            UsageCount();
            popActive = false;
            canSpawn = true;
            boom.interactable = true;
            pick.interactable = true;
            hold.interactable = true;
        }
    }

    //Check chạm UI
    public bool CheckTouchUI()
    {
        EventSystem eventSystem = EventSystem.current;
        if (eventSystem.IsPointerOverGameObject())
        {
            return true;
        }
        return false;
    }

    //Time Delay để tránh spam spawn
    private IEnumerator TimeDelay()
    {
        canSpawn = false;
        yield return new WaitForSeconds(0.75f);                  // Sau 0.75s canSpawn được gán lại true để có thể thực hiện lệnh if bên trên
        canSpawn = true;
    }

    //Tăng khoảng quả được spawn dựa trên số lần combine thành công
    public void OnFruitCombined()
    {
        successfulCombinations++;                                                                 // cộng 1 sau mỗi lần Combine
        if (successfulCombinations >= combinedThreshold && maxSpawnLevel < fruitObj.Length)
        {
            maxSpawnLevel++;                                    // tăng maxSpawnLevel => tăng cấp độ spawn quả
            if (maxSpawnLevel > fruitObj.Length - 5)
            {
                maxSpawnLevel = fruitObj.Length - 5;            // chỉ cho spawn quả ở mốc fruitObj.Length - 5
            }
            successfulCombinations = 0;                         // đặt trở lại về 0 để đếm sau mỗi lần tăng maxSpawnLevel
        }
    }

    //Spawn quả
    private void SpawnFruit()
    {
        GameObject fruitSpawn = fruitObj[currentIndex];
        fruitSpawn.transform.position = transform.position;
        GameObject newFruit = Instantiate(fruitSpawn);
        newFruit.name = fruitObj[currentIndex].name;
        newFruit.tag = "fruits";
        currentIndex = secondIndex;
        secondIndex = thirdIndex;
        thirdIndex = Random.Range(0, maxSpawnLevel);
        StartCoroutine(TimeDelay());
    }

    //Danh sách quả kế tiếp
    private void SeeNextFruit()
    {
        CurrentFruitImage();
        SecondFruitImage();
        ThirdFruitImage();
    }

    //Giữ quả
    public void HoldFruit()
    {
        if (holdIndex == null && Time.timeScale == 1)
        {
            holdIndex = currentIndex;
            currentIndex = secondIndex;
            secondIndex = thirdIndex;
            thirdIndex = Random.Range(0, maxSpawnLevel);
            Sprite holdFruitSprite = fruitObj[(int)holdIndex].GetComponent<SpriteRenderer>().sprite;
            holdFruitImage.sprite = holdFruitSprite;
            CurrentFruitImage();
            SecondFruitImage();
            ThirdFruitImage();
        }
        else if (holdIndex != null && Time.timeScale == 1)
        {
            currentIndex = (int)holdIndex;
            holdIndex = null;
            holdFruitImage.sprite = null;
            CurrentFruitImage();
        }
    }

    //Hiện danh sách quả kế tiếp lên UI
    private void CurrentFruitImage()
    {
        currentFruitSprite = fruitObj[currentIndex].GetComponent<SpriteRenderer>().sprite;
        currentFruitImage.sprite = currentFruitSprite;
    }

    private void SecondFruitImage()
    {
        secondFruitSprite = fruitObj[secondIndex].GetComponent<SpriteRenderer>().sprite;
        secondFruitImage.sprite = secondFruitSprite;
    }

    private void ThirdFruitImage()
    {
        thirdFruitSprite = fruitObj[thirdIndex].GetComponent<SpriteRenderer>().sprite;
        thirdFruitImage.sprite = thirdFruitSprite;
    }

    // Trợ giúp
    public void BoomItems()
    {
        if (Time.timeScale == 1 && boomUsageCount > 0)
        {
            if (!boomActive)
            {
                boomActive = true;
                canSpawn = false;
                pop.interactable = false;
                pick.interactable = false;
                hold.interactable = false;
            }
            else if (boomActive)
            {
                boomActive = false;
                canSpawn = true;
                pop.interactable = true;
                pick.interactable = true;
                hold.interactable = true;
            }
        }
    }

    public void PopItems()
    {
        if (Time.timeScale == 1 && popUsageCount > 0)
        {
            if (!popActive)
            {
                popActive = true;
                canSpawn = false;
                boom.interactable = false;
                pick.interactable = false;
                hold.interactable = false;
            }
            else if (popActive)
            {
                popActive = false;
                canSpawn = true;
                boom.interactable = true;
                pick.interactable = true;
                hold.interactable = true;
            }
        }
    }

    public void PickNextMenu()
    {
        if (Time.timeScale == 1 && pickUsageCount > 0)
        {
            if (!pickNextActive)
            {
                pickNextMenu.SetActive(true);
                pickNextActive = true;
                canSpawn = false;
                boom.interactable = false;
                pop.interactable = false;
                hold.interactable = false;
            }
            else if (pickNextActive)
            {
                pickNextMenu.SetActive(false);
                pickNextActive = false;
                canSpawn = true;
                boom.interactable = true;
                pop.interactable = true;
                hold.interactable = true;
            }
        }
    }

    // Logic của PickNextMenu
    public void NextItem()
    {
        pickedIndex++;
        if (pickedIndex >= maxSpawnLevel)
        {
            pickedIndex = maxSpawnLevel - 1;                 // Item có thể chọn chỉ ở trong khoảng (0, maxSpawnLevel)
        }
        PickedFruitImage();
    }

    public void LastItem()
    {
        if (pickedIndex > 0)
        {
            pickedIndex--;
        }
        PickedFruitImage();
    }

    private void PickedFruitImage()
    {
        nextFruitSprite = fruitObj[pickedIndex].GetComponent<SpriteRenderer>().sprite;
        nextFruitImage.sprite = nextFruitSprite;
        Debug.Log(fruitObj[pickedIndex].name);
    }

    public void PickNextFruit()
    {
        currentIndex = pickedIndex;
        pickNextMenu.SetActive(false);
        UsageCount();
        pickNextActive = false;
        canSpawn = true;
        boom.interactable = true;
        pop.interactable = true;
        hold.interactable = true;
    }

    //Đếm số lượt dùng trợ giúp còn lại
    private void UsageCount()
    {
        if (boomActive)
        {
            boomUsageCount--;
            if (boomUsageCount < 0)
            {
                boomUsageCount = 0;
            }
            boomUsageNumber.text = $"{boomUsageCount}";
        }
        else if (popActive)
        {
            popUsageCount--;
            if (popUsageCount < 0)
            {
                popUsageCount = 0;
            }
            popUsageNumber.text = $"{popUsageCount}";
        }
        else if (pickNextActive)
        {
            pickUsageCount--;
            if (pickUsageCount < 0)
            {
                pickUsageCount = 0;
            }
            pickUsageNumber.text = $"{pickUsageCount}";
        }
    }
}