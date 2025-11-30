using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TicketSpawner : MonoBehaviour
{
    public GameObject[] tickets; //four total
    public GameObject ticketPanel;
    public string[][] recipes;
    public string[][] activeRecipes;
    public Sprite[] recipePrefabs;
    public float spawnTime;
    public int ticketCount;

    public float timer;

    void Update()
    {
        timer += Time.deltaTime;
        if(timer >= spawnTime)
        {
            GenerateRecipe();
            timer = 0f;
            Debug.Log("Generated Ticket");
        }
    }


    void GenerateRecipe()
    {
        if(ticketCount >= tickets.Length)
        {
            return;
        }
        int recipeIndex = Random.Range(0, recipes.Length);
    }

    void SetIngredients(int ticket, int recipeNum)
    {
        tickets[ticket].GetComponent<Image>().sprite = recipePrefabs[recipeNum];
    }

    void ClearTicket(int ticket)
    {
        tickets[ticket].SetActive(false);
        ticketCount--;
    }
    
}
