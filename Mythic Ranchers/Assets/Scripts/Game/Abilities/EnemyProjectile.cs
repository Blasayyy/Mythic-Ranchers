using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class AbilityProjectile : NetworkBehaviour
{
    [SerializeField]
    private Ability ability;
    private Rigidbody2D rig;
    private Animator anim;
    Vector3 initialPosition;

    public void Start()
    {
        rig = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        Vector3 cursorPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        cursorPosition.z = 0;
        Vector3 direction = cursorPosition - transform.position;
        direction.Normalize();
        initialPosition = transform.position;
        rig.velocity = direction * 5.0f;

        if (direction.x < 0)
        {
            anim.SetFloat("Left", 1);
        }
        else
        {
            anim.SetFloat("Right", 1);
        }
    }

    private void Update()
    {
        float distanceTravelled = Vector3.Distance(transform.position, initialPosition);

        if (distanceTravelled >= ability.range)
        {
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<Enemy>())
        {
            collision.gameObject.GetComponent<Enemy>().LoseHealth(ability.damage);
        }
        Destroy(this.gameObject);
    }

    public Ability Ability
    {
        get { return ability; }
        set { ability = value; }
    }
}
