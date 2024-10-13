using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuScript : MonoBehaviour
{
    public void loadGameOne()
    {
        SceneManager.LoadScene(1);
    }
    public void loadGameTwo()
    {
        SceneManager.LoadScene(2);
    }
}
