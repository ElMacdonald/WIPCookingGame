using UnityEngine;
using UnityEngine.UI;

public class TempWeaponGive : MonoBehaviour
{
    [Header("Dish Names (match part of child name)")]
    public string player1Dish; // e.g. "Shrimp pistol" or "Bamboomstick"
    public string player2Dish;
    public AmmoCounter amcountP1;
    public AmmoCounter amcountP2;
    public Sprite bamboomSprite;
    public Sprite shrimpSprite;
    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
        giveWeapons();
    }

    public void giveWeapons()
    {
        GameObject p1 = GameObject.Find("Soosh new");
        GameObject p2 = GameObject.Find("Stew new");
        

        
        if (p1 != null)
            ActivateWeaponForPlayer(p1, player1Dish, 1);
        else
        {
            Debug.LogWarning("Player 1 (Soosh new) not found!");
        }
            

        if (p2 != null)
            ActivateWeaponForPlayer(p2, player2Dish, 2);
        else
        {
            Debug.LogWarning("Player 2 (Stew new) not found!");
        }
            
    }

    private void ActivateWeaponForPlayer(GameObject playerObj, string dishName, int playerNum)
    {
        GameObject amcountP1Obj = GameObject.Find("AMMO NUMS P1");
        GameObject amcountP2Obj = GameObject.Find("AMMO NUMS P2");
        if (amcountP1Obj != null)
            amcountP1 = amcountP1Obj.GetComponent<AmmoCounter>();
        if (amcountP2Obj != null)
            amcountP2 = amcountP2Obj.GetComponent<AmmoCounter>();

    

        bool found = false;
        foreach (Transform child in playerObj.transform.Find("Weapons P" + playerNum).transform)
        {
            // Match both the dish and player number
            if (child.name.Contains(dishName) && child.name.EndsWith(playerNum.ToString()))
            {
                child.gameObject.SetActive(true);
                found = true;
                Debug.Log($"Activated {child.name} for Player {playerNum}");
                if(playerNum == 1)
                {
                    amcountP1.weapon = child.GetComponent<Weapon>();
                    child.GetComponent<Weapon>().reloadInProgress = false;
                }
                else
                {
                    amcountP2.weapon = child.GetComponent<Weapon>();
                    child.GetComponent<Weapon>().reloadInProgress = false;
                }
                if (dishName == "Shrimp pistol")
                {
                    GameObject.Find("P" + playerNum + " Weapon Img").GetComponent<Image>().sprite = shrimpSprite;
                }
                else if (dishName == "bamboomstick")
                {
                    GameObject.Find("P" + playerNum + " Weapon Img").GetComponent<Image>().sprite = bamboomSprite;
                }
            }
            else
            {
                child.gameObject.SetActive(false);
            }
        }

        if (!found)
        {
            Debug.LogWarning($"No weapon found for Player {playerNum} matching '{dishName}'");
        }
    }
}
