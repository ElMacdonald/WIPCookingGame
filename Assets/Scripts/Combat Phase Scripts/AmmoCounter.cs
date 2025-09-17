using UnityEngine;
using TMPro;
public class AmmoCounter : MonoBehaviour
{
    public TextMeshProUGUI ammoText;
    public Weapon weapon;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ammoText.text = weapon.curMag + " / " + weapon.magSize + "\n" + weapon.reserveAmmo;
    }

    // Update is called once per frame
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
