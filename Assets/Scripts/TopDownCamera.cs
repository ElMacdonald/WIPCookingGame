using UnityEngine;
using UnityEngine.EventSystems;

public class TopDownCamera : MonoBehaviour
{
    public Transform player;
    public float followSpeed = 2.0f;     
    public float deadZoneAngle = 5.0f;   

    void Update()
    {

        Vector3 playerDir = player.position - transform.position;
        Quaternion target = Quaternion.LookRotation(playerDir);


        float angleDiff = Quaternion.Angle(transform.rotation, target);
        if (angleDiff > deadZoneAngle)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, target, followSpeed * Time.deltaTime);
        }
    }
}
