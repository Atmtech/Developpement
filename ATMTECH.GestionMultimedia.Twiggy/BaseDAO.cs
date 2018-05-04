﻿using MongoDB.Driver;

namespace ATMTECH.GestionMultimedia.Twiggy
{
    public class BaseDAO
    {
        public IMongoDatabase Database
        {
            get
            {
                var connectionString = "mongodb://127.0.0.1:27017/db";
                MongoClient client = new MongoClient(connectionString);
                IMongoDatabase mongoDatabase = client.GetDatabase("GestionMultimediaTwiggy");
                return mongoDatabase;
            }
        }
    }
}