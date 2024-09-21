using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PlaneScript : MonoBehaviour
{
    public Slider slider;
    public Camera mainCamera;
    float xRotationSpeed = 70f;
    float yRotationSpeed = 100f;
    float zRotationSpeed = 20f;
    float zReturnRotationSpeed = 30f;
    float zStrafeSpeed = 180f;
    float forwardSpeed = 25f;
    int ringsRemaining = 10;
    public TMP_Text goalCounter;
    public TMP_Text gameState;
    public TMP_Text timer;
    bool isGameRunning = false;
    bool isGameWon = false;
    float timeRemaining = 90f;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (isGameRunning)
        {

            if (timeRemaining > 0)
            {
                timeRemaining -= Time.deltaTime;
            } else
            {
                gameState.text = "You ran out of time. Press Space to try again";
                isGameRunning = false;
                gameObject.SetActive(false);
            }
            // Plane rotation 
            float hAxis = Input.GetAxis("Horizontal");
            float vAxis = Input.GetAxis("Vertical");
            float zCurrentRotation = transform.localRotation.eulerAngles.z;
            Vector3 planeRotation = new Vector3(0, 0, 0);
            planeRotation.x = vAxis * xRotationSpeed;
            planeRotation.y = hAxis * yRotationSpeed;
            planeRotation.z = -1 * hAxis * zRotationSpeed;

            if (Input.GetKey("q"))
            {
                planeRotation.z = 1 * zStrafeSpeed;
                hAxis = 1;
            }
            else if (Input.GetKey("e"))
            {
                planeRotation.z = -1 * zStrafeSpeed;
                hAxis = 1;
            }
            //Plane stabilizer
            if (hAxis == 0)
            {
                if (zCurrentRotation > 180f)
                {
                    planeRotation.z = 1 * zReturnRotationSpeed;

                }
                else if (zCurrentRotation > 1.1f)
                {
                    planeRotation.z = zReturnRotationSpeed * -1f;
                }
                else
                {
                    planeRotation.z = 0;
                }
            }
            planeRotation *= Time.deltaTime;
            transform.Rotate(planeRotation, Space.Self);
            // Using boost
            if (Input.GetKey("space") && (slider.value > 0))
            {
                if (slider.value > 0)
                {
                    if (forwardSpeed < 50f)
                    {
                        forwardSpeed += 10f * Time.deltaTime;
                    }
                    slider.value -= 0.1f * Time.deltaTime;
                }
            }
            else
            {
                if (forwardSpeed > 25f)
                {
                    forwardSpeed -= 5f * Time.deltaTime;
                }
            }

            // plane movement
            transform.position += transform.forward * forwardSpeed * Time.deltaTime;

            // Camera
            Vector3 cameraPosition = transform.position;
            cameraPosition += -transform.forward * 25f;
            cameraPosition += Vector3.up * 10f;
            mainCamera.transform.position = cameraPosition;
            mainCamera.transform.LookAt(transform.position);
            if (ringsRemaining <= 0)
            {
                gameState.text = "You won!";
                forwardSpeed = 0f;
                isGameWon = true;
                isGameRunning = false;
            }
            timer.text = "Time remaining: " + Mathf.Round(timeRemaining);
        }
        else { 
            if (Input.GetKeyDown("space") && !isGameWon)
            {
                isGameRunning = true;
                transform.position = new Vector3(36.5f, 11.4399996f, 12.8999996f);
                gameState.text = "";
            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ring"))
        {
            ringsRemaining--;
            goalCounter.text = "Rings remaining: " + ringsRemaining;
            Destroy(other.gameObject);
        }
        else if (other.CompareTag("Fuel"))
        {
            if ((slider.value + 0.5f) > 1)
            {
                slider.value = 1;
            }
            else
            {
                slider.value += 0.5f;
            }
            Destroy(other.gameObject);
        } else if (other.gameObject.name == "Terrain")
        {
            gameObject.SetActive(false);
            gameState.text = "You crashed! Press Space to start again";
            isGameRunning = false;
        } else if (other.CompareTag("Roof"))
        {
            Vector3 goDown = transform.position;
            goDown.y -= 0.5f;
            transform.position = goDown;
            transform.Rotate(45, 0, 0, Space.World);
        } else if (other.CompareTag("Wall"))
        {
            gameState.text = "You went out of bounds! Press space to start again";
            isGameRunning = false;
            gameObject.SetActive(false);
        }
    }
    
}