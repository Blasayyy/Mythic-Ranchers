using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FelBomb : MonoBehaviour
{
    [SerializeField]
    private float duration;
    private float range;
    private Vector3 target, cursorWorldPosition, playerPosition, direction;

    void Start()
    { 
        // hardcoded until we link to db
        range = AbilityManager.instance.abilities[1].range;

        cursorWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        cursorWorldPosition.z = 0;
        playerPosition = PlayerUnit.instance.transform.position;

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


    private void OnCollisionEnter2D(Collision2D collision)
    {
        //Destroy(this.gameObject);
    }
}
