using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class ThirdPersonMovement : MonoBehaviour
{
    [Header("Movement")]
    public float walkSpeed = 6f;
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
    public Transform cameraTransform;

    [Header("Dash Settings")]
    public float dashSpeed = 20f;
    public float dashDuration = 0.15f;
    public float dashCooldown = 2f;

    // Speed curve
    public AnimationCurve dashCurve = AnimationCurve.EaseInOut(0, 1, 1, 0);


    public float lastDashTime = -999f;
    private bool isDashing = false;
    private float dashTimer = 0f;
    private Vector3 dashDirection;

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
        HandleDash();
    }

    public void MovePlayer(float horizontalInput, float verticalInput)
    {
        inputX = horizontalInput;
        inputZ = verticalInput;
    }

    public void TakeInput(string input)
    {
        if (input == "A")
            jumping = true;

        if (input == "B")
            TryDash();   
    }


    private void TryDash()
    {
        if (Time.time < lastDashTime + dashCooldown)
            return;

        Vector3 inputDir = new Vector3(inputX, 0f, inputZ);
        if (inputDir.sqrMagnitude < 0.1f)
            return;

        Vector3 camForward = cameraTransform.forward;
        camForward.y = 0f;
        camForward.Normalize();

        Vector3 camRight = cameraTransform.right;
        camRight.y = 0f;
        camRight.Normalize();

        dashDirection = (camForward * inputDir.z + camRight * inputDir.x).normalized;

        dashTimer = 0f;          // reset timer
        isDashing = true;
        lastDashTime = Time.time;
    }



    private void HandleDash()
    {
        if (!isDashing) return;

        dashTimer += Time.deltaTime;
        float t = dashTimer / dashDuration;

        // End dash
        if (t >= 1f)
        {
            isDashing = false;
            return;
        }

        // Curve-based speed (1 → 0)
        float curveValue = dashCurve.Evaluate(t);
        float currentDashSpeed = dashSpeed * curveValue;

        // Still apply gravity while dashing
        verticalVelocity += gravity * Time.deltaTime;
        if (verticalVelocity < terminalVelocity)
            verticalVelocity = terminalVelocity;

        Vector3 dashVel = dashDirection * currentDashSpeed + Vector3.up * verticalVelocity;

        cc.Move(dashVel * Time.deltaTime);
    }



    private void HandleMovement()
    {
        // Skip normal movement during dash
        if (isDashing) return;

        Vector3 inputDir = new Vector3(inputX, 0f, inputZ);
        float inputMagnitude = Mathf.Clamp01(inputDir.magnitude);
        inputDir = inputDir.normalized;

        Vector3 camForward = cameraTransform != null ? cameraTransform.forward : transform.forward;
        camForward.y = 0f;
        camForward.Normalize();

        Vector3 camRight = cameraTransform != null ? cameraTransform.right : transform.right;
        camRight.y = 0f;
        camRight.Normalize();

        Vector3 moveDir = camForward * inputDir.z + camRight * inputDir.x;
        if (moveDir.sqrMagnitude > 1f) moveDir.Normalize();

        bool sprint = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);
        float baseTargetSpeed = sprint ? sprintSpeed : walkSpeed;

        float speedModifier = cc.isGrounded ? 1f : airControlPercent;
        float desiredSpeed = baseTargetSpeed * inputMagnitude * speedModifier;
        float accel = cc.isGrounded ? groundAccel : airAccel;

        currentSpeed = Mathf.MoveTowards(currentSpeed, desiredSpeed, accel * Time.deltaTime);

        Vector3 camFacing = cameraTransform != null ? cameraTransform.forward : transform.forward;
        camFacing.y = 0f;
        if (camFacing.sqrMagnitude > 0.001f)
        {
            float targetAngle = Mathf.Atan2(camFacing.x, camFacing.z) * Mathf.Rad2Deg;
            float smoothed = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref rotationVelocity, rotationSmoothTime);
            transform.rotation = Quaternion.Euler(0f, smoothed, 0f);
        }

        if (cc.isGrounded)
        {
            if (verticalVelocity < 0f)
                verticalVelocity = -2f;

            if (jumping)
                verticalVelocity = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }
        else
        {
            verticalVelocity += gravity * Time.deltaTime;
            if (verticalVelocity < terminalVelocity)
                verticalVelocity = terminalVelocity;
        }

        Vector3 horizontalVelocity = moveDir * currentSpeed;
        Vector3 velocity = horizontalVelocity + Vector3.up * verticalVelocity;

        cc.Move(velocity * Time.deltaTime);

        jumping = false;
    }
}
