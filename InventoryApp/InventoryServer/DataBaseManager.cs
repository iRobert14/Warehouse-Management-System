using Npgsql;
namespace InventoryServer
{ 


   public static class DataBaseManager
    {
        public static bool CheckUser(string username, string passwordHash)
        {
            using (var conn = new NpgsqlConnection(Program.ConnectionString))
            {
                conn.Open();
                // Căutăm dacă există un user cu acest nume ȘI acest hash
                string sql = "SELECT COUNT(*) FROM users WHERE username = @u AND password_hash = @p";
                using (var cmd = new NpgsqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("u", username);
                    cmd.Parameters.AddWithValue("p", passwordHash);

                    long count = (long)cmd.ExecuteScalar();
                    return count > 0; // Returnează true dacă l-a găsit
                }
            }
        }
    }



}
