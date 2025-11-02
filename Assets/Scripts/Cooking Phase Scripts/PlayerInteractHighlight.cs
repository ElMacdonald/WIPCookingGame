using UnityEngine;

public class PlayerInteractHighlight : MonoBehaviour
{
    public float interactDistance = 2f;
    private Interactable currentTarget;

    void Update()
    {
        // Cast a ray from slightly above player's position forward
        Vector3 origin = transform.position + Vector3.up * 1f;
        Vector3 direction = transform.forward;

        RaycastHit hit;
        Debug.DrawRay(origin, direction * interactDistance, Color.green);
        if (Physics.Raycast(origin, direction, out hit, interactDistance))
        {
            Interactable interact = hit.collider.GetComponentInParent<Interactable>();

            if (interact != null)
            {
                if (currentTarget != interact)
                {
                    ClearHighlight();
                    currentTarget = interact;
                    currentTarget.Highlight();
                    Debug.Log($"Highlighting {currentTarget.name}");
                    
                }
                return;
            }
        }

        ClearHighlight();
    }

    void ClearHighlight()
    {
        if (currentTarget != null)
        {
            currentTarget.RemoveHighlight();
            currentTarget = null;
        }
    }
}
