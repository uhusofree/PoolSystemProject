using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms;

public class HealthBoost : MonoBehaviour
{
    [SerializeField] private int healthBoost = 50;
    private float timer;
    [SerializeField] private float timeLimit = 5f;

    private void Start()
    {
        timer = timeLimit;
    }
    private void Update()
    {
        if (timer > 0)
        {
            timer -= Time.deltaTime;
        }
        else if (timer <= 0)
        {
            OnHit();
        }
    }
    public int GetHealthBoost()
    {
        return healthBoost;
    }

    public void OnHit()
    {
        this.gameObject.SetActive(false);
    }

   
}
