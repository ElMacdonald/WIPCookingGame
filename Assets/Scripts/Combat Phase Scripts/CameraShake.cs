using UnityEngine;
using System.Collections;

public class CameraShake : MonoBehaviour
{
    public float shakeDuration = 0.1f; // Duration of the shake effect
    public float shakeMagnitude = 0.1f; // Magnitude of the shake effect
    public Transform camTransform;

    void Start()
    {
        if (camTransform == null)
        {
            camTransform = Camera.main.transform;
        }
    }

    public void Shake()
    {
        StartCoroutine(ShakeCoroutine());
    }

    public IEnumerator ShakeCoroutine()
    {
        Vector3 originalPos = camTransform.localPosition;
        float elapsed = 0.0f;

        while (elapsed < shakeDuration)
        {
            float x = Random.Range(-1f, 1f) * shakeMagnitude;
            float y = Random.Range(-1f, 1f) * shakeMagnitude;

            camTransform.localPosition = new Vector3(x, y, originalPos.z);

            elapsed += Time.deltaTime;

            yield return null;
        }

        camTransform.localPosition = originalPos;
        Debug.Log("shook");
    }
}
