using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class RestaurantDestruction : MonoBehaviour
{
    public GameObject[] Destructables;
    public float ComebackTime = 10f;
    public float totalRestaurantScore = 100f; //influences customer satisfaction

    public GameObject rubble;


    //Starts thread that destroys object then respawns it
    public void DestroyDestructable(GameObject item)
    {
        StartCoroutine(DestroyItem(item));
    }

    IEnumerator DestroyItem(GameObject item)
    {
        item.SetActive(false);
        //Takes length of destructables and reduces restaurant score by that amount
        totalRestaurantScore  -= (100f / Destructables.Length);
        yield return new WaitForSeconds(ComebackTime);
        if (!item.activeInHierarchy)
        {
            item.SetActive(true);
            totalRestaurantScore += (100f / Destructables.Length);
        }
    }

    public void makeRubble(Vector3 position)
    {
        Instantiate(rubble, position, Quaternion.identity);

    }

    public void repairRestaurant(GameObject item)
    {
        if (!item.activeInHierarchy)
        {
            item.SetActive(true);
            totalRestaurantScore += (100f / Destructables.Length);
        }
    }
}
