using UnityEngine;
using TMPro;
public class AmmoCounter : MonoBehaviour
{
    public TextMeshProUGUI ammoText;
    public Weapon weapon;
    public int playerNum;
    public TempWeaponGive tempWeaponGive;
    // Initializes the counter
    void Start()
    {
        ammoText.text = weapon.curMag + " / " + weapon.magSize + "\n" + weapon.reserveAmmo;
        tempWeaponGive = FindFirstObjectByType<TempWeaponGive>();
        Debug.Log("Trying to find weapon: " + tempWeaponGive.player2Dish + " " + playerNum);
        //GameObject.Find(tempWeaponGive.player2Dish + " " + playerNum).SetActive(true);
        weapon = GameObject.Find(tempWeaponGive.player2Dish + " " + playerNum).GetComponent<Weapon>();
    }

    // Sets the ammo counter text every frame
    void Update()
    {
        if (weapon != null && !weapon.reloadInProgress)
        {
            ammoText.text = weapon.curMag + " / " + weapon.magSize + "\n" + weapon.reserveAmmo;
        }
        else
        {
            ammoText.text = "Reloading..." + "\n" + weapon.reserveAmmo;
        }
    }
}
