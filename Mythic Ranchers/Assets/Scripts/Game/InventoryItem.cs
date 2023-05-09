using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using System.Linq;


public class InventoryItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{

    [Header("UI")]
    public Image image;
    public TMP_Text countText;
    public Image cooldown;

    [HideInInspector] public Transform parentAfterDrag, parentBeforeDrag;
    [HideInInspector] public int count = 1;
    [HideInInspector] public Item item;
    [HideInInspector] public Ability ability;



    public void InitializeItem(Item newItem)
    {
        item = newItem;
        image.sprite = newItem.image;
        RefreshCount();
    }

    public void InitializeAbility(Ability newAbility)
    {
        ability = newAbility;
        image.sprite = newAbility.image;
        RefreshCount();
    }

    public void RefreshCount()
    {
        countText.text = count.ToString();
        bool textActive = count > 1;
        countText.gameObject.SetActive(textActive);
    }

    // drag and drop
    public void OnBeginDrag(PointerEventData eventData)
    {

        if (this.ability != null)
        {
            Debug.Log("Ability");
            TalentTreeManager.instance.AddDuplicate(ability, transform.parent);
        }

        countText.raycastTarget = false;
        cooldown.raycastTarget = false;
        parentAfterDrag = transform.parent;
        parentBeforeDrag = transform.parent;
        transform.SetParent(transform.root);
        transform.SetAsLastSibling();
        image.raycastTarget = false;
        GearManager.instance.UpdateGearUI();


    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        countText.raycastTarget = true;
        transform.SetParent(parentAfterDrag);
        image.raycastTarget = true;
        cooldown.raycastTarget = true;

        if (this.ability != null && parentAfterDrag == parentBeforeDrag)
        {
            Destroy(this.gameObject);
        }
        GearManager.instance.UpdateGearUI();
    }
}
