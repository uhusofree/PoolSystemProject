using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FlashhingText : MonoBehaviour
{
    Text text;
    public float flashingIn = .5f;
    public float flashingOut = .7f;
    public float flashingPause = .8f;
    float timer = 0;
    private Color color;

    private void Start()
    {
        text = GetComponent<Text>();
        color = text.color;
        this.gameObject.SetActive(false);
    }

    private void Update()
    {
        timer += Time.deltaTime;
        if (timer < flashingIn)
        {
            text.color = new Color(color.r, color.g, color.b, timer / flashingIn);
        }
        else if (timer < flashingIn + flashingPause)
        {
            text.color = new Color(color.r, color.g, color.b, 1);
        }
        else if (timer < flashingIn + flashingPause + flashingOut)
        {
            text.color = new Color(color.r, color.g, color.b, 1 - (timer - (flashingIn + flashingPause)) / flashingOut);
        }
        else
        {
            timer = 0;
        }
    }

}
