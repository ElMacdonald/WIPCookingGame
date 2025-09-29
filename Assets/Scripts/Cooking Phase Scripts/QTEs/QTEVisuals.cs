using UnityEngine;
using UnityEngine.UI;

//FOR TESTING ONLY! maybe idk


public class QTEVisuals : MonoBehaviour
{
    private float timer;
    public float timeToPress;
    private float fillRatio;
    private float currentSize;
    private float minSize=0;
    private float maxSize=300;

    public RectTransform bar;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void startQTETimer()
    {
        timer = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        fillRatio = 1 - timer / timeToPress;

        if(fillRatio > 1)
        {
            fillRatio = 1;
        }
        else if(fillRatio < 0)
        {
            fillRatio = 0;
        }

        currentSize = maxSize * fillRatio;

        bar.sizeDelta = new Vector2(currentSize, 50);
    }
}
