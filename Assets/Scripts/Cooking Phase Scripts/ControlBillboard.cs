using UnityEngine;

public class ControlBillboard : MonoBehaviour
{
    public Transform cam;
    public Transform target;
    public Transform player;
    public IngredientInteraction ie;

    public int playerNum;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        target = this.transform;
        //cam = Camera.main.transform;
        //player = GameObject.FindGameObjectWithTag("Player").transform;
        //ie = player.GetComponent<IngredientInteraction>();
    }

    // Update is called once per frame
    void Update()
    {
        target.position = player.position + new Vector3(0, 2, 0);
        target.LookAt(target.position + cam.forward);
        if(ie.canInteract || ie.canTrash || ie.canCook)
        {
            gameObject.GetComponent<SpriteRenderer>().enabled = true;
        }
        else
        {
            gameObject.GetComponent<SpriteRenderer>().enabled = false;
        }
    }
}
