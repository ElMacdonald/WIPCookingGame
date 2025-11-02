using UnityEngine;

public class Interactable : MonoBehaviour
{
    private Renderer rend;
    public Material highlightOverlayMat;

    private Material[] originalMats;
    private bool isHighlighted;

    void Awake()
    {
        rend = GetComponent<Renderer>();
        originalMats = rend.materials;
    }

    public void Highlight()
    {
        if (isHighlighted) return;
        isHighlighted = true;

        Material[] mats = new Material[originalMats.Length + 1];
        originalMats.CopyTo(mats, 0);
        mats[mats.Length - 1] = highlightOverlayMat;
        rend.materials = mats;
    }

    public void RemoveHighlight()
    {
        if (!isHighlighted) return;
        isHighlighted = false;

        rend.materials = originalMats;
    }
}
