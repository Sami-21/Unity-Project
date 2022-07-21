using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class ThirdPersonMovement : MonoBehaviour
{

    public CharacterController characterController;
    public Animator animator;
    public Transform orientation;
    public Transform player;
    public Transform playerObj;
    public Transform Cam;
    public InputAction playerControls;
    public float velocity;
    private float smoothTurnTime = 5f;
    Vector3 direction = Vector3.zero;
    KeyCode block;



    void animationHandler(Vector3 direction)
    {
        bool isRunningForward = animator.GetBool("isRunningForward");
        bool isRunningBackward = animator.GetBool("isRunningBackward");
        bool isStrafeRight = animator.GetBool("isStrafeRight");
        bool isStrafeLeft = animator.GetBool("isStrafeLeft");
        bool isRunningRight = animator.GetBool("isRunningRight");
        bool isRunningLeft = animator.GetBool("isRunningLeft");
        bool isRunningBackwardRight = animator.GetBool("isRunningBackwardRight");
        bool isRunningBackwardLeft = animator.GetBool("isRunningBackwardLeft");

        //Forward Run
        if (direction.z > 0 && !isRunningForward)
        {
            animator.SetBool("isRunningForward",true);
        }
        if(direction.z == 0 && isRunningForward)
        {
            animator.SetBool("isRunningForward",false);
        }
        //Backward Run
        if (direction.z < 0 && !isRunningBackward)
        {
            animator.SetBool("isRunningBackward", true);
        }
        if (direction.z == 0 && isRunningBackward)
        {
            animator.SetBool("isRunningBackward", false);
        }
        //Left Strafe 
        if (direction.x < 0 && !isStrafeLeft)
        {
            animator.SetBool("isStrafeLeft", true);
        }
        if (direction.x == 0 && isStrafeLeft)
        {
            animator.SetBool("isStrafeLeft", false);
        }
        //Right Strafe 
        if (direction.x > 0 && !isStrafeRight)
        {
            animator.SetBool("isStrafeRight", true);
        }
        if (direction.x == 0 && isStrafeRight)
        {
            animator.SetBool("isStrafeRight", false);
        }
        //Running Left 
        if (direction.x < 0 && direction.z > 0 && !isRunningLeft)
        {
            animator.SetBool("isRunningLeft", true);
        }
        if ((direction.x == 0 || direction.z == 0) && isRunningLeft)
        {
            animator.SetBool("isRunningLeft", false);
        }
        //Running Right
        if (direction.x > 0 && direction.z > 0 && !isRunningRight)
        {
            animator.SetBool("isRunningRight", true);
        }
        if ((direction.x == 0 || direction.z == 0 ) && isRunningRight)
        {
            animator.SetBool("isRunningRight", false);
        }
        //Running Backward Left 
        if (direction.x < 0 && direction.z < 0 && !isRunningBackwardLeft)
        {
            animator.SetBool("isRunningBackwardLeft", true);
        }
        if ((direction.x == 0 || direction.z == 0) && isRunningBackwardLeft)
        {
            animator.SetBool("isRunningBackwardLeft", false);
        }
        //Running Backward Right
        if (direction.x > 0 && direction.z < 0 && !isRunningBackwardRight)
        {
            animator.SetBool("isRunningBackwardRight", true);
        }
        if ((direction.x == 0 || direction.z == 0) && isRunningBackwardRight)
        {
            animator.SetBool("isRunningBackwardRight", false);
        }

    }

    void PlayerMove(Vector3 movement , float speed)
    {
        if (speed > 0.1f)
        {
            if (movement != Vector3.zero)
            {
                characterController.Move(speed * Time.deltaTime * movement.normalized);
            }

        }
    }
    private void Awake()
    {
        animator = GetComponent < Animator >();
    }
    private void OnEnable()
    {
        playerControls.Enable();
    }
    private void OnDisable()
    {
        playerControls.Disable();
    }
    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    // Update is called once per frame
    void Update()
    {
        Vector3 viewDirection = player.position - new Vector3(Cam.position.x, player.position.y, Cam.position.z);
        orientation.forward = viewDirection.normalized;

        // Reading player input
        direction = playerControls.ReadValue<Vector3>();

        Vector3 movement = orientation.right * direction.x  + orientation.forward * direction.z;

        // Player rotaion when moving ( player always facing the view direction when moving )
        Vector3 inputDir = orientation.forward * Mathf.Abs(direction.z) + orientation.forward * Mathf.Abs(direction.x);

        // Player rotaion when moving ( player does not always face the view direction when moving )
        // Vector3 inputDir = orientation.forward * direction.z + orientation.right * direction.x;

        if (inputDir != Vector3.zero)
        {
            playerObj.forward = Vector3.Slerp(playerObj.forward, inputDir.normalized, Time.deltaTime * smoothTurnTime);
        }

        animationHandler(direction);
        // Player movement
        PlayerMove(movement, velocity);
    }

}
