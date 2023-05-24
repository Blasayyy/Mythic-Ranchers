using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class FelBomb : NetworkBehaviour
{
    [SerializeField]
    private float duration;
    private float range;
    private float damage = 2f;
    private float damageInterval = 1f;
    private float timer;
    private Vector3 target, cursorWorldPosition, playerPosition, direction;

    void Start()
    { 
        // hardcoded until we link to db
        range = AbilityManager.instance.abilities[1].range;

        cursorWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        cursorWorldPosition.z = 0;
        playerPosition = transform.position;

        direction = cursorWorldPosition - playerPosition;
        direction.Normalize();

        // Check if the target position is within the range
        if (Mathf.Abs(Vector3.Distance(cursorWorldPosition, playerPosition)) <= range)
        {
            target = cursorWorldPosition;         
        }
        else
        {
            target = playerPosition + (direction * range);
        }

        target.z = 0;
        this.transform.position = target;
        Destroy(gameObject, duration);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<Enemy>())
        {
            collision.gameObject.GetComponent<Rigidbody2D>().WakeUp();
            timer += Time.deltaTime;
            if (timer >= damageInterval)
            {
                collision.gameObject.GetComponent<Enemy>().LoseHealth(damage);
                timer = 0f;
            }            
        }
    }

}
