using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageDealer : MonoBehaviour
{
    [SerializeField] private int projectileDamage = 50;
    // Start is called before the first frame update
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            collision.GetComponent<Player>().ReceiveDamage(projectileDamage);
        }
        else
        {
            return;
        }

        this.gameObject.SetActive(false);
    }
}
