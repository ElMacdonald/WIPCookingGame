using UnityEngine;

public class TopDownCamera : MonoBehaviour
{
    public Transform player;

    [Header("Follow Settings")]
    public float followSpeed = 5.0f;     // overall smoothing
    public float deadZoneAngle = 5.0f;   // rotation deadzone

    [Header("Z-Axis Follow Buffer")]
    public float zFollowDistance = 3f;   // how far player can move away before camera follows
    public float zReturnSpeed = 7f;      // speed camera moves back toward player

    private Vector3 offset;               // initial offset from player

    void Start()
    {
        offset = transform.position - player.position;
    }

    void LateUpdate()
    {
        Vector3 targetPos = player.position + offset;

        // x follow
        targetPos.x = Mathf.Lerp(transform.position.x, targetPos.x, followSpeed * Time.deltaTime);

        // z follow
        float zDiff = targetPos.z - transform.position.z;

        if (zDiff > zFollowDistance)
        {
            targetPos.z = Mathf.Lerp(transform.position.z, targetPos.z - zFollowDistance, followSpeed * Time.deltaTime);
        }
        else if (zDiff < -zFollowDistance)
        {
            targetPos.z = Mathf.Lerp(transform.position.z, targetPos.z + zFollowDistance, zReturnSpeed * Time.deltaTime);
        }
        else
        {
            targetPos.z = Mathf.Lerp(transform.position.z, targetPos.z, followSpeed * 0.5f * Time.deltaTime);
        }

        // y axis
        targetPos.y = player.position.y + offset.y;

        transform.position = targetPos;

        // rotation
        Vector3 lookDir = player.position - transform.position;
        Quaternion targetRot = Quaternion.LookRotation(lookDir);
        float angleDiff = Quaternion.Angle(transform.rotation, targetRot);

        if (angleDiff > deadZoneAngle)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, followSpeed * Time.deltaTime);
        }
    }
}
