using UnityEngine;

public class IngredientBin : MonoBehaviour
{
    public string[] qualities = { "Fresh", "Normal", "Stale" };
    public string ingredientName;
    public Ingredient ingredientType;
    public float ingredientCooldown;
    private float cdTimer;

    public GameObject ingredientModel;
    public bool usable;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ingredientType = new Ingredient(ingredientName, "Normal", ingredientModel);
        cdTimer = 0;
        usable = true;
    }


    void Update()
    {
        if (cdTimer > 0)
        {
            cdTimer -= Time.deltaTime;
            usable = false;
        }
        else
        {
            usable = true;
        }
    }

    void updateQuality()
    {
        int randIndex = Random.Range(0, qualities.Length);
        ingredientType.quality = qualities[randIndex];
    }

    public void giveIngredient(GameObject player)
    {
        IngredientHolding ih = player.GetComponent<IngredientHolding>();
        if (ih != null && ih.ingredientCurrentlyHeld == null && usable)
        {
            updateQuality();
            ih.ingredientCurrentlyHeld = new Ingredient(ingredientType.name, ingredientType.quality, ingredientType.ingredientPrefab);
            ih.isHeld = true;
            ih.ingredientName = ingredientType.name;
            Debug.Log("Gave " + ingredientType.quality + " " + ingredientType.name + " to player");
            ih.makeFollowable(ingredientType.name);
            cdTimer = ingredientCooldown;
        }
    }
}
