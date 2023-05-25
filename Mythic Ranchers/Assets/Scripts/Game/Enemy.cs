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
    private bool isWandering = false;
    public HealthBar healthBar;
    public RessourceBar ressourceBar;
    private Rigidbody2D rb;


    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Start is called before the first frame update
    void Start()
    {
        currentHp = maxHp;
        currentRessource = maxRessource;
        healthBar.SetHealth(currentHp, maxHp);
        ressourceBar.SetRessource(currentRessource, maxRessource);
    }

    // Update is called once per frame
    void Update()
    {
        healthBar.SetHealth(currentHp, maxHp);
        ressourceBar.SetRessource(currentRessource, maxRessource);

        if (IsHost)
        {
            if (!isWandering)
            {
                StartCoroutine(Wander());
            }
        }
        
    }

    IEnumerator Wander()
    {
        isWandering = true;

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

        isWandering = false;
    }

    public void LoseHealth(float amount)
    {
        CurrentHp -= amount;
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
