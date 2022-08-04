﻿using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Dapper;
using Discount.API.Entities;
using Microsoft.Extensions.Configuration;
using Npgsql;

namespace Discount.API.Repositories
{
    public class DiscountRepository : IDiscountRepository
    {
        private readonly NpgsqlConnection _connectionString;

        public DiscountRepository(IConfiguration configuration)
        {
            _connectionString = new NpgsqlConnection(configuration.GetValue<string>("DatabaseSettings:ConnectionString"));
        }
        public async Task<Coupon> GetDiscount(string productName)
        {
            var query= "SELECT * FROM Coupon WHERE ProductName = @ProductName";
            var coupon = await _connectionString.QueryFirstOrDefaultAsync<Coupon>(query, new { ProductName = productName });

            if (coupon is null) return new Coupon{ProductName = "No Discount", Amount = 0, Description = "No Discount Desc"};
            
            return coupon;
        }

        public async Task<bool> CreateDiscount(Coupon coupon)
        {

            var query = $"INSERT INTO COUPON (PRODUCTNAME, DESCRIPTION, AMOUNT) VALUES " +
                        $"(@ProductName, @Description, @Amount";

            var param = new { coupon.ProductName, coupon.Description, coupon.Amount };

            return await ExecuteQuery(query, param);
        }

        public async Task<bool> UpdateDiscount(Coupon coupon)
        {
            var query =
                $"UPDATE COUPON SET PRODUCTNAME = @ProductName, DESCRIPTION = @Description, AMOUNT = @Amount WHERE ID = @Id) ";

            var param = new { coupon.ProductName, coupon.Description, coupon.Amount, coupon.Id };

            return await ExecuteQuery(query, param);
        }

        public async Task<bool> DeleteDiscount(string productName)
        {
            var query =
                $"DELETE FROM COUPON WHERE PRODUCTNAME = @ProductName) ";

            return await ExecuteQuery(query, new { productName } );
        }

        private async Task<bool> ExecuteQuery(string query, object param)
        {
            var affected = await _connectionString.ExecuteAsync(query, param);

            return affected != 0 ;
        }
    }
}
