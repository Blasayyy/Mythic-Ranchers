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
