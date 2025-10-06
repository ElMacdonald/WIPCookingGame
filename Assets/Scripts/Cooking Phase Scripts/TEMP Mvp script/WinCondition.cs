using UnityEngine;

public class WinCondition : MonoBehaviour
{
    public GameObject p1WinText;
    public GameObject p2WinText;

    public void declareWinner(int playerNum)
    {
        if (playerNum == 1)
        {
            p1WinText.SetActive(true);
        }
        else
        {
            p2WinText.SetActive(true);
        }
        Time.timeScale = 0;
    }
}
