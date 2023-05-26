using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class AbilityProjectile : NetworkBehaviour
{
    [SerializeField]
    private Ability ability;
    private Rigidbody2D rig;
    Vector3 initialPosition, cursorPosition;
    private bool isInitialized = false;

    public void Start()
    {
        rig = GetComponent<Rigidbody2D>();
        transform.localScale *= ability.radius;
    }

    public void InitializeProjectile()
    {
        if (!isInitialized)
        {
            cursorPosition.z = 0;
            Vector3 direction = (cursorPosition - initialPosition).normalized;
            rig.velocity = direction * 5.0f;

            var angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            if (ability.name == "Burning Bolt")
                transform.rotation = Quaternion.Euler(Vector3.forward * (angle + 270));
            else
                transform.rotation = Quaternion.Euler(Vector3.forward * angle);
            isInitialized = true;
        }
    }

    private void Update()
    {
        // Call InitializeProjectile if it wasn't initialized yet
        if (!isInitialized)
        {
            InitializeProjectile();
        }

        float distanceTravelled = Vector3.Distance(transform.position, initialPosition);

        if (distanceTravelled >= ability.range)
        {
            Destroy(gameObject);
        }
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
                if (ability.slow)
                {
                    collision.gameObject.GetComponent<Enemy>().GetSlowed(ability.slowDuration, ability.slowAmount);
                }
                collision.gameObject.GetComponent<Enemy>().LoseHealth(ability.potency);
            }
        }
        Destroy(this.gameObject);
    }

    public void SetCursorPos(Vector3 cursorPos)
    {
        cursorPosition = cursorPos;
    }

    public void SetInitialPosition(Vector3 position)
    {
        initialPosition = position;
    }
}
