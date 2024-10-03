using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using backend_app.Models;
using System.Collections.Generic;
using System.Security.Cryptography;
using Org.BouncyCastle.Bcpg;
using ZstdSharp.Unsafe;
using Org.BouncyCastle.Tls;


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

                string checkUserQuery = "SELECT COUNT(*) FROM Users WHERE Email = @Email";
                using(MySqlCommand checkCommand = new MySqlCommand(checkUserQuery, connection)) {
                    checkCommand.Parameters.AddWithValue("@Email", user.Email);
                    int userCount = Convert.ToInt32(checkCommand.ExecuteScalar());

                    if(userCount > 0) {
                        return BadRequest(new
                        {
                            message = "このメールアドレスはすでに登録されています"
                        });
                    }
                }

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

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginModels loginRquest) {
            using(MySqlConnection connection = new MySqlConnection(connectionString)) {
                try {
                    connection.Open();
                    string selectQuery = "SELECT Password FROM Users WHERE Email = @Email";
                    using(MySqlCommand com = new MySqlCommand(selectQuery, connection)) {
                        com.Parameters.AddWithValue("@Email", loginRquest.Email);
                        var storedPassword = com.ExecuteScalar() as string;

                        if(storedPassword == null) {
                            return Unauthorized(new
                            {
                                message = "メールアドレスが見つかりません"
                            });
                        }

                        if(storedPassword == loginRquest.Password) {
                            return Ok(new
                            {
                                message = "ログインしました"
                            });
                        }
                        else {
                            return Unauthorized(new
                            {
                                message = "パスワードが違います"
                            });
                        }

                    }

                }
                catch(Exception ex) {
                    return StatusCode(500, new
                    {
                        ex.Message
                    });
                }
            }
        }



    }
}

