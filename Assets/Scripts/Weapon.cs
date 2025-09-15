using UnityEngine;

public class Weapon : MonoBehaviour
{
    public int dmg;
    public float firerate;
    private float firerateTimer;
    public float range;
    private Transform cam;
    public AudioSource[] audios;



    void Start()
    {

    }

    void Update()
    {

    }

    //Sends out a raycast and damages other player, overrideable by unconventional weapons
    private void Fire()
    {

    }

    //reloads the weapon
    private void Reload()
    {

    }

    //plays sound effect held within weapon code
    private void PlaySound(AudioSource aud)
    {

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

    }
}
