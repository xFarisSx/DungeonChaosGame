using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarScript : MonoBehaviour
{

    public Slider slider;
    private float maxHealth;

    public void SetMaxHealth(int health)
    {
        slider.maxValue = health;
        slider.value = health;
        maxHealth = health;

        transform.GetChild(2).GetChild(0).GetComponent<Text>().text = health.ToString();
    }

    public void SetHealth(int health)
    {
        StartCoroutine(Lerp(health, 0.5f));
        transform.GetChild(2).GetChild(0).GetComponent<Text>().text = health.ToString();
        if (transform.GetChild(2).GetComponent<Animator>())
        {
            transform.GetChild(2).GetComponent<Animator>().speed = maxHealth / health;
        }
        
    }

    IEnumerator Lerp(int health, float t)
    {

        float time = 0;
        float startValue = slider.value;
        //float u = Mathf.Sin(time/t * Mathf.PI/2) ;
        while (time < t)
        {
            float u = Mathf.Sin(time/t * Mathf.PI/2) ;
            slider.value = Mathf.Lerp(startValue, health, u);
            time += Time.deltaTime;
            yield return null;
        }
        slider.value = health;
    }
}
