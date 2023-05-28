using UnityEngine;
using Unity.Netcode;
using UnityEngine.UI;

/*******************************************************************************

   Nom du fichier: Ressource.cs
   
   Contexte: Cette classe sert à informer le joueur sur le nombre de ressources
             restantes aux unités dans le jeu
   
   Auteur: Christophe Auclair
   
   Collaborateurs: Matei Pelletier

*******************************************************************************/

public class RessourceBar : NetworkBehaviour
{
    public Slider ressourceSlider;
    public Color manaColor, energyColor;
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
}
