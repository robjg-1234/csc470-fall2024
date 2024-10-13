using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BoxScript : MonoBehaviour
{
    public bool isAlive;
    public Renderer rend;
    //public CharacterController player;
    
    // Start is called before the first frame update
    void Start()
    {
        //player = GameManager.FindAnyObjectByType<CharacterController>();
        setColor();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void setColor()
    {
        if (isAlive)
        {
            gameObject.GetComponent<Collider>().isTrigger = false;
            rend.material.color = new Color(46f / 255f, 158f / 255f, 38f / 255f);
        }
        else
        {
            gameObject.GetComponent<Collider>().isTrigger = true;
            rend.material.color = new Color(1f, 0f, 0.168627451f);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        //if (!isAlive)
        //{
        //    player.Move(new Vector3(7.809491f, 0.8145716f, -8.628792f) - player.transform.position);
        //}
    }
}
