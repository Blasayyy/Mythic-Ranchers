using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.UI;

public class RessourceBar : NetworkBehaviour
{
    public Slider ressourceSlider;
    [SerializeField]
    public Color ressourceColor;
    [SerializeField]
    public Vector3 offset;

    public void SetRessource(float currentRessource, float maxRessource)
    {
        ressourceSlider.value = currentRessource;
        ressourceSlider.maxValue = maxRessource;

        ressourceSlider.fillRect.GetComponentInChildren<Image>().color = ressourceColor;
    }

    private void FixedUpdate()
    {
        ressourceSlider.transform.position = Camera.main.WorldToScreenPoint(transform.parent.position + offset);
    }
}
