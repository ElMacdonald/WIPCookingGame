using UnityEngine;

public class StartControl : MonoBehaviour
{

    private PlayerControls input;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        input = new PlayerControls();
        input.Enable();
        Time.timeScale = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (input.Player.A.ReadValue<float>() > 0)
        {
            Time.timeScale = 1;
        }
        /*
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("Ingredient Testing");
        }
        */
    }
}
