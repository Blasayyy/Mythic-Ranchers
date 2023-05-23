using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Unity.Netcode;

public class PlayerClass : NetworkBehaviour
{
    public HealthBar healthBar;
    public RessourceBar ressourceBar;
    private Rigidbody2D rig;
    private Vector2 movement;
    private Animator anim;
    private FacingDirection facingDirection;
    private bool alive;
    public bool control;
    private bool facingRight;

    private string playerName;
    private string className;
    //private Controller controller; ??
    private Vector3 position;
    private float moveSpeed;
    private float maxHp;
    private float currentHp;
    private float basicAtkDmg;
    private float basicAtkSpeed;
    private float currentRessource;
    private float maxRessource;
    private string ressourceType;
    private int level;
    private string talents;
    private int talentPointsAvailable;
    private int xp;
    private List<EquipmentData> equipment;
    private string[] inventory;
    private string[] abilities;
    private Dictionary<string, int> stats; //dictionary
    private Dictionary<string, int> initialStats; //dictionary
    private ArmorType armorType;
    private int keyLevel;

    public enum FacingDirection
    {
        Left,
        Right,
        Down,
        Up
    }

    public void Start()
    {
        rig = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        facingDirection = FacingDirection.Right;
        alive = true;
        control = true;
        facingRight = true;
        healthBar.SetHealth(currentHp, maxHp);
        ressourceBar.SetRessource(CurrentRessource, MaxRessource);
    }

    public void Update()
    {
        if (IsOwner)
        {
            
            CheckIfDead();
            GetInput();
            healthBar.SetHealth(CurrentHp, MaxHp);
            ressourceBar.SetRessource(CurrentRessource, MaxRessource);
        }
        
    }

    public void FixedUpdate()
    {
        if (IsOwner)
        {
            Move();
        }
    }

    public void LoseHealth(float amount)
    {
        CurrentHp -= amount;
    }

    public void LoseRessource(float amount)
    {
        CurrentRessource -= amount;
    }

    public void GetHealed(float hp)
    {
        float healthAfterHeal = CurrentHp + hp;
        if (healthAfterHeal > MaxHp)
        {
            CurrentHp = MaxHp;
        }
        else
        {
            CurrentHp = CurrentHp + hp;
        }
    }

    public void GainRessource(float gain)
    {
        float ressourceAfterGain = CurrentRessource + gain;
        if (ressourceAfterGain > MaxRessource)
        {
            CurrentRessource = MaxRessource;
        }
        else
        {
            CurrentRessource = CurrentRessource + gain;
        }
    }


    private void GetInput()
    {
        if (control && alive)
        {
            movement.x = Input.GetAxisRaw("Horizontal");
            movement.y = Input.GetAxisRaw("Vertical");
            anim.ResetTrigger("Attacking");
            anim.ResetTrigger("Startled");
            if (Input.GetMouseButtonDown(1))
            {
                InventorySlot slot = InventoryManager.instance.inventorySlots[InventoryManager.instance.selectedSlot];
                InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();
                if (itemInSlot != null && itemInSlot.ability)
                {
                    if (itemInSlot.ability.cost <= CurrentRessource && !itemInSlot.isOnCooldown)
                    {
                        anim.SetTrigger("Attacking");
                        Vector3 target = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0f));
                        AbilityManager.instance.UseAbility(target, transform.position);                        
                        LoseRessource(itemInSlot.ability.cost);
                    }

                }
                if (itemInSlot != null && itemInSlot.item)
                {
                    Item item = InventoryManager.instance.GetSelectedItem(itemInSlot.item.useable);
                }
            }
            if (Input.GetKeyUp(KeyCode.Space))
            {
                anim.SetTrigger("Attacking");
            }
            else if (Input.GetKeyUp(KeyCode.X))
            {
                anim.SetTrigger("Startled");
            }

            anim.SetFloat("Vertical", movement.y);
            anim.SetFloat("Horizontal", movement.x);

            if (movement.x > 0)
            {
                facingDirection = FacingDirection.Right;
                Flip(0);
            }
            else if (movement.x < 0)
            {
                facingDirection = FacingDirection.Left;
                Flip(1);
            }
            else if (movement.y > 0)
            {
                facingDirection = FacingDirection.Up;
            }
            else if (movement.y < 0)
            {
                facingDirection = FacingDirection.Down;
            }

            anim.SetFloat("Up", facingDirection == FacingDirection.Up ? 1 : 0);
            anim.SetFloat("Down", facingDirection == FacingDirection.Down ? 1 : 0);
            anim.SetFloat("Left", facingDirection == FacingDirection.Left ? 1 : 0);
            anim.SetFloat("Right", facingDirection == FacingDirection.Right ? 1 : 0);
            anim.SetFloat("FacingRight", facingRight == true ? 1 : 0);
            anim.SetFloat("FacingLeft", facingRight == false ? 1 : 0);
        }
        else
        {
            movement.x = 0;
            movement.y = 0;
        }
    }

    private void Move()
    {
        rig.velocity = movement * moveSpeed;
        rig.velocity = rig.velocity.normalized * moveSpeed;
        this.Position = transform.position;
    }

    private void Flip(int side)
    {
        if (side == 0 && !facingRight)
        {
            facingRight = true;
        }
        else if (side == 1 && facingRight)
        {
            facingRight = false;
        }
    }

    private void CheckIfDead()
    {
        if (CurrentHp <= 0)
        {
            alive = false;
            anim.SetBool("Alive", false);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (LayerMask.LayerToName(collision.gameObject.layer) == "Ennemies" && control)
        {
            Debug.Log(currentHp);
            float damage = 1;
            LoseHealth(damage);
            healthBar.SetHealth(CurrentHp, MaxHp);

        }
        if (LayerMask.LayerToName(collision.gameObject.layer) == "Items" && control)
        {
            //bool canAdd = InventoryManager.instance.AddItem()
            Debug.Log("Collision with pot");
        }
    }

    public void SetControllOff()
    {
        this.control = false;
    } 
    
    public void SetControllOn()
    {
        this.control = false;
    }

    public string PlayerName
    {
        get { return playerName; }
        set { playerName = value; }
    }

    public string ClassName
    {
        get { return className; }
        set { className = value; }
    }

    public Vector2 Position
    {
        get { return position; }
        set { position = value; }
    }

    public float MoveSpeed
    {
        get { return moveSpeed; }
        set { moveSpeed = value; }
    }

    public float MaxHp
    {
        get { return maxHp; }
        set { maxHp = value; }
    }

    public float CurrentHp
    {
        get { return currentHp; }
        set { currentHp = value; }
    }

    public float BasicAtkDmg
    {
        get { return basicAtkDmg; }
        set { basicAtkDmg = value; }
    }

    public float BasicAtkSpeed
    {
        get { return basicAtkSpeed; }
        set { basicAtkSpeed = value; }
    }

    public float CurrentRessource
    {
        get { return currentRessource; }
        set { currentRessource = value; }
    }

    public float MaxRessource
    {
        get { return maxRessource; }
        set { maxRessource = value; }
    }

    public string RessourceType
    {
        get { return ressourceType; }
        set { ressourceType = value; }
    }

    public int Level
    {
        get { return level; }
        set { level = value; }
    }

    public string Talents
    {
        get { return talents; }
        set { talents = value; }
    }

    public int TalentPointsAvailable
    {
        get { return talentPointsAvailable; }
        set { talentPointsAvailable = value; }
    }

    public int Xp
    {
        get { return xp; }
        set { xp = value; }
    }

    public List<EquipmentData> Equipment
    {
        get { return equipment; }
        set { equipment = value; }
    }

    public string[] Inventory
    {
        get { return inventory; }
        set { inventory = value; }
    }

    public string[] Abilities
    {
        get { return abilities; }
        set { abilities = value; }
    }

    public Dictionary<string, int> Stats
    {
        get { return stats; }
        set { stats = value; }
    }

    public Dictionary<string, int> InitialStats
    {
        get { return initialStats; }
        set { initialStats = value; }
    }

    public ArmorType ArmorType
    {
        get { return armorType; }
        set { armorType = value; }
    }

    public int KeyLevel
    {
        get { return keyLevel; }
        set { keyLevel = value; }
    }
}
