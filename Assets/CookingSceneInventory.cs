using UnityEngine;
using UnityEngine.UI;

public class CookingSceneInventory : MonoBehaviour
{
    public Image ingredientImage;
    public IngredientHolding ih;
    public Sprite[] ingredientSprites;
    public int playerNum;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ingredientImage = GetComponent<Image>();
        if(playerNum == 1)
        {
            ih = GameObject.Find("Player1").GetComponent<IngredientHolding>();
        }
        else
        {
            ih = GameObject.Find("Player2").GetComponent<IngredientHolding>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        switch(ih.ingredientName)
        {
            case "Shrimp":
                ingredientImage.sprite = ingredientSprites[0];
                break;
            case "Scrap Metal":
                ingredientImage.sprite = ingredientSprites[1];
                break;
            case "Salmon":
                ingredientImage.sprite = ingredientSprites[2];
                break;
            case "Bamboo":
                ingredientImage.sprite = ingredientSprites[3];
                break;
            case "Onion":
                ingredientImage.sprite = ingredientSprites[4];
                break;
            default:
                ingredientImage.sprite = null;
                break;
        }
    }
}
