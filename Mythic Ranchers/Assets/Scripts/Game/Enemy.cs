using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class Enemy : NetworkBehaviour
{
    public NetworkVariable<float> currentHp;
    public float maxHp;
    public float currentRessource;
    public float maxRessource;
    public string ressourceType;
    public float moveSpeed = 2f;
    public float waitTimeMin = 1.0f;
    public float waitTimeMax = 3.0f;
    public string type;
    public GameObject abilityPrefab;
    public Ability abilityScriptable;
    private EnemyState currentState;
    private Transform target;
    private bool isWandering = false;
    public HealthBar healthBar;
    public RessourceBar ressourceBar;
    private Rigidbody2D rig;
    private SpriteRenderer spriteRenderer;
    public float flickerDuration = 0.1f;
    public int flickerCount = 2;
    private bool slowed;

    public BoxCollider2D boxCollider;

    public enum EnemyState
    {
        Wandering,
        Chasing,
        Idle
    }

    private void Awake()
    {
        rig = GetComponent<Rigidbody2D>();
    }

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        currentHp.Value = maxHp;
        currentRessource = maxRessource;
        healthBar.SetHealth(currentHp.Value, maxHp);
        ressourceBar.SetRessource(currentRessource, maxRessource, ressourceType);
        if (IsServer)
        {
            StartCoroutine(Wander());
        }

        
    }

    // Update is called once per frame
    void Update()
    {
        healthBar.SetHealth(currentHp.Value, maxHp);
        ressourceBar.SetRessource(currentRessource, maxRessource, ressourceType);

        if (IsHost && currentState == EnemyState.Chasing)
        {
            Vector2 direction = (target.position - transform.position).normalized;
            StopCoroutine(Wander());
            rig.velocity = direction * moveSpeed;

            if(type == "ghast")
            {
                int random = Random.Range(0, 10);
                if(random > 8)
                {
                    GameObject projectile = Instantiate(abilityPrefab, transform.position, Quaternion.identity);
                    projectile.GetComponent<EnemyProjectile>().SetDirection(target.position);
                }
                
            }
            
        }
        else if(IsHost && currentState == EnemyState.Idle && !isWandering)
        {
            StartCoroutine(Wander());
        }

        if(currentHp.Value <= 0)
        {
            NetworkObject netO = GetComponent<NetworkObject>();
            netO.Despawn();
            MythicGameManagerMultiplayer.Instance.EnemiesCount.Value -= 1;
        }

    }

    IEnumerator DamageFlicker()
    {
        if (slowed) 
            yield break;

        for (int i = 0; i < flickerCount; i++)
        {
            spriteRenderer.color = Color.red;
            yield return new WaitForSeconds(flickerDuration);

            spriteRenderer.color = Color.white;
            yield return new WaitForSeconds(flickerDuration);
        }
        
    }

    public IEnumerator PreventKnockback()
    {
        rig.isKinematic = false;
        yield return new WaitForSeconds(0.1f);
        rig.isKinematic = true;

    }

    IEnumerator Slowed(float slowDuration, float slowAmount)
    {
        slowed = true;
        float normalSpeed = MoveSpeed;
        if (slowAmount == 1f)
        {
            rig.constraints = RigidbodyConstraints2D.FreezeAll;
        }
        else
        {
            MoveSpeed *= 1 - slowAmount;
        }
        spriteRenderer.color = new Color(0, 200, 255, 255);
        yield return new WaitForSeconds(slowDuration);
        spriteRenderer.color = Color.white;
        if (slowAmount == 1f)
        {
            rig.constraints = RigidbodyConstraints2D.None;
            rig.constraints = RigidbodyConstraints2D.FreezeRotation;
        }
        else
        {
            MoveSpeed = normalSpeed;
        }
        slowed = false;
    }

    IEnumerator Wander()
    {
        isWandering = true;
        currentState = EnemyState.Wandering;

        Vector2 randomDirection = new Vector2(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f)).normalized;
        float randomWaitTime = Random.Range(waitTimeMin, waitTimeMax);

        float wanderStartTime = Time.time;
        while (Time.time < wanderStartTime + randomWaitTime)
        {
            rig.velocity = randomDirection * moveSpeed;
            rig.velocity = rig.velocity.normalized * moveSpeed;
            yield return null;
        }

        yield return new WaitForSeconds(randomWaitTime);

        currentState = EnemyState.Idle;
        isWandering = false;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (IsHost)
        {
            if (LayerMask.LayerToName(other.gameObject.layer) == "Player")
            {
                target = other.transform;
                currentState = EnemyState.Chasing;
            }
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (IsHost)
        {
            if (LayerMask.LayerToName(other.gameObject.layer) == "Player")
            {
                currentState = EnemyState.Idle;
            }
        }
    }

    public void GetSlowed(float slowDuration, float slowAmount)
    {
        StartCoroutine(Slowed(slowDuration, slowAmount));
    }

    public void NoKnockback()
    {
        StartCoroutine(PreventKnockback());
    }

    public void LoseHealth(float amount)
    {
        CurrentHp -= amount;
        StartCoroutine(DamageFlicker());
    }

    public void LoseRessource(float amount)
    {
        CurrentRessource -= amount;
    }

    public float CurrentHp
    {
        get { return currentHp.Value; }
        set { currentHp.Value = value; }
    }

    public float MaxHp
    {
        get { return maxHp; }
        set { maxHp = value; }
    }

    public float CurrentRessource
    {
        get { return currentRessource; }
        set { currentRessource = value; }
    }

    public float MaxRessource
    {
        get { return maxRessource; }
        set { maxRessource = value; }
    }

    public string RessourceType
    {
        get { return ressourceType; }
        set { ressourceType = value; }
    }

    public float MoveSpeed
    {
        get { return moveSpeed; }
        set { moveSpeed = value; }
    }
}
