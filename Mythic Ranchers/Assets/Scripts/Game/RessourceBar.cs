using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.UI;

public class RessourceBar : NetworkBehaviour
{
    public Slider ressourceSlider;
    [SerializeField]
    public Color manaColor, rageColor, energyColor;    
    [SerializeField]
    public Vector3 offset;
    private PlayerUnit player;


    private void Start()
    {
        if (GetComponentInParent<PlayerUnit>())
        {
            player = GetComponentInParent<PlayerUnit>();
        }
    }

    public void SetRessource(float currentRessource, float maxRessource)
    {
        ressourceSlider.value = currentRessource;
        ressourceSlider.maxValue = maxRessource;
        if (!player || player.RessourceType == "mana")
        {
            ressourceSlider.fillRect.GetComponentInChildren<Image>().color = manaColor;
        }
        else if (player.RessourceType == "rage")
        {
            ressourceSlider.fillRect.GetComponentInChildren<Image>().color = rageColor;
        }
        else if (player.RessourceType == "energy")
        {
            ressourceSlider.fillRect.GetComponentInChildren<Image>().color = energyColor;
        }
    }

    private void FixedUpdate()
    {
        ressourceSlider.transform.position = Camera.main.WorldToScreenPoint(transform.parent.position + offset);
    }
}
