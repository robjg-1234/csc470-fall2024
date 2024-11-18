using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ManagerPanel : MonoBehaviour
{
    public TMP_Text workersCount;
    public TMP_Text rate;

    public void panelUpdate(int workers, int rateInc)
    {
        rate.text = rateInc + "/day";
        workersCount.text = workers + "";
    }
}
