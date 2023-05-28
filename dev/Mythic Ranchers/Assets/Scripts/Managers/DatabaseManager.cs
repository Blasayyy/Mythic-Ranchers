using System.Collections.Generic;
using UnityEngine;
using MongoDB.Driver;
using MongoDB.Bson;
using System;
using System.Threading.Tasks;

/*******************************************************************************

   Nom du fichier: DatabaseManager.cs
   
   Contexte: Cette classe sert a gérer la base donnée
   
   Auteur: Matei Pelletier
   
   Collaborateurs: Christophe Auclair

*******************************************************************************/

public class DatabaseManager : MonoBehaviour
{
    public static DatabaseManager Instance { get; private set; }

    private const string CONNECTION_STRING = "mongodb+srv://Blasayy:Tisanerie1998@mythicranchers.r7dxreu.mongodb.net/?retryWrites=true&w=majority";
    private const string DB_NAME = "MythicRanchers";
    private const string USERS_COLLECTION = "Users";
    private const string CHARACTERS_COLLECTION = "Characters";

    private void Awake()
    {
        Instance = this;
    }

    private IMongoCollection<BsonDocument> GetCollection(string collectionName)
    {
        try
        {
            var settings = MongoClientSettings.FromConnectionString(CONNECTION_STRING);
            settings.ServerApi = new ServerApi(ServerApiVersion.V1);
            var client = new MongoClient(settings);
            var database = client.GetDatabase(DB_NAME);
            return database.GetCollection<BsonDocument>(collectionName);
        }
        catch (MongoConfigurationException ex)
        {
            Debug.LogError("MongoDB configuration exception: " + ex.Message);
        }
        catch (MongoConnectionException ex)
        {
            Debug.LogError("MongoDB connection exception: " + ex.Message);
        }
        catch (MongoException ex)
        {
            Debug.LogError("MongoDB exception: " + ex.Message);
        }
        catch(Exception ex)
        {
            Debug.LogError(ex);
        }

        return default;
    }

    public async Task<bool> CreateUser(string username, string password)
    {
        try
        {
            if (await IsUsernameTaken(username))
            {
                Debug.LogError("Username already taken");
                return false;
            }

            var collection = GetCollection(USERS_COLLECTION);

            BsonDocument document = new BsonDocument
            {
                {"username", username },
                {"password", password },
            };

            await collection.InsertOneAsync(document);
            Debug.Log("User created succesfully");
            return true;
        }
        catch (MongoConfigurationException ex)
        {
            Debug.LogError("MongoDB configuration exception: " + ex.Message);
        }
        catch (MongoConnectionException ex)
        {
            Debug.LogError("MongoDB connection exception: " + ex.Message);
        }
        catch (MongoException ex)
        {
            Debug.LogError("MongoDB exception: " + ex.Message);
        }
        return false;
    }

    public async Task<bool> IsUsernameTaken(string username)
    {
        try
        {
            var collection = GetCollection(USERS_COLLECTION);
            FilterDefinition<BsonDocument> filter = Builders<BsonDocument>.Filter.Eq("username", username);
            var result = await collection.Find(filter).FirstOrDefaultAsync();

            if (result != null)
            {
                return true;
            }
        }
        catch (MongoConfigurationException ex)
        {
            Debug.LogError("MongoDB configuration exception: " + ex.Message);
        }
        catch (MongoConnectionException ex)
        {
            Debug.LogError("MongoDB connection exception: " + ex.Message);
        }
        catch (MongoException ex)
        {
            Debug.LogError("MongoDB exception: " + ex.Message);
        }

        return false;
    }

    public async Task<bool> LoginUser(string username, string password)
    {
        try
        {
            var collection = GetCollection(USERS_COLLECTION);
            FilterDefinition<BsonDocument> filter = Builders<BsonDocument>.Filter.Eq("username", username);
            var result = await collection.Find(filter).FirstOrDefaultAsync();

            if (result != null)
            {
                string storedHashedPassword = result["password"].AsString;

                if (PasswordHasher.Instance.VerifyPassword(password, storedHashedPassword))
                {
                    Debug.Log("Login successful");
                    return true;
                }
                else
                {
                    Debug.LogError("Incorrect password");
                }
            }
            else
            {
                Debug.LogError("User not found");
            }
        }
        catch (MongoConfigurationException ex)
        {
            Debug.LogError("MongoDB configuration exception: " + ex.Message);
        }
        catch (MongoConnectionException ex)
        {
            Debug.LogError("MongoDB connection exception: " + ex.Message);
        }
        catch (MongoException ex)
        {
            Debug.LogError("MongoDB exception: " + ex.Message);
        }

        return false;
    }

    public async Task<bool> CreateCharacter(string username, string characterName, string className, int level, int experiencePoints, int currentKey, List<EquipmentData> equipmentList, string talents)
    {
        try
        {
            if (await CharacterNameExists(characterName))
            {
                Debug.LogError("Character name already exists. Please choose a different name.");
                return false;
            }

            var collection = GetCollection(CHARACTERS_COLLECTION);

            BsonArray equipmentArray = new BsonArray();
            foreach (var equipment in equipmentList)
            {
                equipmentArray.Add(new BsonDocument { 
                    { "name", equipment.Name },
                    { "slot", equipment.Slot },
                    { "stamina", equipment.Stamina },
                    { "strength", equipment.Strength },
                    { "intellect", equipment.Intellect },
                    { "agility", equipment.Agility },
                    { "armor", equipment.Armor },
                    { "haste", equipment.Haste },
                    { "leech", equipment.Leech }
                });
            }

            BsonDocument document = new BsonDocument
        {
            { "username", username },
            { "name", characterName },
            { "class", className},
            { "level", level },
            { "experiencePoints", experiencePoints },
            { "currentKey", currentKey },
            { "equipmentList", equipmentArray },
            { "talents", talents },
        };

            await collection.InsertOneAsync(document);
            Debug.Log("Character created successfully");
            return true;
        }
        catch (MongoConfigurationException ex)
        {
            Debug.LogError("MongoDB configuration exception: " + ex.Message);
        }
        catch (MongoConnectionException ex)
        {
            Debug.LogError("MongoDB connection exception: " + ex.Message);
        }
        catch (MongoException ex)
        {
            Debug.LogError("MongoDB exception: " + ex.Message);
        }

        return false;
    }

    public async void DeleteCharacter(string username, string characterName)
    {
        try
        {
            var collection = GetCollection(CHARACTERS_COLLECTION);
            FilterDefinition<BsonDocument> filter = Builders<BsonDocument>.Filter.And(
                Builders<BsonDocument>.Filter.Eq("username", username),
                Builders<BsonDocument>.Filter.Eq("name", characterName)
            );

            var result = await collection.DeleteOneAsync(filter);

            if (result.IsAcknowledged && result.DeletedCount > 0)
            {
                Debug.Log("Character deleted successfully");
            }
            else
            {
                Debug.LogError("Character not found or could not be deleted");
            }
        }
        catch (MongoConfigurationException ex)
        {
            Debug.LogError("MongoDB configuration exception: " + ex.Message);
        }
        catch (MongoConnectionException ex)
        {
            Debug.LogError("MongoDB connection exception: " + ex.Message);
        }
        catch (MongoException ex)
        {
            Debug.LogError("MongoDB exception: " + ex.Message);
        }
    }

    public async Task<bool> CharacterNameExists(string characterName)
    {
        try
        {
            var collection = GetCollection(CHARACTERS_COLLECTION);
            FilterDefinition<BsonDocument> filter = Builders<BsonDocument>.Filter.Eq("name", characterName);

            var count = await collection.CountDocumentsAsync(filter);

            return count > 0;
        }
        catch (MongoConfigurationException ex)
        {
            Debug.LogError("MongoDB configuration exception: " + ex.Message);
        }
        catch (MongoConnectionException ex)
        {
            Debug.LogError("MongoDB connection exception: " + ex.Message);
        }
        catch (MongoException ex)
        {
            Debug.LogError("MongoDB exception: " + ex.Message);
        }

        return false;
    }

    public async Task<List<BsonDocument>> GetCharactersForAccount(string username)
    {
        try
        {
            var collection = GetCollection(CHARACTERS_COLLECTION);
            FilterDefinition<BsonDocument> filter = Builders<BsonDocument>.Filter.Eq("username", username);

            var characters = await collection.FindAsync(filter).Result.ToListAsync();

            return characters;
        }
        catch (MongoConfigurationException ex)
        {
            Debug.LogError("MongoDB configuration exception: " + ex.Message);
        }
        catch (MongoConnectionException ex)
        {
            Debug.LogError("MongoDB connection exception: " + ex.Message);
        }
        catch (MongoException ex)
        {
            Debug.LogError("MongoDB exception: " + ex.Message);
        }

        return null;
    }

    public async Task<bool> DeleteCharacter(string characterName)
    {
        try
        {
            var collection = GetCollection(CHARACTERS_COLLECTION);
            var filter = Builders<BsonDocument>.Filter.Eq("name", characterName);
            var result = await collection.DeleteOneAsync(filter);

            if (result.DeletedCount > 0)
            {
                Debug.Log("Character " + characterName + " deleted successfully.");
                return true;
            }
            else
            {
                Debug.Log("No character found with the provided name.");
            }
        }
        catch (MongoConfigurationException ex)
        {
            Debug.LogError("MongoDB configuration exception: " + ex.Message);
        }
        catch (MongoConnectionException ex)
        {
            Debug.LogError("MongoDB connection exception: " + ex.Message);
        }
        catch (MongoException ex)
        {
            Debug.LogError("MongoDB exception: " + ex.Message);
        }

        return false;
    }

    public async Task<bool> UpdateCharacter(CharacterData characterData)
    {
        try
        {
            var collection = GetCollection(CHARACTERS_COLLECTION);

            var filter = Builders<BsonDocument>.Filter.Eq("name", characterData.Name);

            BsonArray equipmentArray = new BsonArray();
            foreach (var equipment in characterData.EquipmentList)
            {
                equipmentArray.Add(new BsonDocument {
                { "name", equipment.Name },
                { "slot", equipment.Slot },
                { "stamina", equipment.Stamina },
                { "strength", equipment.Strength },
                { "intellect", equipment.Intellect },
                { "agility", equipment.Agility },
                { "armor", equipment.Armor },
                { "haste", equipment.Haste },
                { "leech", equipment.Leech }
            });
            }

            var update = Builders<BsonDocument>.Update
                .Set("username", characterData.Username)
                .Set("class", characterData.ClassName)
                .Set("level", characterData.Level)
                .Set("experiencePoints", characterData.Experience_points)
                .Set("currentKey", characterData.Current_key)
                .Set("equipmentList", equipmentArray)
                .Set("talents", characterData.Talents);

            var result = await collection.UpdateOneAsync(filter, update);

            if (result.IsAcknowledged && result.ModifiedCount > 0)
            {
                Debug.Log("Character updated successfully");
                return true;
            }
            else
            {
                Debug.LogError("Character not found or could not be updated");
            }
        }
        catch (MongoConfigurationException ex)
        {
            Debug.LogError("MongoDB configuration exception: " + ex.Message);
        }
        catch (MongoConnectionException ex)
        {
            Debug.LogError("MongoDB connection exception: " + ex.Message);
        }
        catch (MongoException ex)
        {
            Debug.LogError("MongoDB exception: " + ex.Message);
        }

        return false;
    }
}