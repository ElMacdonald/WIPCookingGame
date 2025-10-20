using UnityEngine;
using UnityEngine.UI;
public class KnifeAnim : MonoBehaviour
{
    public Sprite[] knifeSprites;
    public int currentFrame = 0;
    public float frameRate = 0.1f; // Time in seconds between frames
    private float timer = 0f;
    private Image spriteRenderer;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        spriteRenderer = GetComponent<Image>();
        if (knifeSprites.Length > 0)
        {
            spriteRenderer.sprite = knifeSprites[0];
        }
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= frameRate)
        {
            timer = 0f;
            currentFrame = (currentFrame + 1) % knifeSprites.Length;
            spriteRenderer.sprite = knifeSprites[currentFrame];
        }
    }
}
