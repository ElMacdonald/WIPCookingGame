using UnityEngine;

public class RecipeBook : MonoBehaviour
{

    public GameObject bookPanel;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        bookPanel.SetActive(false);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            bookPanel.SetActive(true);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            bookPanel.SetActive(false);
        }
    }
}
