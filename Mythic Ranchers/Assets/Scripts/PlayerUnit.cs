using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Threading.Tasks;

public class PlayerUnit : PlayerClass
{

    PlayerUnit player;



    public void AssignVaribles(object[] playerInfo)
    {
        player.PlayerName = (string)playerInfo[0];
        player.ClassName = (string)playerInfo[1];
        player.Position = (Vector2)playerInfo[2];
        player.MoveSpeed = (float)playerInfo[3];
        player.Hp = (float)playerInfo[4]; 
        player.BasicAtkDmg = (float)playerInfo[5];
        player.BasicAtkSpeed = (float)playerInfo[6];
        player.Ressource = (float)playerInfo[7];
        player.Level = (int)playerInfo[8];
        player.Talents = (string[])playerInfo[9];
        player.TalentPointsAvailable = (int)playerInfo[10];
        player.Xp = (int)playerInfo[11];
        player.Equipment = (string[])playerInfo[12];
        player.Inventory = (string[])playerInfo[13];
        player.Abilities = (string[])playerInfo[14];
        player.Stats = (string[])playerInfo[15];
        player.ArmorType = (ArmorTypes)playerInfo[16];
        player.KeyLevel = (int)playerInfo[17];
    }

    public object[] createVariables()
    {
        object[] playerInfo = new object[18];
        playerInfo[0] = "Whutz";
        playerInfo[1] = "Berzeker";
        playerInfo[2] = new Vector2(0, 0);
        playerInfo[3] = 2.0f;
        playerInfo[5] = 1.0f;
        playerInfo[6] = 1.0f;
        playerInfo[7] = 1.0f;
        playerInfo[8] = 1;
        playerInfo[9] = new string[] { "talent1", "talent2", "talent3" };
        playerInfo[10] = 0;
        playerInfo[11] = 0;
        playerInfo[12] = new string[] { "equipment1", "equipment2", "equipment3" };
        playerInfo[13] = new string[] { "inventory1", "inventory2", "inventory3" };
        playerInfo[14] = new string[] { "ability1", "ability2", "ability3" };
        playerInfo[15] = new string[] { "stat1", "stat2", "stat3" };
        playerInfo[16] = ArmorTypes.Mail;
        playerInfo[17] = 1;

        return playerInfo;
    }




    //private Rigidbody2D rig;
    //private Vector2 movement;
    //private Animator anim;
    //private FacingDirection lastFacingDirection;
    //private SpriteRenderer fonduRectangle;
    //private bool alive = true;
    ////public bool control = true;
    //private bool facingRight = true;

    //[SerializeField]
    //public float speed;
    //[SerializeField]
    //private float taux = 0.001f;


    //public enum FacingDirection
    //{
    //    Left,
    //    Right,
    //    Down,
    //    Up
    //}
    // Start is called before the first frame update
    void Start()
    {
        base.Start();
        //rig = GetComponent<Rigidbody2D>();
        //anim = GetComponent<Animator>();
        //lastFacingDirection = FacingDirection.Right;
    }

    private void Update()
    {
        base.Update();
        Debug.Log(this.BasicAtkDmg);
    }

    private void FixedUpdate()
    {
        base.FixedUpdate();
    }

    // Update is called once per frame
    //void Update()
    //{
    //    base.Update();
    //    //if (GetHP() <= 0)
    //    //{
    //    //    alive = false;
    //    //    anim.SetBool("Alive", false);
    //    //}

    //    //if (!IsOwner) return;
    //    //if (control && alive)
    //    //{
    //    //    movement.x = Input.GetAxisRaw("Horizontal");
    //    //    movement.y = Input.GetAxisRaw("Vertical");
    //    //    if (Input.GetKey(KeyCode.Space))
    //    //    {
    //    //        anim.SetTrigger("Attacking");
    //    //    }
    //    //    else if (Input.GetKey(KeyCode.X))
    //    //    {
    //    //        anim.SetTrigger("Startled");
    //    //    }

    //    //    anim.SetFloat("Vertical", movement.y);
    //    //    anim.SetFloat("Horizontal", movement.x);

    //    //    if (movement.x > 0 && !facingRight)
    //    //    {
    //    //        lastFacingDirection = FacingDirection.Right;
    //    //        Flip();
    //    //    }
    //    //    else if (movement.x < 0 && facingRight)
    //    //    {
    //    //        lastFacingDirection = FacingDirection.Left;
    //    //        Flip();
    //    //    }
    //    //}
    //    //else
    //    //{
    //    //    movement.x = 0;
    //    //    movement.y = 0;
    //    //}

    //    //anim.SetFloat("Left", lastFacingDirection == FacingDirection.Left ? 1 : 0);
    //    //anim.SetFloat("Right", lastFacingDirection == FacingDirection.Right ? 1 : 0);

    //}

    //void Flip()
    //{
    //    Vector3 currentScale = gameObject.transform.localScale;
    //    currentScale.x *= -1;
    //    gameObject.transform.localScale = currentScale;

    //    facingRight = !facingRight;
    //}

    //private void FixedUpdate()
    //{
    //    base.FixedUpdate();
    //    //rig.velocity = movement * speed;
    //    //rig.velocity = rig.velocity.normalized * speed;
    //}

    //private async void OnCollisionEnter2D(Collision2D collision)
    //{
    //    if (LayerMask.LayerToName(collision.gameObject.layer) == "Ennemies" && control)
    //    {
    //        float damage = 1;
    //        TakeDamage(damage);
    //        Debug.Log(GetHP());
    //    }
    //}

    //private async void OnTriggerEnter2D(Collider2D collision)
    //{
    //    if (LayerMask.LayerToName(collision.gameObject.layer) == "Oeuf")
    //    {
    //        alive = false;
    //        StartCoroutine(FonduOut());
    //        await Task.Delay(1000);
    //        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    //    }
    //}

}
