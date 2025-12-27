using InventoryCommon.Models;
using Npgsql;


namespace InventoryServer
{


    public static class DataBaseManager
    {
        public static User Authenticate(string username, string passwordHash)
        {
            using (var conn = new NpgsqlConnection(Program.ConnectionString))
            {
                conn.Open();
                // Căutăm dacă există un user cu acest nume ȘI acest hash
                string sql = "SELECT id, username, role FROM users WHERE username = @u AND password_hash = @p";

                using (var cmd = new NpgsqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("u", username);
                    cmd.Parameters.AddWithValue("p", passwordHash);

                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new User
                            {
                                UserId = reader.GetInt32(0),
                                Username = reader.GetString(1),
                                UserRole = Enum.Parse<UserRole>(reader.GetString(2))
                            };
                        }

                    }

                }
            }
            return null;
        }

        public static List<Product> GetAllProducts()
        {
            var products = new List<Product>();
            using (var conn = new NpgsqlConnection(Program.ConnectionString))
            {
                conn.Open();

                string sql = "SELECT id, name, sku, price, category, quantity FROM products";
                using (var cmd = new NpgsqlCommand(sql, conn))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            products.Add(new Product
                            {
                                ID = reader.GetGuid(0),
                                Name = reader.GetString(1),
                                SKU = reader.GetString(2),
                                Price = reader.GetDecimal(3),
                                Category = reader.GetString(4),
                                Quantity = (uint)reader.GetInt32(5)
                            });
                        }
                    }

                }

                return products;
            }
        }
    }
}




