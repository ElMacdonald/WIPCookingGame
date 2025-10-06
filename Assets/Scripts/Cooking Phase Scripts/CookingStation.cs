using UnityEngine;
using System.Collections.Generic;

public class CookingStation : MonoBehaviour
{
    public List<Ingredient> ingredientsInStation = new List<Ingredient>();


    public int maxIngredients;
    public Transform[] ingredientPositions;

    public GameObject[] createdIngredients;

    public IngredientInteraction ie;

    public float cookTime;
    public float cookTimer;
    public bool cooking;

    public GameObject shrimpGunPrefab;
    public GameObject bamboomstickPrefab;
    public WinCondition wc;

    public void AddIngredient(Ingredient ingredient)
    {
        if (ingredientsInStation.Count < maxIngredients)
        {
            ingredientsInStation.Add(ingredient);
            GameObject ingredientObj = Instantiate(ingredient.ingredientPrefab, ingredientPositions[ingredientsInStation.Count - 1].position, Quaternion.identity);
            ingredientObj.transform.parent = this.transform;
            createdIngredients[ingredientsInStation.Count - 1] = ingredientObj;
        }
    }

    public void cookIngredients()
    {
        if (maxIngredients == ingredientsInStation.Count && !cooking)
        {
            cooking = true;
            cookTimer = cookTime;
            ie.canCook = false;
        }
    }

    void FixedUpdate()
    {
        if (cooking)
        {
            cookTimer -= Time.deltaTime;
            if (cookTimer <= 0)
            {
                checkIfCorrectRecipe();
                cooking = false;
                foreach (GameObject go in createdIngredients)
                {
                    Destroy(go);
                }
                ingredientsInStation.Clear();

            }
        }
    }

    // Checks if the ingredients in the station match one of the two valid recipes
    public void checkIfCorrectRecipe()
    {
        if (ingredientsInStation.Count == 0) return;

        // Extract ingredient names
        List<string> names = new List<string>();
        foreach (Ingredient ing in ingredientsInStation)
        {
            names.Add(ing.name);
        }

        // Define the two valid recipes
        string[] recipe1 = { "Cut Salmon", "Scrap Metal", "Cut Bamboo" };
        string[] recipe2 = { "Cut Shrimp", "Scrap Metal", "Cut Bamboo" };

        // Check for matches (ignoring order)
        bool matchRecipe1 = true;
        bool matchRecipe2 = true;

        foreach (string req in recipe1)
        {
            if (!names.Contains(req))
            {
                matchRecipe1 = false;
                break;
            }
        }

        foreach (string req in recipe2)
        {
            if (!names.Contains(req))
            {
                matchRecipe2 = false;
                break;
            }
        }

        if (matchRecipe1)
        {
            Debug.Log("Correct Recipe: Bamboomstick!");
            // TODO: Instantiate salmon dish prefab here
            Instantiate(bamboomstickPrefab, this.transform.position + new Vector3(0, 1, 0), Quaternion.identity * Quaternion.Euler(0, 90, 45));
            wc.declareWinner(ie.playerNum);
        }
        else if (matchRecipe2)
        {
            Debug.Log("Correct Recipe: Shrimp Pistol!");
            // TODO: Instantiate shrimp dish prefab here
            Instantiate(shrimpGunPrefab, this.transform.position + new Vector3(0, 1, 0), Quaternion.identity * Quaternion.Euler(0, 90, 45));
            wc.declareWinner(ie.playerNum);
        }
        else
        {
            Debug.Log("Wrong recipe! Ingredients trashed.");
            // TODO: Play fail sound or spawn trash particles
        }
    }

}
