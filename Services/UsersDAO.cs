using HealthVet.Models;
using System.Data.SqlClient;

namespace HealthVet.Services
{
    public class UsersDAO
    {
            string connectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=HealthPet;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";

        public UsersModel ValidateCredentials(string email, string password)
        {
            UsersModel userFound = null;

            string sqlStatement = "SELECT * FROM dbo.USERS  WHERE email = @email AND password=@password";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(sqlStatement, connection);
                command.Parameters.AddWithValue("@email", email);
                command.Parameters.AddWithValue("@password", password);
                try
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        userFound = new UsersModel { id = (int)reader[0], name = (string)reader[1], lastname = (string)reader[2], idcard = (int)reader[3], phone = (String)reader[4], email = (string)reader[5], password = (string)reader[6], rol = "cliente" };
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                finally
                {
                    connection.Close();
                }
            }
            return userFound;
        }

        public int Insert(UsersModel user)
        {
            int new_id_user = -1;
            string sqlStatement = "INSERT INTO dbo.USERS (name, lastname, idcard, phone, email, password) VALUES (@name, @lastname, @idcard, @phone, @email, @password)";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(sqlStatement, connection);
                command.Parameters.AddWithValue("@name", user.name);
                command.Parameters.AddWithValue("@lastname", user.lastname);
                command.Parameters.AddWithValue("@idcard", user.idcard);
                command.Parameters.AddWithValue("@phone", user.phone);
                command.Parameters.AddWithValue("@email", user.email);
                command.Parameters.AddWithValue("@password", user.password);

                try
                {
                    connection.Open();
                    new_id_user = Convert.ToInt32(command.ExecuteScalar());

                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                finally
                {
                    connection.Close();
                }
            }
            return new_id_user;
        }

        
    }
}
