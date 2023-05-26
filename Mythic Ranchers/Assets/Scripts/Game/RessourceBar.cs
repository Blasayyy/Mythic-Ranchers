using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.UI;

public class RessourceBar : NetworkBehaviour
{
    public Slider ressourceSlider;
    [SerializeField]
    public Color manaColor, energyColor;
    [SerializeField]
    public Vector3 offset;

    public void SetRessource(float currentRessource, float maxRessource, string ressourceType)
    {
        ressourceSlider.value = currentRessource;
        ressourceSlider.maxValue = maxRessource;
        ressourceSlider.fillRect.GetComponentInChildren<Image>().color = manaColor;
        if (ressourceType == "mana")
        {
            ressourceSlider.fillRect.GetComponentInChildren<Image>().color = manaColor;
        }
        else if (ressourceType == "energy")
        {
            ressourceSlider.fillRect.GetComponentInChildren<Image>().color = energyColor;
        }
    }

    private void FixedUpdate()
    {
        ressourceSlider.transform.position = Camera.main.WorldToScreenPoint(transform.parent.position + offset);
    }

    //private void Update()
    //{        
    //    ressourceSlider.transform.position = Camera.main.WorldToScreenPoint(transform.parent.position + offset);
    //}
}
