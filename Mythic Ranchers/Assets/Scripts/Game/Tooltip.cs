using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Unity.Netcode;
using TMPro;


public class Tooltip : NetworkBehaviour
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

        if (anchoredPosition.x + backgroundRect.rect.width > canvasRect.rect.width)
        //tooltip on the left side of the sreen
        {
            anchoredPosition.x = canvasRect.rect.width - backgroundRect.rect.width;
        }
        if (anchoredPosition.y + backgroundRect.rect.height > canvasRect.rect.height)
        //tooltip on the top of the sreen
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
