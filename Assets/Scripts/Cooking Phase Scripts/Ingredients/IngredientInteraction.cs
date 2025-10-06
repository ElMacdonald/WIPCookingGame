using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class IngredientInteraction : MonoBehaviour
{
    public List<GameObject> usableBins = new List<GameObject>();
    public bool canInteract;
    public bool canTrash;
    public bool canCook;
    public bool canBook;
    public bool canCut;
    public bool cutting;

    public int playerNum;
    private bool interactPressed;

    // Checks for input from user and verifies if they're able to interact with ingredient bins or trash
    // If they are, it will either trash the ingredient they're holding or deposit it into the closest bin
    // If there are multiple bins in range, it will deposit it into the closest one
    void Update()
    {
        if (playerNum == 1)
        {
            interactPressed = Input.GetAxisRaw("Interact_P1") > 0;
        }
        else
        {
            interactPressed = Input.GetAxisRaw("Interact_P2") > 0;
        }

        if (interactPressed && (canInteract || canTrash || canCook || canCut))
        {
            if (canCut && !cutting && gameObject.GetComponent<IngredientHolding>().ingredientCurrentlyHeld != null)
            {
                cutting = true;
                GameObject cuttingBoard;
                if (playerNum == 1)
                {
                    cuttingBoard = GameObject.Find("Cutting Board P1");
                }
                else
                {
                    cuttingBoard = GameObject.Find("Cutting Board P2");
                }

                if (cuttingBoard != null)
                {
                    CuttingBoardMicrogame cb = cuttingBoard.GetComponent<CuttingBoardMicrogame>();
                    if (cb != null)
                    {
                        cb.OpenMenu();
                    }
                }
                return;
            }
            if (canTrash)
            {
                gameObject.GetComponent<IngredientHolding>().trashIngredient();
            }
            else if (canCook)
            {
                GameObject cookingStation;
                if (playerNum == 1)
                {
                    cookingStation = GameObject.Find("Cooking Station P1");
                }
                else
                {
                    cookingStation = GameObject.Find("Cooking Station P2");
                }



                if (cookingStation != null)
                {
                    CookingStation cs = cookingStation.GetComponent<CookingStation>();
                    if (cs != null)
                    {
                        IngredientHolding ih = gameObject.GetComponent<IngredientHolding>();
                        if (ih != null && ih.ingredientCurrentlyHeld != null)
                        {
                            cs.AddIngredient(ih.ingredientCurrentlyHeld);
                            ih.trashIngredient();
                        }
                        cs.cookIngredients();
                    }
                }
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

        if (other.gameObject.tag == "Cutting Board")
        {
            canCut = true;
        }
        if (other.gameObject.tag == "Recipe Book")
        {
            canBook = true;
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

        if (other.gameObject.tag == "Cooking Station")
        {
            canCook = false;
        }
        if (other.gameObject.tag == "Cutting Board")
        {
            canCut = false;
        }
        if (other.gameObject.tag == "Recipe Book")
        {
            canBook = false;
        }
    }
}
