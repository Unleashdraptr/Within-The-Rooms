using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    bool Jumping;

    public bool PauseState;

    public float RandomChance;

    public Transform GameSpawn;
    public GameObject ResetPoint;
    public bool Rise;

    public int XMoveChance;
    public int YMoveChance;
    public int ZMoveChance;
    public int XResetChance;
    public int ZResetChance;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
    }
    void Update()
    {
        if (PauseState == false)
        {
            // We are grounded, so recalculate move direction based on axes
            Vector3 forward = transform.TransformDirection(Vector3.forward);
            Vector3 right = transform.TransformDirection(Vector3.right);
            float curSpeedX = canMove ? walkingSpeed * Input.GetAxis("Vertical") : 0;
            float curSpeedY = canMove ? walkingSpeed * Input.GetAxis("Horizontal") : 0;
            float movementDirectionY = moveDirection.y;
            moveDirection = (forward * curSpeedX) + (right * curSpeedY);

            if (Input.GetButton("Jump") && canMove && characterController.isGrounded)
            {
                moveDirection.y = jumpSpeed;
                Jumping = true;
            }
            else
            {
                moveDirection.y = movementDirectionY;
            }
            if (characterController.isGrounded && Jumping == false)
            {
                moveDirection.y = 0;
            }

            if (characterController.isGrounded)
            {
                Jumping = false;
            }
            if (Rise == true)
            {
                moveDirection.y += 0.1f;
            }
            // Apply gravity. Gravity is multiplied by deltaTime twice (once here, and once below
            // when the moveDirection is multiplied by deltaTime). This is because gravity should be applied
            // as an acceleration (ms^-2)
            if (!characterController.isGrounded)
            {
                moveDirection.y -= gravity * Time.deltaTime;
            }

            int Resets = gameObject.GetComponent<ResetSetup>().Resets;
            XMoveChance = Random.Range(1, 30) + Resets;
            YMoveChance = Random.Range(1, 30) + Resets;
            ZMoveChance = Random.Range(1, 30) + Resets;

            XResetChance = Random.Range(1, 30) + Resets;
            ZResetChance = Random.Range(1, 30) + Resets;

            if (XMoveChance > 35)
            {
                Debug.Log("X move");
                float MoveAmount = Random.Range(-10.5f, 10.5f);
                moveDirection.x += MoveAmount;
            }
            if (YMoveChance > 44 && characterController.isGrounded)
            {
                moveDirection.y += jumpSpeed;
            }
            if (ZMoveChance > 35)
            {
                Debug.Log("Z move");
                float MoveAmount = Random.Range(-10.5f, 10.5f);
                moveDirection.z += MoveAmount;
            }
            if (XResetChance > 41)
            {
                Debug.Log("X Stop");
                moveDirection.x = 0;
            }
            if (ZResetChance > 41)
            {
                Debug.Log("Z Stop");
                moveDirection.z = 0;
            }

            // Move the controller
            characterController.Move(moveDirection * Time.deltaTime);

            // Player and Camera rotation
            if (canMove)
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
        RandomChance += 1 * Time.deltaTime;
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
            Rise = true;
        }
        if (collision.gameObject.tag == "ResetPoint")
        {
            if (gameObject.GetComponent<ResetSetup>().Resets < 15)
            {
                gameObject.GetComponent<ResetSetup>().Resets += 1;
                gameObject.GetComponent<ResetSetup>().UpdateBlocks();
            }
            else
                Debug.Log("Game end");
        }
        if(collision.gameObject.tag == "Death")
        {
            PauseState = true;
            GameObject.Find("UI").GetComponent<Buttons>().Death = true;
        }
        if(collision.gameObject.tag == "OverWorld")
        {
            GameObject.Find("UI").GetComponent<Buttons>().Death = true;
            StartCoroutine(ResetState());
        }
    }

}
