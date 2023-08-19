using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ManaBarScript : MonoBehaviour
{
    public Slider slider;
    private float maxMana;

    public void SetMaxMana(float mana)
    {
        slider.maxValue = mana;
        slider.value = mana;
        maxMana = mana;

    }

    public void SetMana(float mana)
    {
        StartCoroutine(Lerp(mana, 0.6f));

    }

    IEnumerator Lerp(float mana, float t)
    {

        float time = 0;
        float startValue = slider.value;
        //float u = Mathf.Sin(time/t * Mathf.PI/2) ;
        while (time < t)
        {
            float u = Mathf.Sin(time / t * Mathf.PI / 2);
            slider.value = Mathf.Lerp(startValue, mana, u);
            time += Time.deltaTime;
            yield return null;
        }
        slider.value = mana;
    }
}
