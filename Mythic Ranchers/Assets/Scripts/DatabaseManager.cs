using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MongoDB.Driver;
using MongoDB.Bson;
using System;

public class DatabaseManager : MonoBehaviour
{

    public static DatabaseManager Instance { get; private set; }

    private const string CONNECTION_STRING = "mongodb+srv://Blasayy:Tisanerie1998@mythicranchers.r7dxreu.mongodb.net/?retryWrites=true&w=majority";
    private const string DB_NAME = "MythicRanchers";
    private const string USERS_COLLECTION = "Users";


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

    public async void InsertDocument(string collectionName)
    {
        try
        {
            var collection = GetCollection(collectionName);

            BsonDocument document = new BsonDocument
            {
                { "field1", "value1" },
                { "field2", "value2" },
            };

            await collection.InsertOneAsync(document);
            Debug.Log("Document inserted successfully.");
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

    public async void CreateUser(string username, string password)
    {
        try
        {
            var collection = GetCollection(USERS_COLLECTION);

            BsonDocument document = new BsonDocument
            {
                {"username", username },
                {"password", password },
            };

            await collection.InsertOneAsync(document);
            Debug.Log("User created succesfully");
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
}
