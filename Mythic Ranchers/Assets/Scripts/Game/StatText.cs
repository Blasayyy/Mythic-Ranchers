using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class StatText : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField]
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
