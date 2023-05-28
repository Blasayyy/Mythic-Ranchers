using UnityEngine;
using TMPro;

/*******************************************************************************

   Nom du fichier: Tooltip.cs
   
   Contexte: Cette classe représente le text que le joueur voit quand il hover un
             item/stat/ability
   
   Auteur: Christophe Auclair
   
   Collaborateurs: Matei Pelletier

*******************************************************************************/

public class Tooltip : MonoBehaviour
{
    [SerializeField]
    private RectTransform canvasRect;
    private TextMeshProUGUI tooltipTMP;
    private RectTransform backgroundRect, rect;
    public static Tooltip instance;

    private void Awake()
    {
        instance = this;
        backgroundRect = transform.Find("Background").GetComponent<RectTransform>();
        tooltipTMP = transform.Find("Text").GetComponent<TextMeshProUGUI>();
        rect = transform.GetComponent<RectTransform>();

        HideTooltip();
    }

    private void SetText(string tooltipText)
    {
        tooltipTMP.SetText(tooltipText);
        tooltipTMP.ForceMeshUpdate();

        Vector2 textSize = tooltipTMP.GetRenderedValues(false);
        Vector2 paddingSize = new Vector2(8f, 8f);
        backgroundRect.sizeDelta = textSize + paddingSize;
    }

    private void Update()
    {
        Vector2 anchoredPosition = Input.mousePosition / canvasRect.localScale.x;

        //tooltip on the far right side of the sreen
        if (anchoredPosition.x + backgroundRect.rect.width > canvasRect.rect.width)
        {
            anchoredPosition.x = canvasRect.rect.width - backgroundRect.rect.width;
        }
        //tooltip on the top of the sreen
        if (anchoredPosition.y + backgroundRect.rect.height > canvasRect.rect.height)        
        {
            anchoredPosition.y = canvasRect.rect.height - backgroundRect.rect.height;
        }

        rect.anchoredPosition = anchoredPosition;
    }

    public void ShowTooltip(string tooltipText)
    {
        gameObject.SetActive(true);
        SetText(tooltipText);
    }

    public void HideTooltip()
    {
        gameObject.SetActive(false);
    }
}
