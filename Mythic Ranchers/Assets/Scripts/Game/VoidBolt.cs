using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoidBolt : MonoBehaviour
{
    [SerializeField]
    private float vitesse;
    private Rigidbody2D rig;

    void Start()
    {
        Vector3 cursorPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 direction = cursorPosition - transform.position;
        direction.Normalize();
        rig = GetComponent<Rigidbody2D>();
        rig.velocity = direction * vitesse;
    }

    //OnCollision pour des objets durs qui se cognent
    //OnTrigger pour entrer/sortir de zones

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Destroy(this.gameObject);
    }
}
