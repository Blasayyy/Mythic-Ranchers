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
        if (ability.helpful && collision.gameObject.GetComponent<PlayerUnit>())
        {
            collision.gameObject.GetComponent<PlayerUnit>().GainHealth(ability.potency);
        }
        if (!ability.helpful && collision.gameObject.GetComponent<Enemy>())
        {
            if (ability.slow)
            {
                collision.gameObject.GetComponent<Enemy>().GetSlowed(ability.slowDuration, ability.slowAmount);
            }
            collision.gameObject.GetComponent<Enemy>().LoseHealth(ability.potency);
        }
    }
}
