using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InvulnerableBar : MonoBehaviour
{
   public Slider invulnerableSlider;

    public void SetInvulnerabilityMaxValue(float timer)
    {
        invulnerableSlider.maxValue = timer; 
    }

    public void SetInvulernabilityBarValue(float timer)
    {
        invulnerableSlider.value = timer;
    }
}
