using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class IngredientInteraction : MonoBehaviour
{
    public List<GameObject> usableBins = new List<GameObject>();
    public bool canInteract;
    public bool canTrash;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && (canInteract || canTrash))
        {
            if (canTrash)
            {
                gameObject.GetComponent<IngredientHolding>().trashIngredient();
            }
            else
            {
                GameObject closestBin = getClosestBin();
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

    GameObject getClosestBin()
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
    }

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
    }
}
