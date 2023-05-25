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
        if (collision.GetContact(0).collider is BoxCollider2D)
        {
            if (ability.helpful && collision.gameObject.GetComponent<PlayerUnit>())
            {
                collision.gameObject.GetComponent<PlayerUnit>().GainHealth(ability.potency);
            }
            if (!ability.helpful && collision.gameObject.GetComponent<Enemy>())
            {
                collision.gameObject.GetComponent<Enemy>().LoseHealth(ability.potency);
            }
        }
    }
}
