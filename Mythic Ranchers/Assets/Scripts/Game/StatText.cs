using UnityEngine;
using UnityEngine.EventSystems;

/*******************************************************************************

   Nom du fichier: StatText.cs
   
   Contexte: Cette classe représente un des character stats dans le UI pour le joueur
   
   Auteur: Christophe Auclair
   
   Collaborateurs: Matei Pelletier

*******************************************************************************/

public class StatText : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public string tooltipText = "";

    public void OnPointerEnter(PointerEventData eventData)
    {
        Tooltip.instance.ShowTooltip(tooltipText);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Tooltip.instance.HideTooltip();
    }
}
