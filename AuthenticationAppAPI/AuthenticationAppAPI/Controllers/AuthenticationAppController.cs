using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using AuthenticationAppAPI.Models;

namespace AuthenticationAppAPI.Controllers
{
    public class AuthenticationAppController : ApiController
    {
        string CS = ConfigurationManager.ConnectionStrings["DC"].ConnectionString;
        
        // GET api/values
        [HttpGet]
        [Route("api/auth")]
        public IHttpActionResult Get()
        {
            try
            {
                var Users = GetTodos();
                return Ok(Users);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        // GET: api/AuthenticationApp/5
        [HttpGet]
        [Route("api/auth/{id}")]
        public IHttpActionResult Get(Guid? id)
        {
            try
            {
                User User = null;
                using (SqlConnection sqlCon = new SqlConnection(CS))
                {
                    sqlCon.Open();
                    string query = "SELECT * FROM UserTable WHERE Id = @Id";
                    SqlCommand cmd = new SqlCommand(query, sqlCon);
                    cmd.Parameters.AddWithValue("@Id", id);

                    SqlDataReader rdr = cmd.ExecuteReader();

                    if (rdr.Read())
                    {
                        User = new User();
                        User.Id = Guid.Parse(rdr["Id"].ToString());
                        User.Name = rdr["Name"].ToString();
                        User.Age = Convert.ToInt32(rdr["Age"].ToString());
                    }
                }
                return Ok(User);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }


        // POST: api/AuthenticationApp
        [HttpPost]
        [Route("api/auth")]
        public IHttpActionResult Post([FromBody] User Usuario)
        {
            try
            {
                Usuario.Id = Guid.NewGuid();
                using (SqlConnection sqlCon = new SqlConnection(CS))
                {
                    sqlCon.Open();
                    string query = "INSERT INTO UserTable VALUES(@Id, @Name, @Age)";
                    SqlCommand sqlCmd = new SqlCommand(query, sqlCon);
                    sqlCmd.Parameters.AddWithValue("@Id", Usuario.Id);
                    sqlCmd.Parameters.AddWithValue("@Name", Usuario.Name);
                    sqlCmd.Parameters.AddWithValue("@Age", Usuario.Age);
                    sqlCmd.ExecuteNonQuery();
                }
                var userList = GetTodos();
                return Ok(userList);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }


        // PUT: api/AuthenticationApp/5
        [HttpPut]
        [Route("api/auth/{id}")]
        public IHttpActionResult Put(Guid? id, [FromBody] User Usuario)
        {
            try
            {
                Usuario.Id = Guid.NewGuid();
                using (SqlConnection sqlCon = new SqlConnection(CS))
                {
                    sqlCon.Open();
                    string query = "Update UserTable SET Name=@Name, Age=@Age where Id=@Id";
                    SqlCommand sqlCmd = new SqlCommand(query, sqlCon);
                    sqlCmd.Parameters.AddWithValue("@Id", id);
                    sqlCmd.Parameters.AddWithValue("@Name", Usuario.Name);
                    sqlCmd.Parameters.AddWithValue("@Age", Usuario.Age);
                    sqlCmd.ExecuteNonQuery();
                }
                var userList = GetTodos();
                return Ok(userList);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        // DELETE: api/AuthenticationApp/5
        public void Delete(int id)
        {
        }
        public List<User> GetTodos()
        {
            List<User> UserList = new List<User>();
            using (SqlConnection con = new SqlConnection(CS))
            {
                SqlCommand cmd = new SqlCommand("SELECT * FROM UserTable", con);
                cmd.CommandType = CommandType.Text;
                con.Open();
                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    var User = new User();
                    User.Id = Guid.Parse(rdr["Id"].ToString());
                    User.Name = rdr["Name"].ToString();
                    User.Age = Convert.ToInt32(rdr["Age"].ToString());
                    UserList.Add(User);
                }
            }

            return UserList;
        }
    }
}
