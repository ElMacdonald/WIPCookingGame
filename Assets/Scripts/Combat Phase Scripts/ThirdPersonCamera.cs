using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour
{
    public Transform target;
    public float distance = 3.0f;
    public float height = 1.5f;
    public float heightDamping = 5.0f;
    public float rotationDamping = 10.0f;
    public float shoulderOffset = 0.5f;
    public Vector3 lookOffset = Vector3.up * 1.2f;

    // Mouse control settings (old Input Manager)
    public float mouseSensitivityX = 3f;
    public float mouseSensitivityY = 2f;
    public bool invertY = false;
    public float minPitch = -35f;
    public float maxPitch = 60f;

    // Scroll zoom
    public float scrollSensitivity = 2f;
    public float minDistance = 1.5f;
    public float maxDistance = 6f;

    private Transform camTransform;
    private float yaw;
    private float pitch;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;

        camTransform = transform;
        if (target == null)
        {
            var player = GameObject.FindWithTag("Player");
            if (player != null) target = player.transform;
        }

        //initialize yaw/pitch from current camera orientation
        Vector3 angles = camTransform.eulerAngles;
        yaw = angles.y;
        pitch = angles.x;
    }

    void LateUpdate()
    {
        if (target == null) return;

        //Mouse input using Unity's old input manager
        yaw += Input.GetAxis("Mouse X") * mouseSensitivityX;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivityY;
        pitch += (invertY ? mouseY : -mouseY);
        pitch = Mathf.Clamp(pitch, minPitch, maxPitch);

        //Zoom with scroll wheel (not used for this game, but left for testing)
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (Mathf.Abs(scroll) > 0.0001f)
        {
            distance = Mathf.Clamp(distance - scroll * scrollSensitivity, minDistance, maxDistance);
        }

        //Build rotation from yaw/pitch
        Quaternion camRotation = Quaternion.Euler(pitch, yaw, 0f);

        //Desired camera position: behind the target with over-the-shoulder offset
        Vector3 desiredPosition = target.position
                                  - camRotation * Vector3.forward * distance
                                  + Vector3.up * height
                                  + camRotation * Vector3.right * shoulderOffset;

        //Smoothly move the camera to the desired position.
        camTransform.position = Vector3.Lerp(camTransform.position, desiredPosition, Time.deltaTime * heightDamping);

        //Look at the target (with look offset) and smooth rotation
        Vector3 lookPoint = target.position + lookOffset;
        Quaternion desiredRotation = Quaternion.LookRotation(lookPoint - camTransform.position);
        camTransform.rotation = Quaternion.Slerp(camTransform.rotation, desiredRotation, Time.deltaTime * rotationDamping);
    }
}
