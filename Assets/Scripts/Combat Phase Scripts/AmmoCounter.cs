using UnityEngine;
using TMPro;
public class AmmoCounter : MonoBehaviour
{
    public TextMeshProUGUI ammoText;
    public Weapon weapon;
    // Initializes the counter
    void Start()
    {
        ammoText.text = weapon.curMag + " / " + weapon.magSize + "\n" + weapon.reserveAmmo;
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
