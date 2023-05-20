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
    //private SpriteRenderer fonduRectangle;
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
    private int level;
    private string talents;
    private int talentPointsAvailable;
    private int xp;
    private List<EquipmentData> equipment;
    private string[] inventory;
    private string[] abilities;
    private Dictionary<string, int> stats; //dictionary
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
        //healthBar = GetComponentInChildren<Slider>();
        facingDirection = FacingDirection.Right;
        alive = true;
        control = true;
        facingRight = true;
        healthBar.SetHealth(currentHp, maxHp);
        ressourceBar.SetRessource(CurrentRessource, MaxRessource);
    }

    public void Update()
    {
        if (!IsOwner) return;
        //Debug.Log(control);
        CheckIfDead();
        GetInput();
        healthBar.SetHealth(CurrentHp, MaxHp);
        ressourceBar.SetRessource(CurrentRessource, MaxRessource);
    }

    public void FixedUpdate()
    {
        if (!IsOwner) return;
        Move();
    }

    public void TakeDamage(float damage)
    {
        CurrentHp -= damage;
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
                    if (itemInSlot.ability.cost <= CurrentRessource)
                    {
                        Vector3 target = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                        target.z = 0;
                        AbilityManager.instance.UseAbility(target, transform.position);
                        CurrentRessource -= itemInSlot.ability.cost;
                        Debug.Log(itemInSlot.ability.cost);
                        Debug.Log(CurrentRessource);
                    }

                }
                if (itemInSlot != null && itemInSlot.item)
                {
                    Item item = InventoryManager.instance.GetSelectedItem(true);
                }
            }
            if (Input.GetKeyUp(KeyCode.Space))
            {
                anim.SetTrigger("Attacking");
                //Instantiate(prefabVoidBolt, transform.position, Quaternion.identity);
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
            TakeDamage(damage);
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

    //IEnumerator FonduIn()
    //{
    //    Color colTemp = Color.black;
    //    fonduRectangle.color = colTemp;
    //    while (fonduRectangle.color.a > 0.0f)
    //    {
    //        colTemp.a -= taux;
    //        fonduRectangle.color = colTemp;
    //        yield return new WaitForEndOfFrame(); //� Chaque update
    //    }
    //    //Pour ne pas avoir de alpha negatif
    //    colTemp.a = 0.0f;
    //    fonduRectangle.color = colTemp;
    //}

    //IEnumerator FonduOut()
    //{
    //    Color colTemp = Color.black;
    //    colTemp.a = 0.0f;
    //    fonduRectangle.color = colTemp;
    //    while (fonduRectangle.color.a < 1f)
    //    {
    //        colTemp.a += taux;
    //        fonduRectangle.color = colTemp;
    //        yield return new WaitForEndOfFrame(); //� Chaque update
    //    }
    //    //Pour ne pas avoir de alpha plus que 1
    //    colTemp.a = 1f;
    //    fonduRectangle.color = colTemp;
    //}


}
