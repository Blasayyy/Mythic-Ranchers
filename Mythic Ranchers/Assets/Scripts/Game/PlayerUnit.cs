using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Threading.Tasks;

public class PlayerUnit : PlayerClass
{

    public static PlayerUnit instance;

    private void Awake()
    {
        instance = this;
    }

    public void AssignVaribles(object[] playerInfo)
    {
        this.PlayerName = (string)playerInfo[0];
        this.ClassName = (string)playerInfo[1];
        this.Position = (Vector2)playerInfo[2];
        this.MoveSpeed = (float)playerInfo[3];
        this.Hp = (float)playerInfo[4]; 
        this.BasicAtkDmg = (float)playerInfo[5];
        this.BasicAtkSpeed = (float)playerInfo[6];
        this.Ressource = (float)playerInfo[7];
        this.Level = (int)playerInfo[8];
        this.Talents = (string[])playerInfo[9];
        this.TalentPointsAvailable = (int)playerInfo[10];
        this.Xp = (int)playerInfo[11];
        this.Equipment = (string[])playerInfo[12];
        this.Inventory = (string[])playerInfo[13];
        this.Abilities = (string[])playerInfo[14];
        this.Stats = (string[])playerInfo[15];
        this.ArmorType = (ArmorType)playerInfo[16];
        this.KeyLevel = (int)playerInfo[17];
    }

    public object[] createVariables()
    {
        object[] playerInfo = new object[18];
        playerInfo[0] = "Whutz";
        playerInfo[1] = "Berzerker";
        playerInfo[2] = new Vector2(0, 0);
        playerInfo[3] = 2.0f;
        playerInfo[4] = 5.0f;
        playerInfo[5] = 1.5f;
        playerInfo[6] = 1.0f;
        playerInfo[7] = 1.0f;
        playerInfo[8] = 1;
        playerInfo[9] = new string[] { "talent1", "talent2", "talent3" };
        playerInfo[10] = 0;
        playerInfo[11] = 0;
        playerInfo[12] = new string[] { "equipment1", "equipment2", "equipment3" };
        playerInfo[13] = new string[] { "inventory1", "inventory2", "inventory3" };
        playerInfo[14] = new string[] { "ability1", "ability2", "ability3" };
        playerInfo[15] = new string[] { "stat1", "stat2", "stat3" };
        playerInfo[16] = ArmorType.Mail;
        playerInfo[17] = 1;

        return playerInfo;
    }

    void Start()
    {
        object[] stats = createVariables();
        this.AssignVaribles(stats);
        base.Start();
    }

    private void Update()
    {
        base.Update();
        Debug.Log(this.BasicAtkDmg);
    }

    private void FixedUpdate()
    {
        base.FixedUpdate();
    }
}
