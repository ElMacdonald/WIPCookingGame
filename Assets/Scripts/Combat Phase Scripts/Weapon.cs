using UnityEngine;
using System.Collections;
using TMPro;
using UnityEngine.UI;
using System.Linq;
using System.Collections.Generic;

public class Weapon : MonoBehaviour
{
    [Header("UI / Camera")]
    public TextMeshProUGUI crosshair;
    public Transform cam;

    [Header("Weapon Stats")]
    public int dmg;
    public float firerate;
    public float range;
    public float reloadTime;
    public int magSize;
    public int curMag;
    public int reserveAmmo;

    [Header("References")]
    public GameObject player;                // the owner of this weapon (will be ignored by raycasts)
    public GameObject[] playersToIgnore;     // optionally ignore other players (teammates)
    public Transform muzzlePoint;            // where the tracer starts

    [Header("Audio")]
    public AudioSource[] audios;

    [Header("Crosshair Feedback")]
    public Color hitColor = Color.red;
    public float flashDuration = 0.15f;
    private Color defaultColor;
    private Coroutine flashCoroutine;

    [Header("Bullet Trail Settings")]
    public Material bulletTrailMaterial;
    public float trailDuration = 0.05f;
    public float trailFadeTime = 0.1f;
    public float trailWidth = 0.02f;

    [Header("Runtime")]
    public bool reloadInProgress;
    public float firerateTimer;

    [Header("Input")]
    public int playerNum;
    private bool fireButton;
    private bool reloadButton;

    private void Start()
    {
        if (crosshair != null)
            defaultColor = crosshair.color;
    }

    private void Update()
    {
        firerateTimer += Time.deltaTime;
        //InputManager();
    }

    // -------------------
    // Public Fire Method
    // -------------------
    public virtual void Fire()
    {
        if (curMag <= 0)
        {
            Reload();
            return;
        }

        curMag--;
        firerateTimer = 0f;

        Vector3 rayOrigin = cam.position;
        Vector3 rayDirection = cam.forward;

        // Get all ray hits along the path (sorted by distance)
        RaycastHit[] hits = Physics.RaycastAll(rayOrigin, rayDirection, range, Physics.DefaultRaycastLayers, QueryTriggerInteraction.Ignore);
        if (hits == null || hits.Length == 0)
        {
            // nothing hit: spawn trail to max range
            Vector3 defaultEnd = rayOrigin + rayDirection * range;
            StartCoroutine(SpawnBulletTrail(defaultEnd));
            return;
        }

        System.Array.Sort(hits, (a, b) => a.distance.CompareTo(b.distance));

        // Build a set of colliders to ignore: the shooter + any playersToIgnore (teammates)
        HashSet<Collider> ignoreColliders = new HashSet<Collider>();
        if (player != null)
        {
            foreach (var c in player.GetComponentsInChildren<Collider>())
                if (c != null) ignoreColliders.Add(c);
        }
        if (playersToIgnore != null)
        {
            foreach (var p in playersToIgnore)
            {
                if (p == null) continue;
                foreach (var c in p.GetComponentsInChildren<Collider>())
                    if (c != null) ignoreColliders.Add(c);
            }
        }

        // Iterate hits and find the first valid (non ignored) hit
        Vector3 finalHitPoint = rayOrigin + rayDirection * range; 
        bool foundValid = false;

        foreach (var h in hits)
        {
            if (h.collider == null) continue;

            // If this collider belongs to a GameObject we should ignore, skip it
            if (ignoreColliders.Contains(h.collider))
                continue;

            // Otherwise this is the first valid thing the bullet actually "hits"
            finalHitPoint = h.point;
            foundValid = true;

            // Attempt to get a Health component on the hit object or its parents
            Health target = h.collider.GetComponentInParent<Health>();
            if (target != null)
            {
                target.takeDamage(dmg);
                FlashCrosshair();
            }

            // stop at the first non-ignored collider (bullet doesn't continue)
            break;
        }

        // Spawn the tracer to the final hit point (either a real hit or max range)
        StartCoroutine(SpawnBulletTrail(finalHitPoint));
    }

    // -------------------
    // Tracer visuals
    // -------------------
    public IEnumerator SpawnBulletTrail(Vector3 hitPoint)
    {
        if (muzzlePoint == null || bulletTrailMaterial == null)
            yield break;

        GameObject trailObj = new GameObject("BulletTrail");
        LineRenderer lr = trailObj.AddComponent<LineRenderer>();

        lr.material = new Material(bulletTrailMaterial);
        lr.startWidth = trailWidth;
        lr.endWidth = trailWidth;
        lr.positionCount = 2;
        lr.useWorldSpace = true;

        Vector3 start = muzzlePoint.position;
        lr.SetPosition(0, start);
        lr.SetPosition(1, hitPoint);

        // keep visible briefly
        yield return new WaitForSeconds(trailDuration);

        // fade out
        float elapsed = 0f;
        Color matColor = lr.material.color;
        while (elapsed < trailFadeTime)
        {
            elapsed += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0f, elapsed / trailFadeTime);
            Color c = matColor;
            c.a = alpha;
            lr.material.color = c;
            yield return null;
        }

        Destroy(trailObj);
    }

    // -------------------
    // Crosshair flash
    // -------------------
    public void FlashCrosshair()
    {
        if (crosshair == null) return;
        if (flashCoroutine != null) StopCoroutine(flashCoroutine);
        flashCoroutine = StartCoroutine(FlashCrosshairCoroutine());
    }

    private IEnumerator FlashCrosshairCoroutine()
    {
        crosshair.color = hitColor;
        yield return new WaitForSeconds(flashDuration);
        crosshair.color = defaultColor;
    }

    // -------------------
    // Reloading
    // -------------------
    public IEnumerator Reload()
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

    // -------------------
    // Input handling
    // -------------------
    private void InputManager()
    {
        if (playerNum == 1)
        {
            fireButton = Input.GetAxis("Fire_P1") > 0;
            reloadButton = Input.GetAxis("Reload_P1") > 0;
        }
        else
        {
            fireButton = Input.GetAxis("Fire_P2") > 0;
            reloadButton = Input.GetAxis("Reload_P2") > 0;
        }

        if (fireButton && firerate < firerateTimer)
        {
            Fire();
            firerateTimer = 0f;
        }

        if (reloadButton && !reloadInProgress)
        {
            StartCoroutine(Reload());
            reloadInProgress = true;
        }
    }

    public void TakeInput(string buttonName)
    {
        if (buttonName == "RT")
        {
            if (firerate < firerateTimer)
            {
                Fire();
                firerateTimer = 0f;
            }
        }
        else if (buttonName == "X")
        {
            if (!reloadInProgress)
            {
                reloadInProgress = true;
                Debug.Log("Reloading started via InputHandler");
                StartCoroutine(Reload());
            }
        }
    }
}
