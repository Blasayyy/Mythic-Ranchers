using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class PlayerClass : NetworkBehaviour
{
    private Rigidbody2D rig;
    private Vector2 movement;
    private Animator anim;
    private FacingDirection facingDirection;
    //private SpriteRenderer fonduRectangle;
    private bool alive = true;
    public bool control = true;
    private bool facingRight = true;

    private string playerName;
    private string className;
    //private Controller controller; ??
    private Vector2 position;
    private float moveSpeed;
    private float hp;
    private float basicAtkDmg;
    private float basicAtkSpeed;
    private float ressource;
    private int level;
    private string[] talents;
    private int talentPointsAvailable;
    private int xp;
    private string[] equipment;
    private string[] inventory;
    private string[] abilities;
    private string[] stats; //dictionary
    private ArmorType armorType;
    private int keyLevel;

    [SerializeField]
    private Transform prefabVoidBolt;

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
        //moveSpeed = 2;
        //Hp = 5;
    }

    public void Update()
    {
        if (!IsOwner) return;
        //Debug.Log(control);
        CheckIfDead();
        GetInput();
    }

    public void FixedUpdate()
    {
        if (!IsOwner) return;
        Move();
    }

    public void TakeDamage(float damage)
    {
        Hp -= damage;
    }


    private void GetInput()
    {
        if (control && alive)
        {
            movement.x = Input.GetAxisRaw("Horizontal");
            movement.y = Input.GetAxisRaw("Vertical");
            anim.ResetTrigger("Attacking");
            anim.ResetTrigger("Startled");
            if (Input.GetMouseButtonDown(0))
            {
                InventorySlot slot = InventoryManager.instance.inventorySlots[InventoryManager.instance.selectedSlot];
                InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();
                if (itemInSlot != null && itemInSlot.ability)
                {
                    Vector3 target = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    target.z = 0;
                    InventoryManager.instance.UseAbility(target, transform.position);
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
        }
        else
        {
            movement.x = 0;
            movement.y = 0;
        }

        anim.SetFloat("Up", facingDirection == FacingDirection.Up ? 1 : 0);
        anim.SetFloat("Down", facingDirection == FacingDirection.Down ? 1 : 0);
        anim.SetFloat("Left", facingDirection == FacingDirection.Left ? 1 : 0);
        anim.SetFloat("Right", facingDirection == FacingDirection.Right ? 1 : 0);
        anim.SetFloat("FacingRight", facingRight == true ? 1 : 0);
        anim.SetFloat("FacingLeft", facingRight == false ? 1 : 0);
    }

    private void Move()
    {
        rig.velocity = movement * moveSpeed;
        rig.velocity = rig.velocity.normalized * moveSpeed;
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
        if (Hp <= 0)
        {
            alive = false;
            anim.SetBool("Alive", false);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (LayerMask.LayerToName(collision.gameObject.layer) == "Ennemies" && control)
        {
            float damage = 1;
            TakeDamage(damage);
            Debug.Log(Hp);
        }
        if (LayerMask.LayerToName(collision.gameObject.layer) == "Items" && control)
        {
            //bool canAdd = InventoryManager.instance.AddItem()
            Debug.Log("Collision with pot");
        }
    }

    public void ControllToggle()
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

    public float Hp
    {
        get { return hp; }
        set { hp = value; }
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

    public float Ressource
    {
        get { return ressource; }
        set { ressource = value; }
    }

    public int Level
    {
        get { return level; }
        set { level = value; }
    }

    public string[] Talents
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

    public string[] Equipment
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

    public string[] Stats
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
