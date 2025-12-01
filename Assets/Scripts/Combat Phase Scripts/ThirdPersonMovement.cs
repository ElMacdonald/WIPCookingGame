using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class ThirdPersonMovement : MonoBehaviour
{
    [Header("Movement")]
    public float walkSpeed = 6f; //sprinting is implemented but not used in this game
    public float sprintSpeed = 6f;
    public float rotationSmoothTime = 0.1f;
    public float airControlPercent = 0.6f;

    [Header("Acceleration")]
    public float groundAccel = 20f;
    public float airAccel = 10f;

    [Header("Jump / Gravity")]
    public float gravity = -9.81f;
    public float jumpHeight = 1.6f;
    public float terminalVelocity = -50f;

    [Header("Camera")]
    public Transform cameraTransform; // if null, will use Camera.main

    private CharacterController cc;
    private float verticalVelocity = 0f;
    private float currentSpeed;
    private float rotationVelocity;


    private float inputX;
    private float inputZ;

    private bool jumping;
    public int playerNum;

    void Start()
    {
        cc = GetComponent<CharacterController>();
        if (cameraTransform == null && Camera.main != null)
            cameraTransform = Camera.main.transform;
    }

    void Update()
    {
        HandleMovement();
    }

    public void MovePlayer(float horizontalInput, float verticalInput)
    {
        inputX = horizontalInput;
        inputZ = verticalInput;
    }

    public void TakeInput(bool jumpInput)
    {
        jumping = jumpInput;
    }


    private void HandleMovement()
    {
        //Read input (old Input Manager)
        /*
        if (playerNum == 1)
        {
            inputX = Input.GetAxis("Horizontal_P1");
            inputZ = -Input.GetAxis("Vertical_P1");
            jumping = Input.GetButtonDown("Jump_P1");
        }
        else
        {
            inputX = Input.GetAxis("Horizontal_P2");
            inputZ = -Input.GetAxis("Vertical_P2");
            jumping = Input.GetButtonDown("Jump_P2");
        }
        */
        
        Vector3 inputDir = new Vector3(inputX, 0f, inputZ);
        float inputMagnitude = Mathf.Clamp01(inputDir.magnitude);
        inputDir = inputDir.normalized;

        //Determine camera relative directions (projected to horizontal plane)
        Vector3 camForward = cameraTransform != null ? cameraTransform.forward : transform.forward;
        camForward.y = 0f;
        camForward.Normalize();
        Vector3 camRight = cameraTransform != null ? cameraTransform.right : transform.right;
        camRight.y = 0f;
        camRight.Normalize();

        //Movement direction relative to camera
        Vector3 moveDir = camForward * inputDir.z + camRight * inputDir.x;
        if (moveDir.sqrMagnitude > 1f) moveDir.Normalize();

        // Determine base speed (sprint with left/right shift)
        bool sprint = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);
        float baseTargetSpeed = sprint ? sprintSpeed : walkSpeed;

        //Apply air control modifier to desired speed, use proper accel / interpolation so air movement still works
        float speedModifier = cc.isGrounded ? 1f : airControlPercent;
        float desiredSpeed = baseTargetSpeed * inputMagnitude * speedModifier;
        float accel = cc.isGrounded ? groundAccel : airAccel;

        //Smooth change of speed (MoveTowards keeps responsiveness in air)
        currentSpeed = Mathf.MoveTowards(currentSpeed, desiredSpeed, accel * Time.deltaTime);

        // Face the same direction the camera is facing (projected to horizontal)
        Vector3 camFacing = cameraTransform != null ? cameraTransform.forward : transform.forward;
        camFacing.y = 0f;
        if (camFacing.sqrMagnitude > 0.001f)
        {
            float targetAngle = Mathf.Atan2(camFacing.x, camFacing.z) * Mathf.Rad2Deg;
            float smoothed = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref rotationVelocity, rotationSmoothTime);
            transform.rotation = Quaternion.Euler(0f, smoothed, 0f);
        }

        //Gravity and Jump
        if (cc.isGrounded)
        {
            if (verticalVelocity < 0f)
                verticalVelocity = -2f; // small downward force to keep grounded

            if (jumping)
            {
                verticalVelocity = Mathf.Sqrt(jumpHeight * -2f * gravity);
            }
        }
        else
        {
            verticalVelocity += gravity * Time.deltaTime;
            if (verticalVelocity < terminalVelocity) verticalVelocity = terminalVelocity;
        }

        //finalized movement
        Vector3 horizontalVelocity = moveDir * currentSpeed;
        Vector3 velocity = horizontalVelocity + Vector3.up * verticalVelocity;
        cc.Move(velocity * Time.deltaTime);
        jumping = false;
    }
}
