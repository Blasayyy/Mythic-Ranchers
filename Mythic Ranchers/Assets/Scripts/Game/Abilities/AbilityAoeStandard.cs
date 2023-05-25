using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class AbilityAoeStandard : NetworkBehaviour
{
    [SerializeField]
    private Ability ability;

    public void Start()
    {
        transform.localScale *= ability.radius;
        Destroy(gameObject, ability.duration);
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<Enemy>())
        {
            collision.gameObject.GetComponent<Enemy>().LoseHealth(ability.damage);
        }
    }

    //public Ability Ability
    //{
    //    get { return ability; }
    //    set { ability = value; }
    //}
}
