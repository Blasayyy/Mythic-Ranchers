using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class AbilityAoeTargeted : NetworkBehaviour
{
    private Ability ability;
    private float timer = 0;
    private Vector3 target, cursorWorldPosition, playerPosition, direction;

    public void Start()
    {
        cursorWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        cursorWorldPosition.z = 0;
        playerPosition = transform.position;

        direction = cursorWorldPosition - playerPosition;
        direction.Normalize();
        transform.localScale *= ability.aoeRange;

        // Check if the target position is within the range
        if (Mathf.Abs(Vector3.Distance(cursorWorldPosition, playerPosition)) <= ability.range)
        {
            target = cursorWorldPosition;
        }
        else
        {
            target = playerPosition + (direction * ability.range);
        }

        target.z = 0;
        this.transform.position = target;
        Destroy(gameObject, ability.duration);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<Enemy>())
        {
            collision.gameObject.GetComponent<Rigidbody2D>().WakeUp();
            timer += Time.deltaTime;
            if (timer >= ability.tick)
            {
                collision.gameObject.GetComponent<Enemy>().LoseHealth(ability.damage);
                timer = 0f;
            }
        }
    }

    public Ability Ability
    {
        get { return ability; }
        set { ability = value; }
    }
}
