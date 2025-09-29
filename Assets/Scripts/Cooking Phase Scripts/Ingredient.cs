using UnityEngine;
using System.Collections;

public class Ingredient
{
    public string name;
    public int quality;
    public GameObject ingredientPrefab;

    public Ingredient(string name, int quality)
    {
        this.name = name;
        this.quality = quality;
    }



}
