using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Unity.Netcode;

public class HealthBar : NetworkBehaviour
{
    public Slider healthSlider;
    [SerializeField]
    public Color lowColor, highColor;
    [SerializeField]
    public Vector3 offset;

    public void SetHealth(float health, float maxHealth)
    {
        healthSlider.value = health;
        healthSlider.maxValue = maxHealth;
        healthSlider.fillRect.GetComponentInChildren<Image>().color = Color.Lerp(lowColor, highColor, healthSlider.normalizedValue);
    }

    private void FixedUpdate()
    {        
        healthSlider.transform.position = Camera.main.WorldToScreenPoint(transform.parent.position + offset);
    }
}
