using UnityEngine;
using Unity.Netcode;

/*******************************************************************************

   Nom du fichier: AbilityProjectile.cs
   
   Contexte: Cette classe représente une ability de type projectile
   
   Auteur: Christophe Auclair
   
   Collaborateurs: Matei Pelletier

*******************************************************************************/

public class AbilityProjectile : NetworkBehaviour
{
    [SerializeField]
    private Ability ability;
    private Rigidbody2D rig;
    private NetworkVariable<Vector3> cursorPosition = new NetworkVariable<Vector3>(new Vector3());
    private NetworkVariable<Vector3> initialPosition = new NetworkVariable<Vector3>(new Vector3());
    private bool isInitialized = false;
    private NetworkVariable<bool> isSetUp = new NetworkVariable<bool>(false);

    public void Start()
    {

        transform.localScale *= ability.radius;
    }

    public void InitializeProjectile()
    {
        if (!isInitialized)
        {
            rig = GetComponent<Rigidbody2D>();
            Vector3 direction = (cursorPosition.Value - initialPosition.Value).normalized;
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
        if (isSetUp.Value)
        {
            InitializeProjectile();
            isSetUp.Value = false;
        }

        float distanceTravelled = Vector3.Distance(transform.position, initialPosition.Value);

        if (distanceTravelled >= ability.range)
        {
            NetworkObject obj = GetComponent<NetworkObject>();
            obj.Despawn();
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
        NetworkObject obj = GetComponent<NetworkObject>();
        obj.Despawn();
    }

    public void SetCursorPos(Vector3 cursorPos)
    {
        cursorPosition.Value = cursorPos;
    }

    public void SetInitialPosition(Vector3 position)
    {
        initialPosition.Value = position;
        isSetUp.Value = true;
    }
}
