using HealthVet.Models;
using System.Data.SqlClient;

namespace HealthVet.Services
{
    public class PetsDAO
    {

        string connectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=HealthPet;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
        public List<BreedsModel> GetAllBreeds()
        {
            List<BreedsModel> BreedsFound = new List<BreedsModel>();

            string sqlStatement = "SELECT dbo.BREEDS.id, dbo.BREEDS.name, dbo.BREEDS.animal_id, dbo.ANIMALS.name FROM dbo.BREEDS INNER JOIN dbo.ANIMALS ON dbo.BREEDS.animal_id=dbo.ANIMALS.id";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(sqlStatement, connection);
                try
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        BreedsFound.Add(new BreedsModel { id = (int)reader[0], name = (string)reader[1], animal_id = (int)reader[2], animal_name = (string)reader[3]});
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
            return BreedsFound;
        }

        public List<PetsModel> GetUserPets(int userId)
        {
            List<PetsModel> foundPets = new List<PetsModel>();
            string sqlStatement = "SELECT dbo.USERS.id, dbo.USERS.name, dbo.PETS.id, dbo.PETS.name, dbo.PETS.age, dbo.BREEDS.id, dbo.BREEDS.name, dbo.ANIMALS.name FROM dbo.USERS JOIN dbo.PETS ON dbo.USERS.id=dbo.PETS.user_id JOIN dbo.BREEDS ON dbo.PETS.breed_id=dbo.BREEDS.id JOIN dbo.ANIMALS ON dbo.BREEDS.animal_id=DBO.ANIMALS.id WHERE dbo.USERS.id=@userId";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(sqlStatement, connection);
                command.Parameters.AddWithValue("@userId", userId);
                try
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        foundPets.Add(new PetsModel { user_id = (int)reader[0], user_name = (string)reader[1], id = (int)reader[2], name = (String)reader[3], age = (int)reader[4], breed_id = (int)reader[5], breed_name = (string)reader[6], animal_name = (string)reader[7] });
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
            return foundPets;
        }

        public int InsertPet(PetsModel pet)
        {
            int new_id_pet = -1;
            string sqlStatement = "INSERT INTO dbo.PETS (name, age, breed_id, user_id) VALUES (@name, @age, @breed_id, @user_id)";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(sqlStatement, connection);
                command.Parameters.AddWithValue("@name", pet.name);
                command.Parameters.AddWithValue("@age", pet.age);
                command.Parameters.AddWithValue("@breed_id", pet.breed_id);
                command.Parameters.AddWithValue("@user_id", pet.user_id);
                try
                {
                    connection.Open();
                    new_id_pet = Convert.ToInt32(command.ExecuteScalar());

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
            return new_id_pet;
        }

        public List<CategoriesModel> GetAllCategories()
        {
            List<CategoriesModel> CategoriesFound = new List<CategoriesModel>();

            string sqlStatement = "SELECT * FROM dbo.CATEGORIES";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(sqlStatement, connection);
                try
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        CategoriesFound.Add(new CategoriesModel { id = (int)reader[0], name = (string)reader[1] });
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
            return CategoriesFound;
        }

        public int AddAppointment(AppointmentsModel appointment)
        {
            int new_id_appointment = -1;
            string sqlStatement = "INSERT INTO dbo.APPOINTMENTS (datetime, user_id, pet_id, category_id) VALUES (@datetime, @user_id, @pet_id, @category_id)";
            if (DateBooked(appointment.datetime) == false)
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    SqlCommand command = new SqlCommand(sqlStatement, connection);
                    command.Parameters.AddWithValue("@datetime", appointment.datetime);
                    command.Parameters.AddWithValue("@user_id", appointment.user_id);
                    command.Parameters.AddWithValue("@pet_id", appointment.pet_id);
                    command.Parameters.AddWithValue("@category_id", appointment.category_id);
                    try
                    {
                        connection.Open();
                        new_id_appointment = Convert.ToInt32(command.ExecuteScalar());

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
                return new_id_appointment;
            }
            else
            {
                return new_id_appointment;
            }
        }
        
        public List<AppointmentsViewModel> GetUserAppointments(int userId)
        {
            List<AppointmentsViewModel> foundAppointments = new List<AppointmentsViewModel>();
            string sqlStatement = "SELECT dbo.APPOINTMENTS.id, .APPOINTMENTS.datetime, dbo.CATEGORIES.name, dbo.PETS.name, dbo.PETS.age, dbo.BREEDS.name, dbo.ANIMALS.name FROM dbo.APPOINTMENTS JOIN dbo.PETS ON dbo.APPOINTMENTS.pet_id=dbo.PETS.id JOIN dbo.BREEDS ON dbo.PETS.breed_id=dbo.BREEDS.id JOIN dbo.ANIMALS ON dbo.BREEDS.animal_id=DBO.ANIMALS.id JOIN dbo.CATEGORIES ON dbo.CATEGORIES.id=dbo.APPOINTMENTS.category_id WHERE dbo.APPOINTMENTS.user_id=@userId";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(sqlStatement, connection);
                command.Parameters.AddWithValue("@userId", userId);
                try
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        foundAppointments.Add(new AppointmentsViewModel { id = (int)reader[0], datetime = Convert.ToDateTime(reader[1]), category_name = (string)reader[2], pet_name = (String)reader[3], age = (int)reader[4], breed_name = (string)reader[5], animal_name = (string)reader[6]});
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
            return foundAppointments;
        }

        public AppointmentsViewModel GetUserAppointmentByDate(int userId, DateTime datetime)
        {
            AppointmentsViewModel foundAppointment = null;
            string sqlStatement = "SELECT dbo.APPOINTMENTS.id, .APPOINTMENTS.datetime, dbo.CATEGORIES.name, dbo.PETS.name, dbo.PETS.age, dbo.BREEDS.name, dbo.ANIMALS.name FROM dbo.APPOINTMENTS JOIN dbo.PETS ON dbo.APPOINTMENTS.pet_id=dbo.PETS.id JOIN dbo.BREEDS ON dbo.PETS.breed_id=dbo.BREEDS.id JOIN dbo.ANIMALS ON dbo.BREEDS.animal_id=DBO.ANIMALS.id JOIN dbo.CATEGORIES ON dbo.CATEGORIES.id=dbo.APPOINTMENTS.category_id WHERE dbo.APPOINTMENTS.user_id=@userId AND dbo.APPOINTMENTS.datetime=@datetime";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(sqlStatement, connection);
                command.Parameters.AddWithValue("@userId", userId);
                command.Parameters.AddWithValue("@datetime", datetime);
                try
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        foundAppointment = new AppointmentsViewModel { id = (int)reader[0], datetime = Convert.ToDateTime(reader[1]), category_name = (string)reader[2], pet_name = (String)reader[3], age = (int)reader[4], breed_name = (string)reader[5], animal_name = (string)reader[6] };
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
            return foundAppointment;
        }

        public AppointmentsModel GetAppointmentById(int appointmentId)
        {
            AppointmentsModel foundAppointment = null;

            string sqlStatement = "SELECT * FROM dbo.APPOINTMENTS WHERE dbo.APPOINTMENTS.id = @appointmentId";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(sqlStatement, connection);
                command.Parameters.AddWithValue("@appointmentId", appointmentId);
                try
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        foundAppointment = new AppointmentsModel { id = (int)reader[0], datetime = Convert.ToDateTime(reader[1]), user_id = (int)reader[2], pet_id = (int)reader[3], category_id = (int)reader[4]};
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
            return foundAppointment;
        }

        public int UpdateAppointment(AppointmentsModel appointment)
        {
            int new_id_appointment = -1;
            string sqlStatement = "UPDATE dbo.APPOINTMENTS SET datetime = @datetime WHERE id = @id";
            if (DateBooked(appointment.datetime) == false)
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    SqlCommand command = new SqlCommand(sqlStatement, connection);
                    command.Parameters.AddWithValue("@id", appointment.id);
                    command.Parameters.AddWithValue("@datetime", appointment.datetime);
                    try
                    {
                        connection.Open();
                        new_id_appointment = Convert.ToInt32(command.ExecuteScalar());

                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                    finally
                    {
                        connection.Close();
                    }
                    return new_id_appointment;
                }
            }
            else
            {
               return new_id_appointment;
            }
                
               
            }
   
        public bool DateBooked(DateTime newDateTime)
        {
            int dateFound = 0;
            string sqlStatement = "SELECT COUNT (*) FROM dbo.APPOINTMENTS WHERE datetime = @newDateTime";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(sqlStatement, connection);
                command.Parameters.AddWithValue("@newDateTime", newDateTime);
                try
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        dateFound = (int)reader[0];
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
            if (dateFound > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        

    }
}
