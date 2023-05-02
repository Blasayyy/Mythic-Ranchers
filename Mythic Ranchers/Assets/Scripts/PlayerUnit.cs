using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Threading.Tasks;

public class PlayerUnit : PlayerClass
{
    private Rigidbody2D rig;
    private Vector2 movement;
    private Animator anim;
    private FacingDirection lastFacingDirection;
    private SpriteRenderer fonduRectangle;
    private bool alive = true;
    public bool control = true;
    private bool facingRight = true;

    [SerializeField]
    public float speed;
    [SerializeField]
    private float taux = 0.001f;


    public enum FacingDirection
    {
        Left,
        Right
    }
    // Start is called before the first frame update
    void Start()
    {
        rig = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        lastFacingDirection = FacingDirection.Right;
    }

    // Update is called once per frame
    void Update()
    {
        if (GetHP() <= 0)
        {
            alive = false;
            anim.SetBool("Alive", false);
        }

        if (!IsOwner) return;
        if (control && alive)
        {
            movement.x = Input.GetAxisRaw("Horizontal");
            movement.y = Input.GetAxisRaw("Vertical");
            if (Input.GetKey(KeyCode.Space))
            {
                anim.SetTrigger("Attacking");
            }
            else if (Input.GetKey(KeyCode.X))
            {
                anim.SetTrigger("Startled");
            }

            anim.SetFloat("Vertical", movement.y);
            anim.SetFloat("Horizontal", movement.x);
        
            if (movement.x > 0 && !facingRight)
            {
                lastFacingDirection = FacingDirection.Right;
                Flip();
            }
            else if (movement.x < 0 && facingRight)
            {
                lastFacingDirection = FacingDirection.Left;
                Flip();
            }
        }
        else
        {
            movement.x = 0;
            movement.y = 0;
        }

        anim.SetFloat("Left", lastFacingDirection == FacingDirection.Left ? 1 : 0);
        anim.SetFloat("Right", lastFacingDirection == FacingDirection.Right ? 1 : 0);

    }

    void Flip()
    {
        Vector3 currentScale = gameObject.transform.localScale;
        currentScale.x *= -1;
        gameObject.transform.localScale = currentScale;

        facingRight = !facingRight;
    }

    private void FixedUpdate()
    {
        rig.velocity = movement * speed;
        rig.velocity = rig.velocity.normalized * speed;
    }

    private async void OnCollisionEnter2D(Collision2D collision)
    {
        if (LayerMask.LayerToName(collision.gameObject.layer) == "Ennemies" && control)
        {
            float damage = 1;
            TakeDamage(damage);
            Debug.Log(GetHP());
        }
    }

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


    IEnumerator FonduIn()
    {
        Color colTemp = Color.black;
        fonduRectangle.color = colTemp;
        while (fonduRectangle.color.a > 0.0f)
        {
            colTemp.a -= taux;
            fonduRectangle.color = colTemp;
            yield return new WaitForEndOfFrame(); //� Chaque update
        }
        //Pour ne pas avoir de alpha n�gatif
        colTemp.a = 0.0f;
        fonduRectangle.color = colTemp;
    }

    IEnumerator FonduOut()
    {
        Color colTemp = Color.black;
        colTemp.a = 0.0f;
        fonduRectangle.color = colTemp;
        while (fonduRectangle.color.a < 1f)
        {
            colTemp.a += taux;
            fonduRectangle.color = colTemp;
            yield return new WaitForEndOfFrame(); //� Chaque update
        }
        //Pour ne pas avoir de alpha plus que 1
        colTemp.a = 1f;
        fonduRectangle.color = colTemp;
    }
}
