using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BuildScript : MonoBehaviour
{
    public string buildName;
    public string buildDescription;
    public int rate = 0;
    public int maxWorkers;
    public int currentWorkers = 0;
    public int baseProduction = 0;
    public int expectedWorkers = 0;
    private void OnEnable()
    {
        GameManager.rateChanges += changeRate;
    }
    
    void Start()
    {
        if (this.CompareTag("Crastle"))
        {
            GameManager.instance.mainBase = this;
            maxWorkers = 5;
            currentWorkers = 5;
        } else if (this.CompareTag("Pond"))
        {
            maxWorkers = 2;
            baseProduction = 5;
        } else if (this.CompareTag("Forest"))
        {
            maxWorkers = 3;
            baseProduction = 10;
        }
        else if (this.CompareTag("OreMine"))
        {
            maxWorkers = 5;
            baseProduction = 5;
        }
        else if (this.CompareTag("StoneCliff"))
        {
            maxWorkers = 0;
        }
        else if (this.CompareTag("ChoppedForest"))
        {
            maxWorkers = 3;
            baseProduction = 10;
        }
        else if (this.CompareTag("desolatedArea"))
        {
            maxWorkers = 0;
        }
        else if (this.CompareTag("House"))
        {
            maxWorkers = 0;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public int stateOfWorkers()
    {
        if (currentWorkers > 0 && currentWorkers+expectedWorkers < maxWorkers)
        {
            return 1;
        } else if(currentWorkers == 0 && expectedWorkers < maxWorkers)
        {
            return 2;
        }
        else
        {
            return 0;
        }
    }
    void changeRate()
    {
        rate = baseProduction*currentWorkers;
    }
    private void OnDestroy()
    {
        GameManager.instance.selectUnit(null);
        GameManager.rateChanges -= changeRate;
    }
}
