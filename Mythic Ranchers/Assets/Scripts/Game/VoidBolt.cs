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
        rig = GetComponent<Rigidbody2D>();
        rig.velocity = new Vector2(1.0f, 0.0f) * vitesse;
    }

    //OnCollision pour des objets durs qui se cognent
    //OnTrigger pour entrer/sortir de zones

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Destroy(this.gameObject);
    }
}
