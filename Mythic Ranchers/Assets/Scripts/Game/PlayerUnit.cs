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

    public void SetCharacterData(CharacterData characterData)
    {
        this.CharacterData = characterData;

        this.PlayerName = characterData.Name;
        this.ClassName = characterData.ClassName;
        this.Position = new Vector3(0, 0, 0);
        this.MoveSpeed = characterData.Stats["haste"] + 2.0f;
        this.MaxHp = characterData.Stats["stamina"] * 10 + 100;
        this.BasicAtkDmg = characterData.Stats["strength"] * 1.5f + 5f;
        this.BasicAtkSpeed = characterData.Stats["haste"] + 5f;
        this.MaxRessource = characterData.Stats["intellect"] * 10 + 100f;
        this.Level = characterData.Level;
        this.Talents = characterData.Talents;
        this.Xp = characterData.Experience_points;
        this.Equipment = characterData.EquipmentList;
        this.Inventory = null; //todo
        this.Abilities = null; //todo
        this.Stats = characterData.Stats;
        this.InitialStats = characterData.Stats;
        this.ArmorType = ArmorType.Mail; // todo
        this.KeyLevel = characterData.Current_key;
    }

    void Start()
    {

        //GameObject abilityManagerObject = Instantiate(abilityManagerPrefab, transform);
        //AbilityManager abilityManager = abilityManagerObject.GetComponent<AbilityManager>();
        //abilityManager.ownerPlayerUnit = this;

        MaxHp = 5;
        CurrentHp = MaxHp;
        MaxRessource = 20;
        CurrentRessource = MaxRessource;
        RessourceType = "mana";
        
        base.Start();
    }

    private void Update()
    {
        if (IsOwner)
        {
            CameraFollowPlayer.instance.SetCameraFollowPlayer(this.transform);
        }
        base.Update();
    }

    private void FixedUpdate()
    {
        base.FixedUpdate();
    }
}
