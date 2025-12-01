using UnityEngine;

public class GameplayManager : MonoBehaviour
{

    public ScreenFader p1Fader;
    public ScreenFader p2Fader;
    public GameObject p1CombatCam;
    public GameObject p2CombatCam;
    public GameObject p1CookingCam;
    public GameObject p2CookingCam;

    public GameObject p1CombatUI;
    public GameObject p2CombatUI;
    public GameObject p1CookingUI;
    public GameObject p2CookingUI;

    public GameObject p1Combat;
    public GameObject p2Combat;
    public GameObject p1Cooking;
    public GameObject p2Cooking;

    public TempWeaponGive tempWeaponGive;

    void Start()
    {
        SwitchToCookingPhase(1);
        SwitchToCookingPhase(2);
        //SwitchToCombatPhase(1);
        //SwitchToCombatPhase(2);
    }

    public void SwitchToCookingPhase(int playerNum)
{
    if (playerNum == 1)
    {
        StartCoroutine(p1Fader.FadeTransition(() =>
        {
            p1CombatCam.SetActive(false);
            p1CookingCam.SetActive(true);
            p1CombatUI.SetActive(false);
            p1CookingUI.SetActive(true);
            p1Combat.SetActive(false);
            p1Cooking.SetActive(true);
        }));
    }
    else
    {
        StartCoroutine(p2Fader.FadeTransition(() =>
        {
            p2CombatCam.SetActive(false);
            p2CookingCam.SetActive(true);
            p2CombatUI.SetActive(false);
            p2CookingUI.SetActive(true);
            p2Combat.SetActive(false);
            p2Cooking.SetActive(true);
        }));
    }
}


    public void SwitchToCombatPhase(int playerNum)
{
    if (playerNum == 1)
    {
        tempWeaponGive.player1Dish = GameObject.Find("Player1").GetComponent<IngredientHolding>().ingredientCurrentlyHeld.name;
        StartCoroutine(p1Fader.FadeTransition(() =>
        {
            p1CombatCam.SetActive(true);
            p1CookingCam.SetActive(false);
            p1CombatUI.SetActive(true);
            p1CookingUI.SetActive(false);
            p1Combat.SetActive(true);
            p1Cooking.SetActive(false);
            tempWeaponGive.giveWeapons();
        }));
    }
    else
    {
        tempWeaponGive.player2Dish = GameObject.Find("Player2").GetComponent<IngredientHolding>().ingredientCurrentlyHeld.name;
        StartCoroutine(p2Fader.FadeTransition(() =>
        {
            p2CombatCam.SetActive(true);
            p2CookingCam.SetActive(false);
            p2CombatUI.SetActive(true);
            p2CookingUI.SetActive(false);
            p2Combat.SetActive(true);
            p2Cooking.SetActive(false);
            tempWeaponGive.giveWeapons();
        }));
    }
}

}
