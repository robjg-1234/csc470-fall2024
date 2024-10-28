using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class normalRedPlatform : MonoBehaviour
{
    public Renderer rend;
    float stateTimer = 3f;
    bool state = true;
    // Start is called before the first frame update
    void Start()
    {
        updateState();
    }

    // Update is called once per frame
    void Update()
    {
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
            rend.material.color = Color.red;
            transform.GetComponent<Collider>().enabled = true;
        }
        else
        {
            transform.GetComponent<Collider>().enabled = false;
            rend.material.color = new Color(0, 0, 0, 0);
        }
    }
}
