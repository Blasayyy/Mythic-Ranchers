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
    public HealthBar healthBar;
    public RessourceBar ressourceBar;

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
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        currentHp -= 1;
        currentRessource -= 1;
    }
}
