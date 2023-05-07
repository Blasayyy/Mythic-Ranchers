using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MongoDB.Driver;
using MongoDB.Bson;

public class DatabaseManager : MonoBehaviour
{

    private const string CONNECTION_STRING = "mongodb+srv://Blasayy:Tisanerie1998@mythicranchers.r7dxreu.mongodb.net/?retryWrites=true&w=majority";
    private const string DB_NAME = "MythicRanchers";


    private IMongoCollection<BsonDocument> GetCollection(string collectionName)
    {
        var client = new MongoClient(CONNECTION_STRING);
        var database = client.GetDatabase(DB_NAME);
        return database.GetCollection<BsonDocument>(collectionName);
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


}
