using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour
{
    [Header("Target")]
    public Transform target;

    [Header("Camera Settings")]
    public float distance = 3.0f;
    public float height = 1.5f;
    public float shoulderOffset = 0.5f;
    public Vector3 lookOffset = Vector3.up * 1.2f;

    [Header("Smoothing")]
    public float positionSmooth = 12f;
    public float rotationSmooth = 16f;

    [Header("Look Sensitivity")]
    public float mouseSensitivityX = 3f;
    public float mouseSensitivityY = 2f;
    public bool invertY = false;

    public float minPitch = -35f;
    public float maxPitch = 60f;

    [Header("Zoom (optional)")]
    public float scrollSensitivity = 2f;
    public float minDistance = 1.5f;
    public float maxDistance = 6f;

    private Transform camTransform;
    private float yaw;
    private float pitch;

    private float xAxis;
    private float yAxis;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;

        camTransform = transform;

        if (target == null)
        {
            var player = GameObject.FindWithTag("Player");
            if (player != null)
                target = player.transform;
        }

        // Initialize yaw/pitch from current camera orientation
        Vector3 angles = camTransform.eulerAngles;
        yaw = angles.y;
        pitch = angles.x;
    }

    // Called externally by input system
    public void MoveCamera(float xInput, float yInput)
    {
        xAxis = xInput;
        yAxis = yInput;
    }

    void LateUpdate()
    {
        if (!target)
            return;

        // ----- ROTATION INPUT -----
        yaw += xAxis * mouseSensitivityX;

        float mouseY = yAxis * mouseSensitivityY;
        pitch += invertY ? -mouseY : mouseY;
        pitch = Mathf.Clamp(pitch, minPitch, maxPitch);

        // Rotation from yaw & pitch ONLY (no LookAt fighting)
        Quaternion targetRot = Quaternion.Euler(pitch, yaw, 0f);

        // ----- ZOOM (optional) -----
        distance = Mathf.Clamp(distance, minDistance, maxDistance);

        // ----- CAMERA POSITION -----
        Vector3 desiredPos =
            target.position
            - targetRot * Vector3.forward * distance
            + Vector3.up * height
            + targetRot * Vector3.right * shoulderOffset;

        camTransform.position = Vector3.Lerp(
            camTransform.position,
            desiredPos,
            Time.deltaTime * positionSmooth
        );

        // ----- CAMERA ROTATION -----
        camTransform.rotation = Quaternion.Slerp(
            camTransform.rotation,
            targetRot,
            Time.deltaTime * rotationSmooth
        );

        // (Optional) Look offset applied after rotation:
        camTransform.LookAt(target.position + lookOffset);
    }
}
