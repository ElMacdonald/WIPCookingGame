using UnityEngine;
using System.Collections;

public class IngredientHolding : MonoBehaviour
{
    public Ingredient ingredientCurrentlyHeld;
    public string ingredientName;
    public bool isHeld;
    public GameObject[] followables;



    void Start()
    {
        makeFollowable("invisible spaghetti");
        isHeld = false;
    }

    public void makeFollowable(string name)
    {
        foreach (GameObject g in followables)
        {
            if (g.name == "Following " + name)
            {
                g.SetActive(true);
            }
            else
            {
                g.SetActive(false);
            }
        }
    }

    public void trashIngredient()
    {
        ingredientCurrentlyHeld = null;
        isHeld = false;
        makeFollowable("invisible spaghetti");
    }

    void Update()
    {
        ingredientName = ingredientCurrentlyHeld != null ? ingredientCurrentlyHeld.name : "";
    }
}
