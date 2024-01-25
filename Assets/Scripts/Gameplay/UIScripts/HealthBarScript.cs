using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarScript : MonoBehaviour
{
    /// <summary>
    /// The health slider.
    /// </summary>
    public Slider slider;

    /// <summary>
    /// Set the health slider value and maximum value.
    /// </summary>
    /// <param name="health">The current health value.</param>
    /// <param name="maxhealth">The maximum health value.</param>
    public void SetHealth(float health, float maxhealth)
    {
        slider.maxValue = maxhealth;
        slider.value = health;
    }
}
