using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Threading.Tasks;

public class PlayerUnit : PlayerClass
{

    public static PlayerUnit instance;

    private CharacterData CharacterData;
    //[SerializeField]
    //private GameObject abilityManagerPrefab;

    private void Awake()
    {
        instance = this;
    }

    

    public void AssignVaribles(object[] playerInfo)
    {
        this.PlayerName = (string)playerInfo[0];
        this.ClassName = (string)playerInfo[1];
        this.Position = (Vector3)playerInfo[2];
        this.MoveSpeed = (float)playerInfo[3];
        this.Hp = (float)playerInfo[4]; 
        this.BasicAtkDmg = (float)playerInfo[5];
        this.BasicAtkSpeed = (float)playerInfo[6];
        this.Ressource = (float)playerInfo[7];
        this.Level = (int)playerInfo[8];
        this.Talents = "";
        this.TalentPointsAvailable = (int)playerInfo[10];
        this.Xp = (int)playerInfo[11];
        this.Equipment = null;
        this.Inventory = (string[])playerInfo[13];
        this.Abilities = (string[])playerInfo[14];
        this.Stats = null;
        this.ArmorType = (ArmorType)playerInfo[16];
        this.KeyLevel = (int)playerInfo[17];
    }

    public void SetCharacterData(CharacterData characterData)
    {
        this.CharacterData = characterData;

        this.PlayerName = characterData.Name;
        this.ClassName = characterData.ClassName;
        this.Position = new Vector3(0, 0, 0);
        this.MoveSpeed = characterData.Stats["haste"] + 2.0f;
        this.Hp = characterData.Stats["stamina"] * 10 + 100;
        this.BasicAtkDmg = characterData.Stats["strength"] * 1.5f + 5f;
        this.BasicAtkSpeed = characterData.Stats["haste"] + 5f;
        this.Ressource = characterData.Stats["intellect"] * 10 + 100f;
        this.Level = characterData.Level;
        this.Talents = characterData.Talents;
        this.Xp = characterData.Experience_points;
        this.Equipment = characterData.EquipmentList;
        this.Inventory = null; //todo
        this.Abilities = null; //todo
        this.Stats = characterData.Stats;
        this.ArmorType = ArmorType.Cloth; // todo
        this.KeyLevel = characterData.Current_key;
    }

    void Start()
    {
        
        //GameObject abilityManagerObject = Instantiate(abilityManagerPrefab, transform);
        //AbilityManager abilityManager = abilityManagerObject.GetComponent<AbilityManager>();
        //abilityManager.ownerPlayerUnit = this;

        object[] stats = createVariables();
        this.AssignVaribles(stats);
        CameraFollowPlayer.instance.SetCameraFollowPlayer(this.transform);
        base.Start();
    }

    private void Update()
    {
        base.Update();
    }

    private void FixedUpdate()
    {
        base.FixedUpdate();
    }
}
