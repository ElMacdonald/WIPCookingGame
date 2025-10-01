using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class IngredientInteraction : MonoBehaviour
{
    public List<GameObject> usableBins = new List<GameObject>();
    public bool canInteract;
    public bool canTrash;
    public bool canCook;

    // Checks for input from user and verifies if they're able to interact with ingredient bins or trash
    // If they are, it will either trash the ingredient they're holding or deposit it into the closest bin
    // If there are multiple bins in range, it will deposit it into the closest one
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && (canInteract || canTrash || canCook))
        {
            if (canTrash)
            {
                gameObject.GetComponent<IngredientHolding>().trashIngredient();
            }
            else
            {
                GameObject closestBin = GetClosestBin();
            if (closestBin != null)
            {
                IngredientBin ib = closestBin.GetComponent<IngredientBin>();
                if (ib != null)
                {
                    ib.giveIngredient(this.gameObject);
                }
            }
            }
        }
    }

    // Finds the closest ingredient bin to the player
    GameObject GetClosestBin()
    {
        GameObject closest = null;
        float closestDist = Mathf.Infinity;
        Vector3 currentPos = transform.position;

        foreach (GameObject bin in usableBins)
        {
            Vector3 directionToTarget = bin.transform.position - currentPos;
            float dSqrToTarget = directionToTarget.sqrMagnitude;
            if (dSqrToTarget < closestDist)
            {
                closestDist = dSqrToTarget;
                closest = bin;
            }
        }

        return closest;
    }


    // Used for detecting if the player is in range of an ingredient bin
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Ingredient")
        {
            if (!usableBins.Contains(other.gameObject))
                usableBins.Add(other.gameObject);

            if (usableBins.Count > 0)
            {
                canInteract = true;
            }
        }

        if (other.gameObject.tag == "Trash")
        {
            canTrash = true;
        }

        if (other.gameObject.tag == "Cooking Station")
        {
            canCook = true;
        }
    }

    // Used whenever player leaves the trigger area of bins and stations
    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Ingredient")
        {
            if (usableBins.Contains(other.gameObject))
                usableBins.Remove(other.gameObject);

            if (usableBins.Count == 0)
            {
                canInteract = false;
            }
        }

        if (other.gameObject.tag == "Trash")
        {
            canTrash = false;
        }
        
        if(other.gameObject.tag == "Cooking Station")
        {
            canCook = false;
        }
    }
}
