using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;

namespace Labb3AndersOrlinder
{
    class Program
    {
        static void Main(string[] args)
        {
            MongoClient client = new MongoClient();
            var restaurantdb = client.GetDatabase("restaurantdb");
            restaurantdb.DropCollection("restaurants");
            var restaurants = restaurantdb.GetCollection<BsonDocument>("restaurants");

            var restaurantbsonarray = new List<BsonDocument>
            {
                new BsonDocument
                {
                    { "_id", "5c39f9b5df831369c19b6bca" },
                    { "name", "Sun Bakery Trattoria" },
                    { "stars", 4 },
                    { "categories",
                        new BsonArray
                        {
                            { "Pizza" },
                            { "Pasta" },
                            { "Italian" },
                            { "Coffee" },
                            { "Sandwiches" }
                        }
                    }
                },

                new BsonDocument
                {
                    { "_id", "5c39f9b5df831369c19b6bcb"},
                    { "name", "Blue Bagels Grill"},
                    { "stars", 3},
                    { "categories",
                        new BsonArray
                        {
                            { "Bagels" },
                            { "Cookies" },
                            { "Sandwiches" }
                        }
                    }
                },
                new BsonDocument
                {
                    { "_id", "5c39f9b5df831369c19b6bcc" },
                    { "name", "Hot Bakery Cafe" },
                    { "stars", 4 },
                    { "categories",
                        new BsonArray
                        {
                            { "Bakery" },
                            { "Cafe" },
                            { "Coffee" },
                            { "Dessert" }
                        }
                    }
                },
                new BsonDocument
                {
                    { "_id", "5c39f9b5df831369c19b6bcd" },
                    { "name", "XYZ Coffee Bar" },
                    { "stars", 5 },
                    { "categories",
                        new BsonArray
                        {
                            { "Coffee" },
                            { "Cafe" },
                            { "Bakery" },
                            { "Chocolates" }
                        }
                    }
                },
                new BsonDocument
                {
                    { "_id", "5c39f9b5df831369c19b6bce"},
                    { "name", "456 Cookies Shop" },
                    { "stars", 4 },
                    { "categories",
                        new BsonArray
                        {
                            { "Bakery" },
                            { "Cookies" },
                            { "Cake" },
                            { "Coffee" }
                        }
                    }
                }
            };

            restaurants.InsertMany(restaurantbsonarray);

            AllRestaurants(restaurants);
            Console.WriteLine();
            AllCafes(restaurants);
            Console.WriteLine();
            IncrementStarToXYZ(restaurants);
            Console.WriteLine();
            ChangeNameCookieShop(restaurants);
            Console.WriteLine();
            BetterRestaurangsByStars(restaurants);
        }

        private static void BetterRestaurangsByStars(IMongoCollection<BsonDocument> restaurants)
        {
            var filterByStars = Builders<BsonDocument>.Filter.Gte("stars", 4);
            var sort = Builders<BsonDocument>.Sort.Descending("stars");
            var proj = Builders<BsonDocument>.Projection.Include("name").Include("stars").Exclude("_id");

            var bestRestaurantsList = restaurants.Find(filterByStars).Sort(sort).Project(proj);
            foreach (var r in bestRestaurantsList.ToEnumerable())
            {
                Console.WriteLine(r);
            }
        }

        private static void ChangeNameCookieShop(IMongoCollection<BsonDocument> restaurants)
        {
            var filter456 = Builders<BsonDocument>.Filter.Eq("name", "456 Cookies Shop");
            var changeName = Builders<BsonDocument>.Update.Set("name", "123 Cookies Heaven");
            restaurants.UpdateOne(filter456, changeName);

            var restaurantList = restaurants.Find(new BsonDocument());
            foreach (var r in restaurantList.ToEnumerable())
            {
                Console.WriteLine(r);
            }
        }

        private static void IncrementStarToXYZ(IMongoCollection<BsonDocument> restaurants)
        {
            var filterXYZ = Builders<BsonDocument>.Filter.Eq("name", "XYZ Coffee Bar");
            var incrementStars = Builders<BsonDocument>.Update.Inc("stars", 1);
            restaurants.UpdateOne(filterXYZ, incrementStars);

            var restaurantList = restaurants.Find(new BsonDocument());
            foreach (var r in restaurantList.ToEnumerable())
            {
                Console.WriteLine(r);
            }
        }

        private static void AllCafes(IMongoCollection<BsonDocument> restaurants)
        {
            var filterCafe = Builders<BsonDocument>.Filter.Eq("categories", "Cafe");
            var projection = Builders<BsonDocument>.Projection.Exclude("_id").Include("name");
            var cafeList = restaurants.Find(filterCafe).Project(projection);

            foreach (var c in cafeList.ToEnumerable())
            {
                Console.WriteLine(c);
            }
        }

        private static void AllRestaurants(IMongoCollection<BsonDocument> restaurants)
        {
            var restaurantList = restaurants.Find(new BsonDocument());
            foreach (var r in restaurantList.ToEnumerable())
            {
                Console.WriteLine(r);
            }
        }
    }
}
