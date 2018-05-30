using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour {

    public float health, maxHealth = 100f, hunger, maxHunger = 100f, thirst, maxThirst = 100f, stamina, maxStamina = 100f;
    public float healthRegenRate = .1f, hungerIncreaseRate = .2f, thirstIncreaseRate = .25f, staminaLossRate = .75f, staminaRegenRate = .5f;
    public float healthLossWhenHungry = .4f, healthLossWhenThirsty = .5f;

    public Image healthImg, hungerImg, thirstImg, staminaImg;
    public Text healthTxt, hungerTxt, thirstTxt, staminaTxt;

    private void Start()
    {
        health = maxHealth;
        hunger = maxHunger;
        thirst = maxThirst;
        stamina = maxStamina;
    }

    private void Update()
    {
        hunger -= hungerIncreaseRate * Time.deltaTime;
        thirst -= thirstIncreaseRate * Time.deltaTime;

        if (hunger <= 0)
        {
            hunger = 0;
            health -= healthLossWhenHungry * Time.deltaTime;
        }
        if (thirst <= 0)
        {
            thirst = 0;
            health -= healthLossWhenThirsty * Time.deltaTime;
        }

        CheckHealth();
        UpdateUI();
    }

    void CheckHealth()
    {
        if (health <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log("You are dead !");
    }

    void UpdateUI()
    {
        var healthPercent = health / maxHealth;
        healthImg.fillAmount = healthPercent;
        healthTxt.text = Mathf.RoundToInt(healthPercent * 100f).ToString();

        var hungerPercent = hunger / maxHunger;
        hungerImg.fillAmount = hungerPercent;
        hungerTxt.text = Mathf.RoundToInt(hungerPercent * 100f).ToString();

        var thirstPercent = thirst / maxThirst;
        thirstImg.fillAmount = thirstPercent;
        thirstTxt.text = Mathf.RoundToInt(thirstPercent * 100f).ToString();

        var staminaPercent = stamina / maxStamina;
        staminaImg.fillAmount = staminaPercent;
        staminaTxt.text = Mathf.RoundToInt(staminaPercent * 100f).ToString();
    }

    public void Regen(float _health, float _hunger, float _thirst, float _stamina)
    {
        health += _health;
        hunger += _hunger;
        thirst += _thirst;
        stamina += _stamina;
    }

    public void TakeDamage(float _damage)
    {
        health -= _damage;
    }

}
