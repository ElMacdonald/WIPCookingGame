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
                cooking = false;
                foreach (GameObject go in createdIngredients)
                {
                    Destroy(go);
                }
                ingredientsInStation.Clear();
            }
        }
    }
}
