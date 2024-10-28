using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class ChracterController : MonoBehaviour
{
    //Objective Manager
    int goldenCoins = 0;
    bool notWon = true;
    public TMP_Text objective;
    public TMP_Text coinsCollected;
    public TMP_Text instructions;

    public GameObject star;
    public Renderer rend;
    public GameObject playerCamera;
    public CharacterController cc;
    float rotateSpeed = 1;
    float playerRotationSpeed = 5f;
    float moveSpeed = 8f;
    float jumpVelocity = 8;
    public GameObject playerModel;
    Color defaultColor;
    GameObject platform;
    // These will be used to simulate gravity, and for jumping
    float yVelocity = 0;
    float gravity = -18f;
    //Long Jump speed
    float longJumpVelocity = 5f;
    float longJumpForward = 0f;
    float longJumpForwardVelocity = 18f;
    float friction = -15f;
    // superjump mechanic
    float SuperJump = 16f;
    float superJumpChargeTime = 1.5f;
    float fallingTime = 0;
    float coyoteTime = 0.5f;
    bool jumped = false;

    float jumpBuffer = 0.0f;
    // Start is called before the first frame update
    //platform
    Vector3 amountPlatformMoved;
    Vector3 prevPos;
    void Start()
    {
        defaultColor = rend.material.color;
    }

    // Update is called once per frame
    void Update()
    {
        if (notWon)
        {
            float hAxis = Input.GetAxis("Horizontal");
            float vAxis = Input.GetAxis("Vertical");
            if (!cc.isGrounded)
            {
                rend.material.color = defaultColor;
                fallingTime += Time.deltaTime;

                if (fallingTime < coyoteTime && Input.GetKeyDown(KeyCode.Space) && !jumped)
                {
                    yVelocity = jumpVelocity;
                }
                yVelocity += gravity * Time.deltaTime;
            }
            else
            {
                yVelocity = -2;

                fallingTime = 0;
                if (Input.GetKey(KeyCode.LeftShift))
                {
                    rend.material.color = new Color32(3, 182, 252, 255);
                    if (Input.GetKeyDown(KeyCode.Space) || jumpBuffer > 0)
                    {
                        jumped = true;
                        yVelocity = longJumpVelocity;
                        longJumpForward = longJumpForwardVelocity;
                        rend.material.color = defaultColor;
                    }
                }
                else if (Input.GetKey(KeyCode.C))
                {
                    if (superJumpChargeTime > 0)
                    {
                        superJumpChargeTime -= Time.deltaTime;
                        if (Input.GetKeyDown(KeyCode.Space) || jumpBuffer > 0)
                        {
                            jumped = true;
                            yVelocity = jumpVelocity;
                            superJumpChargeTime = 1.5f;
                        }
                    }
                    else if (superJumpChargeTime <= 0)
                    {
                        rend.material.color = new Color32(42, 161, 73, 255);
                        if (Input.GetKeyDown(KeyCode.Space))
                        {
                            jumped = true;
                            yVelocity = SuperJump;
                            superJumpChargeTime = 1.5f;
                            rend.material.color = defaultColor;
                        }
                    }
                }
                else
                {
                    if (Input.GetKeyDown(KeyCode.Space) || jumpBuffer > 0)
                    {
                        jumped = true;
                        yVelocity = jumpVelocity;
                    } else
                    {
                        jumped = false;
                    }
                    rend.material.color = defaultColor;
                    superJumpChargeTime = 1.5f;
                }

                if (longJumpForward > 0)
                {
                    longJumpForward += friction * Time.deltaTime;
                    longJumpForward = Mathf.Clamp(longJumpForward, 0, 10000);
                }
            }
            if (Input.GetKeyDown(KeyCode.Space))
            {
                jumpBuffer = 0.2f;
            }
            else
            {
                if (jumpBuffer > 0)
                {
                    jumpBuffer -= Time.deltaTime;
                }
            }
            // Moving with the camera
            Vector3 amountToMove = playerCamera.transform.forward;
            amountToMove.y = 0.0f;
            amountToMove = amountToMove.normalized;
            amountToMove = amountToMove * vAxis * moveSpeed;
            amountToMove += playerModel.transform.forward * longJumpForward;


            if (hAxis > 0 || hAxis < 0)
            {
                if (playerCamera.transform.rotation.eulerAngles.y <= 5 || playerCamera.transform.rotation.eulerAngles.y >= 355)
                {
                    playerCamera.transform.Rotate(Vector3.up * hAxis * rotateSpeed * Time.deltaTime * -1);
                }
                else if (playerCamera.transform.rotation.eulerAngles.y > 350 && hAxis != -1)
                {
                    playerCamera.transform.Rotate(Vector3.up * 1 * rotateSpeed * Time.deltaTime);
                }
                else if (playerCamera.transform.rotation.eulerAngles.y < 10 && hAxis != 1)
                {
                    playerCamera.transform.Rotate(Vector3.up * -1 * rotateSpeed * Time.deltaTime);
                }
            }
            else
            {
                if (playerCamera.transform.rotation.eulerAngles.y > 350 && hAxis != 1)
                {
                    playerCamera.transform.Rotate(Vector3.up * 1 * rotateSpeed * Time.deltaTime);
                }
                else if (playerCamera.transform.rotation.eulerAngles.y < 10 && hAxis != -1)
                {
                    playerCamera.transform.Rotate(Vector3.up * -1 * rotateSpeed * Time.deltaTime);
                }
            }



            amountToMove += playerCamera.transform.right * moveSpeed * hAxis;
            amountToMove.y += yVelocity;

            amountToMove *= Time.deltaTime;
            if (platform != null)
            {
                amountPlatformMoved = platform.transform.position - prevPos;
                amountToMove += amountPlatformMoved;
                prevPos = platform.transform.position;
            }
            cc.Move(amountToMove);
            amountToMove.y = 0;
            if (platform != null)
            {
                amountToMove -= amountPlatformMoved;

                if (amountToMove != Vector3.zero)
                {
                    playerModel.transform.rotation = Quaternion.Slerp(playerModel.transform.rotation, Quaternion.LookRotation(amountToMove), Time.deltaTime * playerRotationSpeed);
                }
            }
            else
            {
                if (amountToMove != Vector3.zero)
                {
                    playerModel.transform.rotation = Quaternion.Slerp(playerModel.transform.rotation, Quaternion.LookRotation(amountToMove), Time.deltaTime * playerRotationSpeed);
                }
            }
            moveSpeed = 8f;

        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Coin"))
        {
            Destroy(other.gameObject);
            goldenCoins += 1;
            coinsCollected.text = "Coins Collected: " + goldenCoins;
            if (goldenCoins == 5)
            {
                coinsCollected.text = " ";
                objective.text = "Objective: Collect the Star";
                star.SetActive(true);
            }
        }
        else if (other.CompareTag("Star"))
        {
            objective.text = "You won!";
            instructions.text = " ";
            notWon = false;
        }
        else if (other.CompareTag("platform"))
        {
            if (other.GetComponent<platformScript>() != null)
            {
                if (other.GetComponent<platformScript>().isAliive)
                {

                    platform = other.gameObject;
                    prevPos = platform.transform.position;
                }
            }
            else if (other.GetComponent<bluePlatform>() != null)
            {
                if (other.GetComponent<bluePlatform>().isAliive)
                {
                    platform = other.gameObject;
                    prevPos = platform.transform.position;
                }
            }


        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("platform"))
        {
            platform = null;
        }
    }
}
