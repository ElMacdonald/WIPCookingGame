using UnityEngine;
using UnityEngine.System.Collections;

public class Weapon : MonoBehaviour
{
    [Header("Weapon Stats")]
    public int dmg;
    public float firerate;
    public float firerateTimer;
    public float range;
    public float reloadTime;
    public int magSize;
    public int curMag;
    public int reserveAmmo;
    private Transform cam;

    [Header("Sound Effects")]
    public AudioSource[] audios;
    private bool reloadInProgress;




    void Start()
    {
        cam = Camera.main.transform;
    }

    void Update()
    {
        firerateTimer += Time.deltaTime;
        InputManager();
    }

    //Sends out a raycast and damages other player, overrideable by unconventional weapons
    private void Fire()
    {
        if (curMag > 0)
        {
            if (Physics.Raycast(cam.position, cam.forward, out RaycastHit hit, range))
            {
                Debug.Log("Hit " + hit.transform.name);
            }
        }
    }

    //reloads the weapon
    private IEnumerator Reload()
    {
        yield return new WaitForSeconds(reloadTime);
        int neededAmmo = magSize - curMag;

        if (reserveAmmo >= neededAmmo)
        {
            curMag += neededAmmo;
            reserveAmmo -= neededAmmo;
        }
        else
        {
            curMag += reserveAmmo;
            reserveAmmo = 0;
        }

        reloadInProgress = false;
    }

    //plays sound effect held within weapon code
    private IEnumerator PlaySound(AudioSource aud)
    {
        yield return new WaitForSeconds(0.01f);
    }

    //Emulates weapon recoil
    private void Recoil()
    {

    }

    //Shakes camera for enhanced gunplay feel
    private void CamShake()
    {

    }

    //handles input from player
    private void InputManager()
    {
        //Shooting
        if (Input.GetAxis("Fire1") != 0 && firerate > firerateTimer)
        {
            Fire();
        }

        //Reloading
        if (Input.GetKeyDown(KeyCode.R) && !reloadInProgress)
        {
            reloadInProgress = true;
            Reload();
        }
    }
}
