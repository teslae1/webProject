using bbsongPro.Models;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace bbsongPro.Foundations
{
    public class UserFoundation
    {

        public User GetUser(string type)
        {
            var user = new User();
            using(var conn = new MySqlConnection(Startup.ConnString))
            {
                conn.Open();
                using(var cmd = new MySqlCommand("SELECT * FROM `Users` WHERE type = @type ",conn ))
                {
                    cmd.Parameters.AddWithValue("@type", type);

                    using(var reader = cmd.ExecuteReader())
                    {
                        reader.Read();
                        user.Username = reader["username"].ToString();
                        user.Password = reader["password"].ToString();
                    }
                }
            }
            return user;
        }
    }
}
