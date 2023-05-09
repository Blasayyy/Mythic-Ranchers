using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SpellCooldown : MonoBehaviour
{
    public static SpellCooldown instance;

    [SerializeField]
    private Image imageCooldown;
    [SerializeField]
    private TMP_Text textCooldown;

    public GameObject felBombPrefab;
    public GameObject voidboltPrefab;

    private bool isCooldown = false;
    private float cooldownTime = 0.0f;
    private float cooldownTimer = 0.0f;


    // Start is called before the first frame update
    void Start()
    {
        textCooldown.gameObject.SetActive(false);
        imageCooldown.fillAmount = 0.0f;
    }

    private void Awake()
    {
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        ApplyCooldown();
    }

    public void UseSpell()
    {
        if (isCooldown)
        {
            // sound effect? spell is on cd
        }
        else
        {
            isCooldown = true;
            textCooldown.gameObject.SetActive(true);
            cooldownTimer = cooldownTime;
        }
    }

    void ApplyCooldown()
    {
        // substract time since last called
        cooldownTimer -= Time.deltaTime;

        if (cooldownTimer < 0.0f)
        {
            isCooldown = false;
            textCooldown.gameObject.SetActive(false);
            imageCooldown.fillAmount = 0.0f;
        }
        else
        {
            textCooldown.text = Mathf.RoundToInt(cooldownTimer).ToString();
            imageCooldown.fillAmount = cooldownTimer / cooldownTime;
        }
    }

    public bool UseAbility(Vector3 target, Vector3 playerPos)
    {
        InventorySlot slot = InventoryManager.instance.inventorySlots[InventoryManager.instance.selectedSlot];
        InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();

        if (itemInSlot.ability)
        {
            UseSpell();
            if (itemInSlot.ability.type == AbilityType.AoeTargetted)
            {
                Instantiate(felBombPrefab, target, Quaternion.identity);
                return true;
            }
            else if (itemInSlot.ability.type == AbilityType.Projectile)
            {
                Instantiate(voidboltPrefab, playerPos, Quaternion.identity);
                return true;
            }
            return false;
        }
        else
        {
            return false;
        }
    }
}
