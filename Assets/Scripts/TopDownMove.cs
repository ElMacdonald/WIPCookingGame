using UnityEngine;
using UnityEngine.EventSystems;

public class TopDownMove : MonoBehaviour
{
    private float horizontal;
    private float vertical;
    private Vector3 move;
    public float speed;
    public float rotationSpeed;

    void Start()
    {
        
    }

    void Update()
    {
        vertical = Input.GetAxisRaw("Vertical");
        horizontal = Input.GetAxisRaw("Horizontal");
        move = new Vector3(horizontal, 0, vertical).normalized;

        transform.position += speed * Time.deltaTime * move;

        if (move != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(move);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }
}
