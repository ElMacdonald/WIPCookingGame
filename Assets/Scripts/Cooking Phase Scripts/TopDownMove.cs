using UnityEngine;
using UnityEngine.EventSystems;

public class TopDownMove : MonoBehaviour
{
    private float horizontal;
    private float vertical;
    private Vector3 move;
    public float speed;
    public float rotationSpeed;

    public int playerNum;

    void FixedUpdate()
    {
        //gets input
        if (playerNum == 1)
        {
            vertical = -Input.GetAxis("Vertical_P1");
            horizontal = Input.GetAxis("Horizontal_P1");
        }
        else
        {
            vertical = -Input.GetAxis("Vertical_P2");
            horizontal = Input.GetAxis("Horizontal_P2");
        }

        //creates movement vector
        move = new Vector3(horizontal, 0, vertical).normalized;

        //applies vector
        transform.position += speed * Time.deltaTime * move;

        //rotates player to face move direction
        if (move != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(move);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }
}
