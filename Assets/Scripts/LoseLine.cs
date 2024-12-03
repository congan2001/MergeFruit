using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoseLine : MonoBehaviour
{
    public List<Collider2D> touchedColliders = new List<Collider2D>();
    public bool isTouching = false;
    public Coroutine gameOverCoroutine;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("fruits"))
        {
            touchedColliders.Add(collision);
            isTouching = true;
            gameOverCoroutine = StartCoroutine(GameOver());
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (touchedColliders.Contains(collision))
        {
            touchedColliders.Remove(collision);
        }
        if (touchedColliders.Count == 0)
        {
            isTouching = false;
            StopCoroutine(gameOverCoroutine);
        }
    }

    private IEnumerator GameOver()
    {
        yield return new WaitForSeconds(3f);
        if (isTouching)
        {
            SceneManager.LoadSceneAsync(3);
        }
    }
}