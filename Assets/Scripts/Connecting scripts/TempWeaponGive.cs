using UnityEngine;

public class TempWeaponGive : MonoBehaviour
{
    [Header("Dish Names (match part of child name)")]
    public string player1Dish; // e.g. "Shrimp pistol" or "Bamboomstick"
    public string player2Dish;

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
        giveWeapons();
    }

    public void giveWeapons()
    {
        GameObject p1 = GameObject.Find("soosh new");
        GameObject p2 = GameObject.Find("stew new");

        if (p1 != null)
            ActivateWeaponForPlayer(p1, player1Dish, 1);
        else
        {
            Debug.LogWarning("Player 1 (Soosh new) not found!");
            return;
        }
            

        if (p2 != null)
            ActivateWeaponForPlayer(p2, player2Dish, 2);
        else
        {
            Debug.LogWarning("Player 2 (Stew new) not found!");
            return;
        }
            
    }

    private void ActivateWeaponForPlayer(GameObject playerObj, string dishName, int playerNum)
    {
        bool found = false;
        foreach (Transform child in playerObj.transform.Find("Weapons").transform)
        {
            // Match both the dish and player number
            if (child.name.Contains(dishName) && child.name.EndsWith(playerNum.ToString()))
            {
                child.gameObject.SetActive(true);
                found = true;
                Debug.Log($"Activated {child.name} for Player {playerNum}");
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
