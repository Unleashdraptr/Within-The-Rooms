using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Movement : MonoBehaviour
{
    public float walkingSpeed = 7.5f;
    public float jumpSpeed = 8.0f;
    public float gravity = 20.0f;
    public Camera playerCamera;
    public float lookSpeed = 2.0f;
    public float lookXLimit = 45.0f;
    CharacterController characterController;
    public Vector3 moveDirection = Vector3.zero;
    float rotationX = 0;
    bool canMove = true;
    bool isJumping;

    public bool PauseState;

    public float RandomChance;

    public Transform GameSpawn;
    public GameObject ResetPoint;
    public bool Rise;

    public bool StuckZ;
    public bool NoX;
    public bool NoY;
    public bool NoZ;
    public bool NoCamMove;
    public float[] LevelChances = { 5, 10, 25, 35, 45};
    public float EffectCast;
    public float EffectLasting;
    public int RandomChoice;
    public float RNG;

    public AudioSource Walking;
    public AudioSource Jumping;
    string[] DeathMessages = { "Did you just die?", "Why did you go again?", "Why cant you move?", "Whats controlling this?", "Finally Freedom." };
    void Start()
    {
        characterController = GetComponent<CharacterController>();
        Cursor.visible = false;
    }
    void Update()
    {
        if (EffectCast >= 2 && GetComponent<ResetSetup>().Resets > 0)
        {
            RNG = Random.Range(0, 50);
            if (RNG < LevelChances[GetComponent<ResetSetup>().Resets -1] && StuckZ == false && NoX == false && NoY == false && NoZ == false && NoCamMove == false)
            {
                RandomChoice = Random.Range(1, 11);

                if (RandomChoice <= 2)
                {
                    StuckZ = true;
                }
                if (RandomChoice > 2 && RandomChoice <= 5)
                {
                    NoX = true;
                }
                if (RandomChoice > 5 && RandomChoice <= 7)
                {
                    NoY = true;
                }
                if (RandomChoice > 7 && RandomChoice <= 9)
                {
                    NoZ = true;
                }
                if (RandomChoice == 10)
                {
                    NoCamMove = true;
                }
                EffectLasting = 0;
            }
            EffectCast = 0;
        }
        if (EffectLasting >= 10)
        {
            StuckZ = false;
            NoX = false;
            NoY = false;
            NoZ = false;
            NoCamMove = false;
        }
        if (PauseState == false)
        {
            float movementDirectionY = 0f;
            if (StuckZ == true)
            {
                moveDirection.z = 10f;
            }
            else
            {
                // We are grounded, so recalculate move direction based on axes
                Vector3 forward = transform.TransformDirection(Vector3.forward);
                Vector3 right = transform.TransformDirection(Vector3.right);
                float curSpeedX = 0f;
                float curSpeedY = 0f;
                if (NoX == false)
                {
                    curSpeedX = canMove ? walkingSpeed * Input.GetAxis("Vertical") : 0;
                }
                if (NoZ == false)
                {
                    curSpeedY = canMove ? walkingSpeed * Input.GetAxis("Horizontal") : 0;
                }
                movementDirectionY = moveDirection.y;
                moveDirection = (forward * curSpeedX) + (right * curSpeedY);
            }

            if (Input.GetButton("Jump") && canMove && characterController.isGrounded && NoY == false)
            {
                moveDirection.y = jumpSpeed;
                isJumping = true;
                Walking.Stop();
                Jumping.Play();
            }
            else
            {
                moveDirection.y = movementDirectionY;
            }
            if (characterController.isGrounded && isJumping == false)
            {
                moveDirection.y = 0;
            }

            if (characterController.isGrounded)
            {
                isJumping = false;
            }
            if (Rise == true)
            {
                moveDirection.y += 0.5f;
            }
            // Apply gravity. Gravity is multiplied by deltaTime twice (once here, and once below
            // when the moveDirection is multiplied by deltaTime). This is because gravity should be applied
            // as an acceleration (ms^-2)
            if (!characterController.isGrounded)
            {
                moveDirection.y -= gravity * Time.deltaTime;
            }
            // Move the controller

            characterController.Move(moveDirection * Time.deltaTime);
            if (characterController.isGrounded && moveDirection.x == 0 || moveDirection.z == 0)
            {
                Walking.Play();
            }

            // Player and Camera rotation
            if (canMove && NoCamMove == false)
            {
                rotationX += -Input.GetAxis("Mouse Y") * lookSpeed;
                rotationX = Mathf.Clamp(rotationX, -lookXLimit, lookXLimit);
                playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
                transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * lookSpeed, 0);
            }
        }

        if (transform.position.y >= 200)
        {
            Rise = false;
            moveDirection.y = 0;
            Vector3 Spawn = GameSpawn.GetComponent<Transform>().position;
            transform.SetPositionAndRotation(Spawn, transform.rotation);
        }
        EffectCast += 1 * Time.deltaTime;
        EffectLasting += 1 * Time.deltaTime;
    }

    IEnumerator ResetState()
    {

        yield return new WaitForSeconds(5f);
        GameObject.Find("UI").GetComponent<Buttons>().Death = false;
        GameObject.Find("UI").GetComponent<Buttons>().ResetUI();
    }
    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "Spinners")
        {
            Debug.Log("Fly mode");
            Rise = true;
        }
        if (collision.gameObject.tag == "ResetPoint")
        {
            if (gameObject.GetComponent<ResetSetup>().Resets < 5)
            {
                gameObject.GetComponent<ResetSetup>().Resets += 1;
                gameObject.GetComponent<ResetSetup>().UpdateBlocks();
            }
            if (gameObject.GetComponent<ResetSetup>().Resets == 5)
            {
                Cursor.visible = true;
                SceneManager.LoadScene("End");        
            }
        }
        if(collision.gameObject.tag == "Death")
        {
            Cursor.visible = true;
            PauseState = true;
            GameObject.Find("UI").GetComponent<Buttons>().Death = true;
        }
        if(collision.gameObject.tag == "OverWorld")
        {
            Debug.Log("Fake Death");
            GameObject.Find("UI").GetComponent<Buttons>().Death = true;
            GameObject.Find("Death Text").GetComponent<Text>().text = DeathMessages[gameObject.GetComponent<ResetSetup>().Resets];
            StartCoroutine(ResetState());
        }
    }

}
