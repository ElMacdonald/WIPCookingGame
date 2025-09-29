using UnityEngine;
using System.Collections;

public class Weapon : MonoBehaviour
{
    public int dmg;
    public float firerate;
    public float range;
    public float reloadTime;
    public int magSize;
    public int curMag;
    public int reserveAmmo;
    private Transform cam;

    [Header("Sound Effects")]
    public AudioSource[] audios;
    
    public bool reloadInProgress;
    public float firerateTimer;

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
    public void Fire()
    {
        Debug.Log("Firing");
        if (curMag > 0)
        {
            curMag--;
            firerateTimer = 0;
            if (Physics.Raycast(cam.position, cam.forward, out RaycastHit hit, range))
            {
                Debug.Log("Hit " + hit.transform.name);
                Health target = hit.transform.GetComponent<Health>();
                if (target != null)
                {
                    target.takeDamage(dmg);
                }
            }
        }
    }

    //reloads the weapon
    public IEnumerator Reload()
    {
        Debug.Log("beginning wait");
        yield return new WaitForSeconds(reloadTime);
        Debug.Log("wait over"); 
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
        cam.GetComponent<CameraShake>().Shake();
    }

    //handles input from player
    private void InputManager()
    {
        //Shooting
        if (Input.GetAxis("Fire1") != 0 && firerate < firerateTimer)
        {
            Fire();
            CamShake();
        }

        //Reloading
        if (Input.GetKeyDown(KeyCode.R) && !reloadInProgress)
        {
            StartCoroutine(Reload());
            Debug.Log("Reloading");
            reloadInProgress = true;
        }
    }
}
