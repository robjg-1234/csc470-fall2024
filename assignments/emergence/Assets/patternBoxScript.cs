using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class patternBoxScript : MonoBehaviour
{
    public bool isAlive;
    public Renderer rend;
    public bool isTarget;

    // Start is called before the first frame update
    void Start()
    {
        setColor();
    }

    public void setColor()
    {
        if (isAlive)
        {
            rend.material.color = new Color(46f / 255f, 158f / 255f, 38f / 255f);
        }
        else
        {
            rend.material.color = new Color(1f, 0f, 0.168627451f);
        }
    }
    private void OnMouseDown()
    {
        if (!isTarget)
        {
            isAlive = !isAlive;
            setColor();
        }

    }
    public void becomeAlive()
    {
        isAlive = true;
        setColor();
    }
}
