using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuScript : MonoBehaviour
{
    public Image fade;
    IEnumerator fadeIntoLoad()
    {
        while (fade.color.a<1)
        {
            fade.color = new Color(fade.color.r, fade.color.g, fade.color.b, fade.color.a +0.5f * Time.deltaTime);
            yield return null;
        }
        SceneManager.LoadScene(1);
    }
    public void loadGame()
    {
        fade.gameObject.SetActive(true);
        StartCoroutine(fadeIntoLoad());
    }
}
