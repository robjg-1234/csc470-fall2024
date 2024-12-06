using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    static public GameManager instance;
    public Action levelCompleted;
    public int puzzleID;
    public GameObject pauseMenu;
    public GameObject settingsMenu;
    public Slider sensSlider;
    public TMP_InputField sensField;
    bool isPaused = false;
    public Action pausedGame;
    public bool pauseAvailable = true;
    public Image fade;
    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        StartCoroutine(fadeIn());
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !isPaused && pauseAvailable && !fade.gameObject.activeSelf)
        {
            isPaused = true;
            pauseMenu.SetActive(true);
        }
    }
    public void userBeatAPuzzle()
    {
        levelCompleted?.Invoke();
    }
    public void changeTextValue()
    {
        sensField.text = Math.Round(sensSlider.value, 2).ToString();
    }

    public void setSliderValue()
    {
        float val = float.Parse(sensField.text);
        sensSlider.value = Mathf.Clamp(val, 1f, 10f);
    }
    public void openSettings()
    {
        pauseMenu.SetActive(false);
        settingsMenu.SetActive(true);
    }
    public void closeSettings()
    {
        settingsMenu.SetActive(false);
        pauseMenu.SetActive(true);
    }
    public void closeMenu()
    {
        pauseMenu.SetActive(false);
        isPaused = false;
        pausedGame?.Invoke();
    }

    IEnumerator fadeIn()
    {
        while (fade.color.a > 0)
        {
            fade.color = new Color(fade.color.r, fade.color.g, fade.color.b, fade.color.a - 0.5f * Time.deltaTime);
            yield return null;
        }
        fade.gameObject.SetActive(false);
    }
}
