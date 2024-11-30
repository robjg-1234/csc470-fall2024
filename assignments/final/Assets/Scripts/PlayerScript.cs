using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class PlayerScript : MonoBehaviour
{
    PanelScript currentPuzzle;
    public GameObject playerCamera;
    float playerSpeed = 3f;
    float cameraRotationSpeed = 3f;
    float gravity = -10f;
    public CharacterController cc;
    float yVelocity = 0;
    float pitch;
    float yaw;
    Camera cam;
    Vector3 targetPosition;
    GameManager gm;
    // Start is called before the first frame update
    void Start()
    {
        gm = GameManager.instance;
        cam = playerCamera.GetComponent<Camera>();
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        if (currentPuzzle != null)
        {
            if (Vector3.Distance(targetPosition, transform.position) > 0.5f)
            {
                currentPuzzle.transform.GetComponent<Collider>().enabled = false;
                Vector3 getCloser = targetPosition - transform.position;
                cc.Move(getCloser * playerSpeed * Time.deltaTime);
                pitch = 0f;
                yaw = currentPuzzle.transform.rotation.eulerAngles.y + 180;
                playerCamera.transform.eulerAngles = new Vector3(pitch, yaw, 0f);
            }
            else
            {
                if (Input.GetMouseButtonDown(1))
                {
                    currentPuzzle.transform.GetComponent<Collider>().enabled = true;
                    currentPuzzle.isSelected = false;
                    currentPuzzle = null;
                }
            }
            
        }
        else
        {
            if (!cc.isGrounded)
            {
                yVelocity += gravity * Time.deltaTime;
            }
            else
            {
                yVelocity = -2;
            }

            float hAxis = Input.GetAxis("Horizontal");
            float vAxis = Input.GetAxis("Vertical");


            pitch += cameraRotationSpeed * Input.GetAxis("Mouse Y") * -1;
            yaw += cameraRotationSpeed * Input.GetAxis("Mouse X");
            //Limit how high or low you can go with the camera
            pitch = Mathf.Clamp(pitch, -90f, 90f);
            //Wrap around if a complete 360 is done (horizontal Axis)
            while (yaw < 0f)
            {
                yaw += 360f;
            }
            while (yaw >= 360f)
            {
                yaw -= 360f;
            }

            playerCamera.transform.eulerAngles = new Vector3(pitch, yaw, 0f);

            Vector3 amountToMove = playerCamera.transform.forward.normalized * playerSpeed * vAxis;
            amountToMove += playerCamera.transform.right.normalized * playerSpeed * hAxis;
            amountToMove.y += yVelocity;
            amountToMove *= Time.deltaTime;
            cc.Move(amountToMove);
            if (Input.GetMouseButtonDown(0))
            {
                Ray mousePositionRay = cam.ScreenPointToRay(Input.mousePosition);
                RaycastHit hitInfo;
                if (Physics.Raycast(mousePositionRay, out hitInfo, 1f))
                {
                    if (hitInfo.collider.CompareTag("Panel"))
                    {
                        currentPuzzle = hitInfo.collider.gameObject.GetComponent<PanelScript>();
                        if (currentPuzzle != null)
                        {
                            gm.puzzleID = currentPuzzle.PuzzleID;
                            currentPuzzle.isSelected = true;
                            targetPosition = currentPuzzle.transform.position + currentPuzzle.transform.forward * 1.5f;
                            targetPosition.y += 1f;
                        }
                        

                    }
                }
            }
        }
    }
}
