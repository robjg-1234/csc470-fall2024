using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bluePlatform : MonoBehaviour
{
    public Renderer rend;
    float movingTimer = 3f;
    bool state = false;
    float stateTimer = 3f;
    float platformSpeed = 10f;
    public bool isForward = false;
    public bool isAliive = true;
    // Start is called before the first frame update
    void Start()
    {
        updateState();
    }

    // Update is called once per frame
    void Update()
    {
        if (movingTimer > 0)
        {
            if (!isForward)
            {
                transform.position += transform.forward * platformSpeed * Time.deltaTime * -1;
            }
            else
            {
                transform.position += transform.forward * platformSpeed * Time.deltaTime;
            }
            movingTimer -= Time.deltaTime;
        }
        else
        {
            isForward = !isForward;
            movingTimer = 3f;
        }
        if (stateTimer > 0)
        {
            stateTimer -= Time.deltaTime;
        }
        else
        {
            state = !state;
            updateState();
            stateTimer = 3f;
        }
    }
    void updateState()
    {
        if (state)
        {
            rend.material.color = Color.blue;
            transform.GetComponent<Collider>().enabled = true;
            isAliive = true;
        }
        else
        {
            transform.GetComponent<Collider>().enabled = false;
            rend.material.color = new Color(0, 0, 0, 0);
            isAliive = false;
        }
    }
}
