using UnityEngine;

public class CameraPositioner : MonoBehaviour
{
    public Transform pointOne;
    public Transform pointTwo;
    public float moveSpeed = 5f;

    private Coroutine moveRoutine;

    public void MoveToPointOne()
    {
        StartMove(pointOne);
    }

    public void MoveToPointTwo()
    {
        StartMove(pointTwo);
    }

    private void StartMove(Transform target)
    {
        if (moveRoutine != null)
            StopCoroutine(moveRoutine);

        moveRoutine = StartCoroutine(LerpToTransform(target));
    }

    private System.Collections.IEnumerator LerpToTransform(Transform target)
    {
        Vector3 startPos = transform.position;
        Quaternion startRot = transform.rotation;

        Vector3 endPos = target.position;
        Quaternion endRot = target.rotation;

        float t = 0f;
        bool rotationDone = false;

        while (t < 1f)
        {
            t += Time.deltaTime * moveSpeed;

            transform.position = Vector3.Lerp(startPos, endPos, t);

            if (!rotationDone)
            {
                float rotationT = t * 2f;  // reach 1 at halfway
                if (rotationT >= 1f)
                {
                    // snap cleanly & stop rotating
                    transform.rotation = endRot;
                    rotationDone = true;
                }
                else
                {
                    transform.rotation = Quaternion.Slerp(startRot, endRot, rotationT);
                }
            }

            yield return null;
        }

        // finishes movement
        transform.position = endPos;
        if (!rotationDone)
            transform.rotation = endRot;

        moveRoutine = null;
    }
}
