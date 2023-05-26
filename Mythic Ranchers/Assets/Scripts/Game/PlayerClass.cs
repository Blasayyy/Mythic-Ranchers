using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Unity.Netcode;
using System.Threading.Tasks;

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
    public bool invulnerable;
    private bool facingRight;
    public float flickerDuration = 0.1f;
    public int flickerCount = 5;
    private SpriteRenderer spriteRenderer;
    private int frameCount;
    private bool slowed;

    private string playerName;
    private string className;
    private Vector3 position;
    private float moveSpeed;
    private float initialMoveSpeed;
    public NetworkVariable<float> currentHp;
    public NetworkVariable<float> maxHp;
    public NetworkVariable<float> currentRessource;
    public NetworkVariable<float> maxRessource;
    private float basicAtkDmg;
    private float basicAtkSpeed;
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
        spriteRenderer = GetComponent<SpriteRenderer>();
        facingDirection = FacingDirection.Right;
        alive = true;
        control = true;
        facingRight = true;
        healthBar.SetHealth(CurrentHp, MaxHp);
        ressourceBar.SetRessource(CurrentRessource, MaxRessource, RessourceType);
    }

    public void Update()
    {
        if (IsOwner)
        {            
            CheckIfDead();
            GetInput();            
        }
        healthBar.SetHealth(CurrentHp, MaxHp);
        ressourceBar.SetRessource(CurrentRessource, MaxRessource, RessourceType);
    }

    public void FixedUpdate()
    {
        if (IsOwner)
        {
            Move();
            if (RegenTick())
            {
                Regeneration();
            }
        }
    }

    public void LoseHealth(float amount)
    {
        CurrentHp -= amount * (1 - 0.01f * stats["armor"]);
    }

    public void LoseRessource(float amount)
    {
        CurrentRessource -= amount;
    }

    public void GainHealth(float hp)
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

    public bool RegenTick()
    {
        frameCount += 1;

        if (frameCount >= 120)
        {
            frameCount = 0;
            return true;
        }

        return false;
    }

    public void Regeneration()
    {
        GainHealth(Stats["stamina"] * 0.1f);
        if (ClassName == "Necromancer" || ClassName == "Mage")
        {
            GainRessource(Stats["intellect"] * 1f);
        }
        else if (ClassName == "Berzerker")
        {
            GainRessource(Stats["intellect"] * 0.4f + 6f);
        }
    }

    public void AnimationTrigger(AbilityType abilityType)
    {
        if (ClassName == "Berzerker")
        {
            anim.SetTrigger("Attacking");
        }
        else if (ClassName == "Mage")
        {
            anim.SetTrigger("Cast");
        }
        else if (ClassName == "Necromancer")
        {
            switch (abilityType)
            {
                case AbilityType.Projectile:
                    anim.SetTrigger("Cast2");
                    break;
                case AbilityType.AoeStandard:
                    anim.SetTrigger("Cast1");
                    break;
                case AbilityType.AoeTargeted:
                    anim.SetTrigger("Cast1");
                    break;
                case AbilityType.Frontal:
                    anim.SetTrigger("Cast1");
                    break;
            }
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
                        AnimationTrigger(itemInSlot.ability.type);                        
                        AbilityManager.instance.UseAbility(transform.position);                        
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
            else if (Input.GetKeyUp(KeyCode.X) && ClassName == "berzerker")
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

    private async void CheckIfDead()
    {
        if (!alive)
        {
            return;
        }

        if (CurrentHp <= 0)
        {
            alive = false;
            anim.SetBool("Alive", false);
            rig.constraints = RigidbodyConstraints2D.FreezeAll;
            await Task.Delay(3000);
            transform.position = MythicGameManager.Instance.mapData.Item1[0].center;
            alive = true;
            control = true;
            CurrentHp = MaxHp / 2;
            CurrentRessource = MaxRessource / 2;
            MoveSpeed = InitialMoveSpeed;
            rig.constraints = RigidbodyConstraints2D.FreezeRotation;
            StopAllCoroutines();
            MythicGameManagerMultiplayer.Instance.DeathsCount.Value += 1;
            MythicGameManagerMultiplayer.Instance.TimerCount.Value -= 10;
            anim.SetBool("Alive", true);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (LayerMask.LayerToName(collision.gameObject.layer) == "Ennemies" && control && !invulnerable)
        {
            float damage = 15;
            LoseHealth(damage);
            StartCoroutine(Slowed(2f, 0.25f));
            if (!slowed)
            {
                StartCoroutine(DamageFlicker());
            }

        }
        if (LayerMask.LayerToName(collision.gameObject.layer) == "Items" && control)
        {
            //bool canAdd = InventoryManager.instance.AddItem()
            Debug.Log("Collision with pot");
        }
    }

    private IEnumerator DamageFlicker()
    {
        invulnerable = true;
        for (int i = 0; i < flickerCount; i++)
        {
            spriteRenderer.color = Color.red;
            yield return new WaitForSeconds(flickerDuration);

            spriteRenderer.color = Color.white;
            yield return new WaitForSeconds(flickerDuration);
        }
        invulnerable = false;
    }

    private IEnumerator Slowed(float slowDuration, float slowAmount)
    {
        slowed = true;
        float normalSpeed = MoveSpeed;
        if (slowAmount == 1f)
        {
            rig.constraints = RigidbodyConstraints2D.FreezeAll;
        }
        else
        {
            MoveSpeed *=  1 - slowAmount;
        }
        if (ClassName == "Berzerker")
        {
            spriteRenderer.color = new Color(0, 150, 255, 255);
        }
        else if (ClassName == "Necromancer")
        {
            spriteRenderer.color = new Color(75, 255, 255, 255);
        }
        yield return new WaitForSeconds(slowDuration);
        spriteRenderer.color = Color.white;
        if (slowAmount == 1f)
        {
            rig.constraints = RigidbodyConstraints2D.None;
            rig.constraints = RigidbodyConstraints2D.FreezeRotation;
        }
        else
        {                        
            MoveSpeed = normalSpeed;
        }
        slowed = false;
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

    public float InitialMoveSpeed
    {
        get { return initialMoveSpeed; }
        set { initialMoveSpeed = value; }
    }

    public float MaxHp
    {
        get { return maxHp.Value; }
        set { maxHp.Value = value; }
    }

    public float CurrentHp
    {
        get { return currentHp.Value; }
        set { currentHp.Value = value; }
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
        get { return currentRessource.Value; }
        set { currentRessource.Value = value; }
    }

    public float MaxRessource
    {
        get { return maxRessource.Value; }
        set { maxRessource.Value = value; }
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
