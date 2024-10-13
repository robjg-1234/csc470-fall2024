using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class MyScript : MonoBehaviour
{
    public GameObject mainCam;
    public TMP_Text winScreen;
    Vector3 startingPos;
    public GameObject cam;
    CharacterController characterController;
    bool back = true;
    //public GameObject body;
    // Start is called before the first frame update
    void Start()
    {
        startingPos = transform.position;
        characterController = transform.GetComponent<CharacterController>();
        Debug.Log(startingPos.x +" "+ startingPos.y +" " + startingPos.z);
    }

    // Update is called once per frame
    void Update()
    {
        if (!back)
        {
            characterController.Move(startingPos - transform.position);
            if (transform.position == startingPos)
            {
                back = true;
            }
        }
        if (transform.position.y < -5f)
        {
            characterController.Move(startingPos - transform.position);

        }

    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Finish Line"))
        {
            Destroy(transform.gameObject);
            mainCam.SetActive(false);
            cam.SetActive(true);
            winScreen.text = "You win!";
        }
        if (other.CompareTag("Box"))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}
