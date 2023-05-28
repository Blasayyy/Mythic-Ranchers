using UnityEngine;
using Unity.Netcode;

/*******************************************************************************

   Nom du fichier: AbilityAoeTargeted.cs
   
   Contexte: Cette classe représente une ability de type area of effect a un endroit choisi
   
   Auteur: Christophe Auclair
   
   Collaborateurs: Matei Pelletier

*******************************************************************************/

public class AbilityAoeTargeted : NetworkBehaviour
{
    [SerializeField]
    private Ability ability;
    private float timer = 0;
    private Vector3 target, cursorWorldPosition, playerPosition, direction;

    public void Start()
    {
        cursorWorldPosition.z = 0;
        playerPosition = transform.position;

        direction = (cursorWorldPosition - playerPosition).normalized;
        transform.localScale *= ability.radius;

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

    public void SetCursorPos(Vector3 cursorPos)
    {
        cursorWorldPosition = cursorPos;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (ability.helpful && collision.gameObject.GetComponent<PlayerUnit>())
        {
            collision.gameObject.GetComponent<Rigidbody2D>().WakeUp();
            timer += Time.deltaTime;
            if (timer >= ability.tick)
            {
                collision.gameObject.GetComponent<PlayerUnit>().GainHealth(ability.potency);
                timer = 0f;
            }
            return;
        }

        // check pour voir si le bon collider est touché
        BoxCollider2D boxCollider = collision.gameObject.GetComponent<BoxCollider2D>();
        if (boxCollider == null)
            return;
        float distance = Vector2.Distance(this.transform.position, boxCollider.bounds.center);
        if (distance > 2)
            return;        
        if (ability.tick == 0)        
            return;        

        if (!ability.helpful && collision.gameObject.GetComponent<Enemy>())
        {
            collision.gameObject.GetComponent<Rigidbody2D>().WakeUp();
            timer += Time.deltaTime;
            if (timer >= ability.tick)
            {
                collision.gameObject.GetComponent<Enemy>().LoseHealth(ability.potency);
                timer = 0f;
            }
        }                
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (ability.helpful && collision.gameObject.GetComponent<PlayerUnit>() && ability.tick == 0)
        {
            collision.gameObject.GetComponent<PlayerUnit>().GainHealth(ability.potency);
        }

        // check pour voir si le bon collider est touché
        BoxCollider2D boxCollider = collision.gameObject.GetComponent<BoxCollider2D>();
        if (boxCollider == null)
            return;
        float distance = Vector2.Distance(this.transform.position, boxCollider.bounds.center);
        if (distance > 2)
            return;

        if (!ability.helpful && collision.gameObject.GetComponent<Enemy>() && ability.tick == 0)
        {
            collision.gameObject.GetComponent<Enemy>().LoseHealth(ability.potency);
        }                    
    }
}
