using System.Collections.Generic;
using Catalog.API.Entities;
using MongoDB.Driver;

namespace Catalog.API.Data
{
    public class CatalogContextSeed
    {
        public static void SeedData(IMongoCollection<Product> productCollection)
        {
            bool existProduct = productCollection.Find(p => true).Any();
            if (!existProduct) 
                productCollection.InsertManyAsync(GetMyProducts());
        }

        private static IEnumerable<Product> GetMyProducts()
        {
            return new List<Product>()
            {
                new ()
                {
                    Id = "602d2149e773f2a3990b47f5",
                    Name = "IPhone X",
                    Description = "Descrição IPhone X",
                    Image = "iphone.png",
                    Price = 3000.00M,
                    Category = "Smart Phone"
                },
                new ()
                {
                    Id = "602d2149e773f2a3990b47f6",
                    Name = "Samsung S22",
                    Description = "Descrição Samsung S22",
                    Image = "S22.png",
                    Price = 6000.00M,
                    Category = "Smart Phone"
                },
                new ()
                {
                    Id = "602d2149e773f2a3990b47f7",
                    Name = "LG G7 ThinQ",
                    Description = "Descrição LG G7 ThinQ",
                    Image = "ThinQ.png",
                    Price = 240.00M,
                    Category = "Home Kitchen"
                },
            };
        }
    }
}
