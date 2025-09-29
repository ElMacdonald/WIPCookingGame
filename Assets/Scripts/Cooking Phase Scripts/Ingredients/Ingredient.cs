using UnityEngine;
using System.Collections;

public class Ingredient
{
    public string name;
    public string quality;
    public GameObject ingredientPrefab;

    public Ingredient(string name, string quality)
    {
        this.name = name;
        this.quality = quality;
    }



}
