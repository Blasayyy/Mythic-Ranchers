using UnityEngine;
using UnityEngine.UI;
using Unity.Netcode;

/*******************************************************************************

   Nom du fichier: HealthBar.cs
   
   Contexte: Cette classe sert à informer le joueur sur le nombre de vie restantes aux
             unités dans le jeu
   
   Auteur: Christophe Auclair
   
   Collaborateurs: Matei Pelletier

*******************************************************************************/

public class HealthBar : NetworkBehaviour
{
    public Slider healthSlider;
    public Color lowColor, highColor;
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
