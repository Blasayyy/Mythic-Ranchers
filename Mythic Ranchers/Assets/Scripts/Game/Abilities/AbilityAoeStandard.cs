using UnityEngine;
using Unity.Netcode;

/*******************************************************************************

   Nom du fichier: AbilityAoeStandard.cs
   
   Contexte: Cette classe représente une ability de type area of effect autour du joueur
   
   Auteur: Christophe Auclair
   
   Collaborateurs: Matei Pelletier

*******************************************************************************/

public class AbilityAoeStandard : NetworkBehaviour
{
    [SerializeField]
    private Ability ability;

    public void Start()
    {
        transform.localScale *= ability.radius;
        Destroy(gameObject, ability.duration);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (ability.helpful && collision.gameObject.GetComponent<PlayerUnit>())
        {
            collision.gameObject.GetComponent<PlayerUnit>().GainHealth(ability.potency);
            return;
        }

        // check pour voir si le bon collider est touché
        BoxCollider2D boxCollider = collision.gameObject.GetComponent<BoxCollider2D>();
        if (boxCollider == null)
            return;
        float distance = Vector2.Distance(this.transform.position, boxCollider.bounds.center);
        if (distance > 2)            
            return;

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
