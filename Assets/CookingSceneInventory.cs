using UnityEngine;
using UnityEngine.UI;

public class CookingSceneInventory : MonoBehaviour
{
    public Image ingredientImage;
    public IngredientHolding ih;
    public Sprite[] ingredientSprites;
    public GameObject cutSprite;
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

        cutSprite = transform.parent.Find("Cut Sprite").gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        string name = ih.ingredientName.ToLower();

        ingredientImage.color = new Color(1, 1, 1, 1);
        if (name.Contains("shrimp pistol"))
            ingredientImage.sprite = ingredientSprites[5];
        else if (name.Contains("scrap metal"))
            ingredientImage.sprite = ingredientSprites[1];
        else if (name.Contains("salmon"))
            ingredientImage.sprite = ingredientSprites[2];
        else if (name.Contains("bamboomstick"))
            ingredientImage.sprite = ingredientSprites[6];
        else if (name.Contains("onion"))
            ingredientImage.sprite = ingredientSprites[4];
        else if (name.Contains("shrimp"))
            ingredientImage.sprite = ingredientSprites[0];
        else if (name.Contains("bamboo"))
            ingredientImage.sprite = ingredientSprites[3];
        else //makes it transparent
            ingredientImage.color = new Color(1, 1, 1, 0);

        if(name.Contains("cut"))
        {
            cutSprite.SetActive(true);
        }else
        {
            cutSprite.SetActive(false);
        }
    }

}
