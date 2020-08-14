using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthSystem : MonoBehaviour
{
    [SerializeField] Slider Healthbar;
    [SerializeField] Text HealthbarText;
    private int pHealth, pMaxHealth = 265;

    void Start()
    {
        pHealth = pMaxHealth;
        UpdateHealthbar();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.U))
        {
            TakeDamage(15);
        }
    }

    public int GetHealth()
    {
        return pHealth;
    }

    public void SetHealth(int newHealth)
    {
        pHealth = newHealth;
        UpdateHealthbar();
    }

    public void TakeDamage(int damage)
    {
        pHealth -= damage;
        UpdateHealthbar();
    }

    public void Heal(int healAmount)
    {
        int resultingHealth = pHealth + healAmount;

        if (resultingHealth > pMaxHealth)
        {
            pHealth = pMaxHealth;
        }
        else
        {
            pHealth = resultingHealth;
        }

        UpdateHealthbar();
    }

    private void UpdateHealthbar()
    {
        float percentage = (float)pHealth / (float)pMaxHealth;
        Healthbar.value = percentage;
        HealthbarText.text = pHealth.ToString();
    }
}
