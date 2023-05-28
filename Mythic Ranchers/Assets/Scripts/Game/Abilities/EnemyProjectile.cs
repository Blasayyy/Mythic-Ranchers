using UnityEngine;
using Unity.Netcode;

/*******************************************************************************

   Nom du fichier: EnemyProjectile.cs
   
   Contexte: Cette classe représente une ability de type projectile d'un enemi
   
   Auteur: Christophe Auclair
   
   Collaborateurs: Matei Pelletier

*******************************************************************************/

public class EnemyProjectile : NetworkBehaviour
{
    [SerializeField]
    private Ability ability;
    private Rigidbody2D rig;
    Vector3 initialPosition;
    private Vector3 direction;

    public void Start()
    {
        rig = GetComponent<Rigidbody2D>();
        Vector3 cursorPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        cursorPosition.z = 0;
        initialPosition = transform.position;
        transform.localScale *= ability.radius;
        var angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(Vector3.forward * angle);
    }

    public void SetDirection(Vector3 targetPosition)
    {
        rig = GetComponent<Rigidbody2D>();
        direction = (targetPosition - transform.position).normalized;
        rig.velocity = direction * 5.0f;
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
        if (collision.gameObject.GetComponent<PlayerUnit>())
        {
            collision.gameObject.GetComponent<PlayerUnit>().LoseHealth(ability.potency);
        }

        Destroy(this.gameObject);
    }
}
