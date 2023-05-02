using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class PlayerClass : NetworkBehaviour
{
    private string name;
    //private Controller controller; ??
    private Vector2 position;
    private float hp = 5;
    private float basicAtkDmg;
    private float basicAtkSpeed;
    private float ressource;
    private int level;
    private string[] talents;
    private int talentPointsAvailable;
    private int xp;
    private string[] equipment;
    private string[] inventory;
    private string[] abilities;
    private string[] stats; //dictionary
    private ArmorType armorType;
    private int keyLevel;

    public enum ArmorType
    {
        Cloth,
        Leather,
        Mail
    }



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TakeDamage(float damage)
    {
        hp -= damage;
    }

    public float GetHP()
    {
        return hp;
    }


}
