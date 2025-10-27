using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    public int maxHealth;
    public int curHealth;
    public bool isDead;
    public TextMeshProUGUI healthText;
    public Image healthBar;
    public float healthMaxWidth;

    void Start()
    {
        curHealth = maxHealth;
        isDead = false;
        healthMaxWidth = healthBar.rectTransform.sizeDelta.x;
        UpdateHealthUI();
        
    }

    void Update()
    {
        UpdateHealthUI();
    }

    public void takeDamage(int dmg)
    {
        curHealth -= dmg;
        if (curHealth <= 0 && !isDead)
        {
            Die();
        }
        UpdateHealthUI();
    }

    void UpdateHealthUI()
    {
        if (healthText != null)
        {
            healthText.text = curHealth.ToString() + " / " + maxHealth.ToString();
        }
        if (healthBar != null)
        {
            float healthPercent = (float)curHealth / (float)maxHealth;
            healthBar.rectTransform.sizeDelta = new Vector2(healthMaxWidth * healthPercent, healthBar.rectTransform.sizeDelta.y);
        }
    }
    void Die()
    {
        isDead = true;
        Debug.Log(transform.name + " has died.");
    }
}
