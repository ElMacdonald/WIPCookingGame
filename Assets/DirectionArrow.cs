using UnityEngine;

public class DirectionArrow : MonoBehaviour
{
    public Transform player;
    public Transform target;
    public GameObject arrowModel;
    public GameObject[] targets;

    public float radius = 2f;     // How far from player the arrow hovers
    public float height = 1f;     // Hover height
    public float hideRange = 2f;  // Range to hide the arrow
    public IngredientHolding ih;

    void Update()
    {
        SetTarget();
        if (!player || !target) return;

        float distance = Vector3.Distance(player.position, target.position);

        // Hide when close
        arrowModel.SetActive(distance > hideRange);
        if (!arrowModel.activeSelf) return;

        // ---- GET DIRECTION FROM PLAYER TO TARGET ----
        Vector3 dir = (target.position - player.position).normalized;

        // ---- POSITION AROUND PLAYER BASED ON DIRECTION ----
        Vector3 newPos = player.position + dir * radius;
        newPos.y = player.position.y + height;
        transform.position = newPos;

        // ---- ROTATE ARROW TO FACE THE TARGET ----
        dir.y = 0;
        transform.rotation = Quaternion.LookRotation(dir);
        
    }

    void SetTarget()
    {
        string ingredientName = ih.ingredientName.ToLower();
        // Default to first target
        target = targets[0].transform;
        // Check for specific ingredients to set target
        if(ih.ingredientCurrentlyHeld != null)
        {
            if(ih.ingredientCurrentlyHeld.quality == "Dish")
            {
                target = targets[3].transform;
                return;
            }
        }
        if ((ingredientName.Contains("bamboo") || ingredientName.Contains("salmon") || ingredientName.Contains("onion") || ingredientName.Contains("shrimp")) && !ingredientName.Contains("cut"))
        {
            target = targets[2].transform;
            return;
        }
        if(ingredientName.Contains("cut") || ih.ingredientCurrentlyHeld != null)
        {
            target = targets[1].transform;
            return;
        }

        
    }
}
