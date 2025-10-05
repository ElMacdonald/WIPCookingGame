using UnityEngine;


// This script places the ingredient that the player is holding in front of them, following their movements

public class IngredientFollow : MonoBehaviour
{
    public Transform player;
    public IngredientHolding ih;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //player = GameObject.FindGameObjectWithTag("Player").transform;
        ih = player.GetComponent<IngredientHolding>();
    }

    // Update is called once per frame
    void Update()
    {
        if (ih.isHeld && ih.ingredientCurrentlyHeld != null)
        {
            this.transform.position = player.position + player.forward + new Vector3(0, -0.2f, 0);
            this.transform.rotation = player.rotation;
        }
    }
}
