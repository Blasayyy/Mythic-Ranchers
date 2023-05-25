using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class Enemy : NetworkBehaviour
{
    public float currentHp;
    public float maxHp;
    public float currentRessource;
    public float maxRessource;
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
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    public float flickerDuration = 0.1f;
    public int flickerCount = 2;
    private Color flickerColor = Color.red;

    public enum EnemyState
    {
        Wandering,
        Chasing,
        Idle
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        currentHp = maxHp;
        currentRessource = maxRessource;
        healthBar.SetHealth(currentHp, maxHp);
        ressourceBar.SetRessource(currentRessource, maxRessource);
        if (IsServer)
        {
            StartCoroutine(Wander());
        }

    }

    // Update is called once per frame
    void Update()
    {
        healthBar.SetHealth(currentHp, maxHp);
        ressourceBar.SetRessource(currentRessource, maxRessource);

        if (IsHost && currentState == EnemyState.Chasing)
        {
            Vector2 direction = (target.position - transform.position).normalized;
            rb.velocity = direction * moveSpeed;
        }
        else if(IsHost && currentState == EnemyState.Idle && !isWandering)
        {
            StartCoroutine(Wander());
        }

    }

    IEnumerator DamageFlicker()
    {
        for (int i = 0; i < flickerCount; i++)
        {
            spriteRenderer.color = flickerColor;
            yield return new WaitForSeconds(flickerDuration);

            spriteRenderer.color = Color.white;
            yield return new WaitForSeconds(flickerDuration);
        }
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
            rb.velocity = randomDirection * moveSpeed;
            rb.velocity = rb.velocity.normalized * moveSpeed;
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
        get { return currentHp; }
        set { currentHp = value; }
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
}
