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
    public TempWeaponGive tempWeaponGive;

    public void declareWinner(int playerNum, string dishName)
    {
        if (playerNum == 1)
        {
            //p1WinText.SetActive(true);
            sceneToLoad = 1;
            tempWeaponGive.player1Dish = dishName;
        }
        else
        {
            //p2WinText.SetActive(true);
            sceneToLoad = 1;
            tempWeaponGive.player2Dish = dishName;
        }
        if(tempWeaponGive.player1Dish != "" && tempWeaponGive.player2Dish != "")
        {
            hasWinner = true;
            Time.timeScale = 0;
        }
        
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