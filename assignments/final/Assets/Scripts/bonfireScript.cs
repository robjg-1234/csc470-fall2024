using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ProBuilder;
using UnityEngine.UI;

public class bonfireScript : MonoBehaviour
{
    public GameObject fireParticles;
    GameManager gameManager;
    int puzzleCount = 25;
    bool endGame = false;
    public Image finalPanel;
    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameManager.instance;
        gameManager.levelCompleted += puzzleCheck;
    }
    void puzzleCheck()
    {
        puzzleCount--;
        if (puzzleCount < 1)
        {
            fireParticles.SetActive(true);
            endGame = true;
        }
    }
    public bool tryEndGame()
    {
        if (endGame)
        {
            finalPanel.gameObject.SetActive(true);
            StartCoroutine(FinishGame());
            return true;
        }
        return false;
    }
    IEnumerator FinishGame()
    {
        while (finalPanel.color.a <1)
        {
            finalPanel.color = new Color(finalPanel.color.r, finalPanel.color.g, finalPanel.color.b, finalPanel.color.a + 0.5f * Time.deltaTime);
            yield return null;
        }
        Application.Quit();
    }
}
