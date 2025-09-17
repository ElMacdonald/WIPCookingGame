using UnityEngine;
using UnityEngine.EventSystems;

public class TopDownCamera : MonoBehaviour
{
    public Transform player;
    public float followSpeed = 2.0f; //speed the camera follows    
    public float deadZoneAngle = 5.0f; //deadzone the camera sits at

    void Update()
    {
        //Gets player direction and rotation to look at
        Vector3 playerDir = player.position - transform.position;
        Quaternion target = Quaternion.LookRotation(playerDir);


        //Finds a sweet spot to slerp to giving a natural camera sway
        float angleDiff = Quaternion.Angle(transform.rotation, target);
        if (angleDiff > deadZoneAngle)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, target, followSpeed * Time.deltaTime);
        }
    }
}
