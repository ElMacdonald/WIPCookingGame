using UnityEngine;

public class TodayBuildWin : MonoBehaviour
{
    public GameObject p1WinText;
    public GameObject p2WinText;

    public Health p1Health;
    public Health p2Health;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(p1Health.curHealth <= 0)
        {
            p2WinText.SetActive(true);
            Time.timeScale = 0;
        }
        else if(p2Health.curHealth <= 0)
        {
            p1WinText.SetActive(true);
            Time.timeScale = 0;
        }
    }
}
