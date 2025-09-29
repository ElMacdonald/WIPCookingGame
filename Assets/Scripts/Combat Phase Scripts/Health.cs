using UnityEngine;

public class Health : MonoBehaviour
{
    public int maxHealth;
    public int curHealth;
    public bool isDead;

    void Start()
    {
        curHealth = maxHealth;
        isDead = false;
    }

    public void takeDamage(int dmg)
    {
        curHealth -= dmg;
        if (curHealth <= 0 && !isDead)
        {
            Die();
        }
    }

    void Die()
    {
        isDead = true;
        Debug.Log(transform.name + " has died.");
    }
}
