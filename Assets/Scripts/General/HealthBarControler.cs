using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarControler : MonoBehaviour
{
    private Slider slider;

    private void Awake()
    {
        slider = this.gameObject.GetComponent<Slider>();
    }

    public void SetMax(float maxHealth)
    {
        slider.maxValue = maxHealth;
        slider.value = maxHealth;
    }

    public void ChangeHealthBar(float currentHealth)
    {
        slider.value = currentHealth;
    }
}
