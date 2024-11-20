using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;


public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public static Action rateChanges;
    public Camera mainCamera;
    public GameObject workerPrefab;
    public GameObject housePrefab;
    public GameObject prefabMine;
    public GameObject prefabchoppedWood;
    public GameObject buildPreview;
    public GameObject crastlePreview;
    public GameObject forestPreview;
    public GameObject stonesPreveiw;
    public GameObject choppedForestPreview;
    public GameObject oreMinePreveiw;
    public GameObject pondPreview;
    public GameObject endGameSummary;
    public GameObject desolatedAreaPreview;
    public TMP_Text summaryText;
    //Crastle Info
    public Slider happinessSlider;
    public TMP_Text oreRatesText;
    public TMP_Text foodRatesText;
    public TMP_Text woodRatesText;
    public TMP_Text available;
    //Everything else
    public TMP_Text buildName;
    public TMP_Text buildDescription;
    public TMP_Text woodText;
    public TMP_Text oreText;
    public TMP_Text foodText;
    public TMP_Text peopleText;
    public TMP_Text dayText;
    public TMP_Text dayTimer;
    //Resources
    int oreAmount = 25;
    int woodAmount = 50;
    int foodAmount = 20;
    int foodRate = 0;
    int woodRate = 0;
    int oreRate = 0;
    float timer = 1;
    int dayCount = 1;
    int hourCount = 0;
    bool gameEnded = false;
    int deathCount = 0;
    int houses = 1;
    List<GameObject> previews = new List<GameObject>();
    BuildScript selectedBuilding;
    BuildScript provisionalBuild;
    public BuildScript mainBase;
    public void addWorkers()
    {
        if (selectedBuilding != null)
        {
            if (selectedBuilding.stateOfWorkers() > 0)
            {
                if (mainBase.currentWorkers > 0)
                {
                    selectedBuilding.expectedWorkers++;
                    mainBase.currentWorkers--;
                    WorkerScript newWorker = Instantiate(workerPrefab, transform.position, Quaternion.identity).GetComponent<WorkerScript>();
                    newWorker.moveTowardsBuilding(selectedBuilding);
                    newGlobalRate(1, selectedBuilding.baseProduction);
                }
            }
        }
    }
    public void removeWorkers()
    {
        if (selectedBuilding != null)
        {
            if (selectedBuilding.stateOfWorkers() < 2)
            {
                
                selectedBuilding.currentWorkers--;
                WorkerScript newWorker = Instantiate(workerPrefab, selectedBuilding.transform.position, Quaternion.identity).GetComponent<WorkerScript>();
                newWorker.moveTowardsBuilding(mainBase);
                rateChanges.Invoke();
                newGlobalRate(-1, selectedBuilding.baseProduction);
            }
        }
    }
    private void OnEnable()
    {
        instance = this;
    }
    void Start()
    {
        previews.Add(crastlePreview);
        previews.Add(forestPreview);
        previews.Add(pondPreview);
        previews.Add(oreMinePreveiw);
        previews.Add(stonesPreveiw);
        previews.Add(choppedForestPreview);
        previews.Add(desolatedAreaPreview);
    }

    void Update()
    {
        if (!gameEnded)
        {
            crastleInfoPanel();
            updateTopPart();
            controlSecondaryTabs();
            rateChanges.Invoke();
            if (Input.GetKeyDown(KeyCode.Tab))
            {
                toggleTab();
            }
            if (!buildPreview.activeSelf)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    Ray mousePositionRay = mainCamera.ScreenPointToRay(Input.mousePosition);
                    RaycastHit hitInfo;
                    if (Physics.Raycast(mousePositionRay, out hitInfo, Mathf.Infinity))
                    {
                        selectedBuilding = hitInfo.collider.gameObject.GetComponent<BuildScript>();
                        if (selectedBuilding != null)
                        {
                            toggleTab();
                        }
                    }
                }
            }
            if (timer > 0)
            {
                timer -= Time.deltaTime;
            }
            else
            {
                timer = 1;
                hourCount += 1;
                if (hourCount > 23)
                {
                    foodAmount += foodRate;
                    oreAmount += oreRate;
                    woodAmount += woodRate;
                    happinessCheck();
                    
                    if (foodAmount < 0)
                    {
                        foodAmount = 0;
                    }
                    hourCount = 0;
                    dayCount += 1;
                    if (foodAmount >= mainBase.maxWorkers * 8)
                    {
                        mainBase.currentWorkers += 1;
                        mainBase.maxWorkers += 1;
                    }
                    if (foodAmount <= mainBase.maxWorkers * 2)
                    {
                        if (provisionalBuild != null)
                        {
                            if (provisionalBuild.currentWorkers > 0)
                            {
                                provisionalBuild.currentWorkers -= 1;
                            }
                            else
                            {
                                mainBase.currentWorkers -= 1;
                            }
                        }
                        else
                        {
                            mainBase.currentWorkers -= 1;
                        }
                        mainBase.maxWorkers -= 1;
                        deathCount += 1;
                    }
                    foodAmount -= mainBase.maxWorkers * 4;
                    if (happinessSlider.value <= 0 || deathCount>4 || dayCount>14)
                    {
                        gameEnded = true;
                    }
                }
            }
        }
        else
        {
            buildPreview.SetActive(false);
            endGame();
        }
    }
    public void toggleTab()
    {
        buildPreview.SetActive(!buildPreview.activeSelf);
    }

    void controlSecondaryTabs()
    {
        if (selectedBuilding != null)
        {
            buildName.text = selectedBuilding.buildName;
            buildDescription.text = selectedBuilding.buildDescription;
            foreach (GameObject g in previews)
            {
                if (!g.CompareTag(selectedBuilding.tag))
                {
                    g.SetActive(false);
                }
                else
                {
                    g.SetActive(true);
                    if (!g.CompareTag("StoneCliff") && !g.CompareTag("Crastle") && !g.CompareTag("desolatedArea") && !g.CompareTag("House"))
                    {
                        ManagerPanel changePanel = g.GetComponent<ManagerPanel>();
                        changePanel.panelUpdate(selectedBuilding.currentWorkers, selectedBuilding.rate);
                    }
                }
            }
        }
        else
        {
            buildName.text = "There is nothing selected";
            buildDescription.text = "Left click on any building to start.";
            foreach (GameObject g in previews)
            {
                g.SetActive(false);
            }
        }
    }
    public void selectUnit(BuildScript building)
    {
        selectedBuilding = building;
    }
    public void transformBuild()
    {
        if (selectedBuilding.CompareTag("Forest"))
        {
            if (oreAmount >= 25)
            {
                foodRate -= selectedBuilding.rate;
                mainBase.currentWorkers += selectedBuilding.currentWorkers;
                oreAmount -= 25;
                Vector3 currentPos = selectedBuilding.transform.position;
                Quaternion currentRotation = selectedBuilding.transform.rotation;
                Destroy(selectedBuilding.gameObject);
                GameObject newBuild = Instantiate(prefabchoppedWood, currentPos, currentRotation);
                toggleTab();
            }
        }
        else if (selectedBuilding.CompareTag("StoneCliff"))
        {
            if (woodAmount >= 75)
            {
                woodAmount -= 75;
                Vector3 currentPos = selectedBuilding.transform.position;
                Quaternion currentRotation = selectedBuilding.transform.rotation;
                Destroy(selectedBuilding.gameObject);
                GameObject newBuild = Instantiate(prefabMine, currentPos, currentRotation);
                toggleTab();
            }
        }
        else if (selectedBuilding.CompareTag("desolatedArea"))
        {
            if (woodAmount>=30 && oreAmount >= 50)
            {
                woodAmount-=30;
                oreAmount -= 50;
                Vector3 currentPos = selectedBuilding.transform.position;
                Quaternion currentRotation = selectedBuilding.transform.rotation;
                Destroy(selectedBuilding.gameObject);
                GameObject newBuild = Instantiate(housePrefab, currentPos, currentRotation);
                toggleTab();
                houses += 1;
            }
        }
    }

    void updateTopPart()
    {
        oreText.text = oreAmount.ToString();
        foodText.text = foodAmount.ToString();
        woodText.text = woodAmount.ToString();
        peopleText.text = mainBase.maxWorkers.ToString();
        dayText.text = "Day " + dayCount;
        dayTimer.text = hourCount + ":00";
    }

    void newGlobalRate(int changeState, int rateModifier)
    {
        if (selectedBuilding != null)
        {
            if (selectedBuilding.CompareTag("Pond") || selectedBuilding.CompareTag("Forest"))
            {
                foodRate += rateModifier * changeState;
            }
            else if (selectedBuilding.CompareTag("ChoppedForest"))
            {
                woodRate += rateModifier * changeState;
            }
            else if (selectedBuilding.CompareTag("OreMine"))
            {
                oreRate += rateModifier * changeState;
            }
        }
    }

    void crastleInfoPanel()
    {
        available.text = mainBase.currentWorkers + "/" + mainBase.maxWorkers;
        foodRatesText.text = (mainBase.maxWorkers * 4) + "/day";
        oreRatesText.text = oreRate + "/day";
        woodRatesText.text = woodRate + "/day";
    }

    void happinessCheck()
    {
        if (foodAmount <= mainBase.maxWorkers * 2)
        {
            happinessSlider.value -= 0.3f;
        }
        else if (foodAmount < mainBase.maxWorkers * 4)
        {
            happinessSlider.value -= 0.2f;
        }
        else
        {
            if (happinessSlider.value + 0.05f < 1f)
            {
                happinessSlider.value += 0.05f;
            }
            else
            {
                happinessSlider.value = 1f;
            }
        }
        if (mainBase.currentWorkers > 0)
        {
            happinessSlider.value -= 0.1f;
        }
        else
        {
            if (happinessSlider.value + 0.05f < 1f)
            {
                happinessSlider.value += 0.05f;
            }
            else
            {
                happinessSlider.value = 1f;
            }
        }
        if (houses*4 < mainBase.maxWorkers)
        {
            happinessSlider.value -= 0.05f;
        } else
        {
            if (happinessSlider.value + 0.3f < 1f)
            {
                happinessSlider.value += 0.3f;
            }
            else
            {
                happinessSlider.value = 1f;
            }
        }
    }
    void endGame()
    {
        if (dayCount > 14)
        {
            if (happinessSlider.value > 0.75 && deathCount == 0)
            {
                summaryText.text = "Congratulations! You survived the 14 days and kept your people happy.";
            }
            else if (happinessSlider.value > 0.75 && deathCount < 3)
            {
                summaryText.text = "Congratulations! You survived the 14 days and kept your people happy, but at the cost of some individuals.";
            }
            else
            {
                summaryText.text = "Unfortunate! You were able to survive the 14 days but your people were not happy which led to a revolution and your imminent death.";
            }
        }
        else
        {
            summaryText.text = "Despite your best efforts, you couldn't provide enough help to your people, you were exiled from the community and lived your last days alone.";
        }
        endGameSummary.SetActive(true);
    }
}
