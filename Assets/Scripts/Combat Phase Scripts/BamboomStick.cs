using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BamboomStick : Weapon
{
    [Header("Bamboom Settings")]
    public int pelletCount = 8;           // number of pellets fired per shot
    public float spreadAngle = 5f;        // degrees of random spread
    public float pelletRangeMultiplier = 1f; // adjust if you want slightly shorter or longer range

    public override void Fire()
    {
        if (curMag <= 0)
        {
            Reload();
            return;
        }

        
        curMag--;
        firerateTimer = 0f;

        Vector3 rayOrigin = cam.position;
        Vector3 forward = cam.forward;

        // Build ignore collider set (same as base)
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

        // Fire multiple pellets
        for (int i = 0; i < pelletCount; i++)
        {
            // Random spread direction
            Vector3 spreadDir = Quaternion.Euler(
                Random.Range(-spreadAngle, spreadAngle),
                Random.Range(-spreadAngle, spreadAngle),
                0f
            ) * forward;

            RaycastHit[] hits = Physics.RaycastAll(rayOrigin, spreadDir, range * pelletRangeMultiplier, Physics.DefaultRaycastLayers, QueryTriggerInteraction.Ignore);
            Vector3 finalHitPoint = rayOrigin + spreadDir * range;
            bool validHit = false;

            if (hits != null && hits.Length > 0)
            {
                System.Array.Sort(hits, (a, b) => a.distance.CompareTo(b.distance));

                foreach (var h in hits)
                {
                    if (h.collider == null) continue;
                    if (ignoreColliders.Contains(h.collider))
                        continue;

                    finalHitPoint = h.point;
                    validHit = true;

                    Health target = h.collider.GetComponentInParent<Health>();
                    if (target != null)
                    {
                        target.takeDamage(dmg);
                        FlashCrosshair();
                    }
                    break;
                }
            }

            // spawn tracer for each pellet
            StartCoroutine(SpawnBulletTrail(finalHitPoint));
        }
    }
}
