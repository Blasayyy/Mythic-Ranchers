using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoidBolt : MonoBehaviour
{
    [SerializeField]
    private float vitesse;
    private Rigidbody2D rig;
    private Animator anim;
    private float range;
    Vector3 initialPosition;

    void Start()
    {
        range = TalentTreeManager.instance.abilities[0].range;
        rig = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        Vector3 cursorPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        cursorPosition.z = 0;
        Vector3 direction = cursorPosition - transform.position;
        initialPosition = transform.position;
        direction.Normalize();

        if (direction.x < 0)
        {
            anim.SetFloat("Left", 1);
        }
        else
        {
            anim.SetFloat("Right", 1);
        }

        rig = GetComponent<Rigidbody2D>();
        rig.velocity = direction * 5.0f;
    }

    private void Update()
    {
        float distanceTravelled = Vector3.Distance(transform.position, initialPosition);

        if (distanceTravelled >= range)
        {
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Destroy(this.gameObject);
    }
}
