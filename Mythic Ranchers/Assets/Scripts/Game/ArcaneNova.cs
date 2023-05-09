using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcaneNova : MonoBehaviour
{
    [SerializeField]
    private float duration;


    void Start()
    {
        Destroy(gameObject, duration);
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        //Destroy(this.gameObject);
    }
}
