using UnityEngine;
using UnityEngine.SceneManagement;

// The win con for the cooking phases, determines which player gets an advantage in combat

public class WinCondition : MonoBehaviour
{
    public GameObject p1WinText;
    public GameObject p2WinText;
    private bool hasWinner = false;
    private float timer = 0f;
    private float waitTime = 1f;
    private int sceneToLoad;

    public void declareWinner(int playerNum)
    {
        if (playerNum == 1)
        {
            p1WinText.SetActive(true);
            sceneToLoad = 1;
        }
        else
        {
            p2WinText.SetActive(true);
            sceneToLoad = 2;
        }
        hasWinner = true;
        Time.timeScale = 0;
    }

    void Update()
    {

        if (hasWinner)
        {
            timer += Time.unscaledDeltaTime;
            if ((Input.GetAxisRaw("Interact_P1") > 0 || Input.GetAxisRaw("Interact_P2") > 0) && timer >= waitTime)
            {
                Time.timeScale = 1;
                SceneManager.LoadScene(sceneToLoad);
            }
        }
    }
}