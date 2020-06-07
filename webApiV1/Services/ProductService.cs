using MongoDB.Driver;
using webApiV1.Models.Products;
using AspNetCore.Identity.MongoDbCore.Infrastructure;
using System.Linq;
using System.Collections.Generic;

using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MongoDbGenericRepository.Models;

namespace webApiV1.Services
{
    public class ProductService
    {

        private readonly IMongoCollection<Product> _products;
        public ProductService(MongoDbSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            _products = database.GetCollection<Product>("Products");
        }


        public async Task<List<Product>> GetAll()
        {
            var products = await _products.Find(product => true).ToListAsync();
            return products;
        }


    }
}