using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class AbilityProjectile : NetworkBehaviour
{
    [SerializeField]
    private Ability ability;
    private Rigidbody2D rig;
    Vector3 initialPosition;

    public void Start()
    {
        rig = GetComponent<Rigidbody2D>();
        Vector3 cursorPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        cursorPosition.z = 0;
        Vector3 direction = (cursorPosition - transform.position).normalized;
        initialPosition = transform.position;
        transform.localScale *= ability.radius;
        rig.velocity = direction * 5.0f;

        var angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(Vector3.forward * angle);
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
        if (ability.helpful && collision.gameObject.GetComponent<PlayerUnit>())
        {
            collision.gameObject.GetComponent<PlayerUnit>().GainHealth(ability.potency);
        }
        if (!ability.helpful && collision.gameObject.GetComponent<Enemy>())
        {
            collision.gameObject.GetComponent<Enemy>().LoseHealth(ability.potency);
        }
        Destroy(this.gameObject);
    }
}
