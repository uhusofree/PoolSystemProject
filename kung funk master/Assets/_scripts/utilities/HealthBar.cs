using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Image healthBarFill;


    public void SetMaxHealth(int maxHealth)
    {
        healthBarFill.fillAmount = maxHealth;
    }
   public void SetHealthBar(int health, int maxHealth)
    {
        healthBarFill.fillAmount = (float)health / (float)maxHealth;
    }
}
