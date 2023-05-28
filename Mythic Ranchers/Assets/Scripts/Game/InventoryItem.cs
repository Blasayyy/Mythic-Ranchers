using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

/*******************************************************************************

   Nom du fichier: InventoryItem.cs
   
   Contexte: Cette classe représente un item ou une ability dans un des inventory slots
   
   Auteur: Christophe Auclair
   
   Collaborateurs: Matei Pelletier

*******************************************************************************/

public class InventoryItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerEnterHandler, IPointerExitHandler
{
    [Header("UI")]
    public Image image;
    public TMP_Text countText;

    [SerializeField]
    private Image imageCooldown;
    [SerializeField]
    private TMP_Text textCooldown;
 
    [HideInInspector] public Transform parentAfterDrag, parentBeforeDrag;
    [HideInInspector] public int count = 1;
    [HideInInspector] public Item item;
    [HideInInspector] public Ability ability;
    [HideInInspector] public string tooltip;

    public bool isOnCooldown = false;
    private float cooldownTime = 0.0f;
    private float cooldownTimer = 0.0f;

    void Start()
    {
        if (this.ability)
        {
            textCooldown.gameObject.SetActive(false);
            imageCooldown.fillAmount = 0.0f;
            cooldownTime = ability.cooldown;
            tooltip = ability.tooltip;
        }
        else
        {
            textCooldown.gameObject.SetActive(false);
            imageCooldown.gameObject.SetActive(false);
            tooltip = item.tooltip;
        }
    }

    void Update()
    {
        if (isOnCooldown)
        {
            ApplyCooldown();
        }
    }

    public bool IsCastable()
    {
        if (isOnCooldown)
        {
            return false;
        }
        else
        {
            isOnCooldown = true;
            textCooldown.gameObject.SetActive(true);
            cooldownTimer = cooldownTime;
            return true;
        }
    }

    void ApplyCooldown()
    {
        cooldownTimer -= Time.deltaTime;

        if (cooldownTimer < 0.0f)
        {
            isOnCooldown = false;
            textCooldown.gameObject.SetActive(false);
            imageCooldown.fillAmount = 0.0f;
        }
        else
        {
            textCooldown.text = Mathf.RoundToInt(cooldownTimer).ToString();
            imageCooldown.fillAmount = cooldownTimer / cooldownTime;
        }
    }

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

    public void OnPointerEnter(PointerEventData eventData)
    {
        Tooltip.instance.ShowTooltip(tooltip);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Tooltip.instance.HideTooltip();
    }

    // drag and drop
    public void OnBeginDrag(PointerEventData eventData)
    {

        if (this.ability != null)
        {
            AbilityManager.instance.AddDuplicate(ability, transform.parent);
        }

        countText.raycastTarget = false;
        imageCooldown.raycastTarget = false;
        parentAfterDrag = transform.parent;
        parentBeforeDrag = transform.parent;
        transform.SetParent(transform.root);
        transform.SetAsLastSibling();
        image.raycastTarget = false;
        GearManager.instance.UpdateGear();
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
        imageCooldown.raycastTarget = true;

        if (this.ability != null && parentAfterDrag == parentBeforeDrag)
        {
            Destroy(this.gameObject);
        }
        GearManager.instance.UpdateGear();
    }
}
