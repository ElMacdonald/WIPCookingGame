using UnityEngine;
using UnityEngine.SceneManagement;


//Temp combat win condition for playtests

public class CombatWinCon : MonoBehaviour
{
    public Health player1Health;
    public Health player2Health;

    // Update is called once per frame
    void Update()
    {
        if (player1Health.isDead)
        {
            Debug.Log("Player 2 Wins!");
            Time.timeScale = 0;
        }
        else if (player2Health.isDead)
        {
            Debug.Log("Player 1 Wins!");
            Time.timeScale = 0;
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(0);
        }
    }
}
