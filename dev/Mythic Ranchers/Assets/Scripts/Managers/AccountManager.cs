using MongoDB.Bson;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

/*******************************************************************************

   Nom du fichier: AccountManager.cs
   
   Contexte: Cette classe sert a gérer les comptes et les informations des joueurs
   
   Auteur: Matei Pelletier
   
   Collaborateurs: Christophe Auclair

*******************************************************************************/

public class AccountManager : MonoBehaviour
{
    public static AccountManager Instance { get; private set; }

    public string Username { get; set; }
    public List<CharacterData> CharacterDatas { get; private set; }

    public int SelectedCharacter { get; set; }

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            SelectedCharacter = 0;
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public async void GetUserData(string username)
    {
        Username = username;
        CharacterDatas = await GetCharacterDatas();
    }

    private async Task<List<CharacterData>> GetCharacterDatas()
    {
        List<BsonDocument> characterBsons = await DatabaseManager.Instance.GetCharactersForAccount(Username);
        List<CharacterData> characterDatas = new List<CharacterData>();

        foreach (BsonDocument bsonCharacter in characterBsons)
        {
            CharacterData characterData = BsonToCharacterData(bsonCharacter);
            characterDatas.Add(characterData);
        }

        return characterDatas;
    }

    private CharacterData BsonToCharacterData(BsonDocument bsonDocument)
    {
        string name = bsonDocument.GetValue("name").AsString;
        string username = bsonDocument.GetValue("username").AsString;
        int level = bsonDocument.GetValue("level").AsInt32;
        int experiencePoints = bsonDocument.GetValue("experiencePoints").AsInt32;
        string className = bsonDocument.GetValue("class").AsString;
        int currentKey = bsonDocument.GetValue("currentKey").AsInt32;

        BsonArray equipmentBsonArray = bsonDocument.GetValue("equipmentList").AsBsonArray;
        List<EquipmentData> equipmentList = new List<EquipmentData>();

        // not implemented
        //foreach (BsonDocument equipmentBson in equipmentBsonArray)
        //{
        //    string equipmentName = equipmentBson.GetValue("name").AsString;
        //    EquipmentData equipmentData = new EquipmentData(equipmentName);
        //    equipmentList.Add(equipmentData);
        //}

        string talents = bsonDocument.GetValue("talents").AsString;

        return new CharacterData(name, username, level, experiencePoints, className, currentKey, equipmentList, talents);
    }
}
