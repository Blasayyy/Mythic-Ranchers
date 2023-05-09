using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FelBomb : MonoBehaviour
{
    [SerializeField]
    private float duration;
    private Rigidbody2D rig;
    private Animator anim;

    void Start()
    {
        Destroy(gameObject, duration);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //Destroy(this.gameObject);
    }
}
