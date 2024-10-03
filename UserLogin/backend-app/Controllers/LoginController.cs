using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using backend_app.Models;
using System.Collections.Generic;
using System.Security.Cryptography;
using Org.BouncyCastle.Bcpg;
using ZstdSharp.Unsafe;


namespace backend_app.Controllers {
    [ApiController]
    [Route("api/[controller]")]
    public class LoginController : ControllerBase {
        private readonly string connectionString = "Server=localhost;Database=UserDb;User=root;Password=Ibuki0606#;";
        [HttpPost("create")]
        public IActionResult CreateUser([FromBody] Users user) {
            MySqlConnection connection = new MySqlConnection(connectionString);
            try {
                connection.Open();
                string innsertQuery = "INSERT INTO Users(Name,Email,Password)  VALUES(@Name,@Email,@PasswordHash)";
                using(MySqlCommand com = new MySqlCommand(innsertQuery, connection)) {
                    com.Parameters.AddWithValue("@Name", user.Name);
                    com.Parameters.AddWithValue("@Email", user.Email);
                    com.Parameters.AddWithValue("@PasswordHash", user.PasswordHash);

                    com.ExecuteNonQuery();
                }

                return Ok(new
                {
                    message = "接続できました！"
                });
            }
            catch(Exception ex) {
                return StatusCode(500, ex.Message);
            }
        }


    }
}
