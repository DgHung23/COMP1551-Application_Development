using Microsoft.Data.SqlClient;
using MVCConsoleApp.Data.Entities;

namespace MVCConsoleApp.Data.Services
{
    public class PersonRoleServices
    {
        private readonly string connectionString = DbConfig.ConnectionString;

        public bool Create(PersonRole personRole)
        {
            bool valid = false;
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sql = "INSERT INTO PersonRoles ([Name], [Description]) OUTPUT INSERTED.Id VALUES (@name, @description)";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@name", personRole.Name);
                        command.Parameters.AddWithValue("@description", string.IsNullOrEmpty(personRole.Description) ? (object)DBNull.Value : personRole.Description);

                        var executed = command.ExecuteScalar();
                        Console.WriteLine($"data vừa thêm row:\n{executed}");
                        valid = true;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return valid;
        }

        public List<PersonRole> GetAll()
        {
            var result = new List<PersonRole>();
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "SELECT Id, Name, [Description] FROM PersonRoles ORDER BY Id";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            result.Add(MapRole(reader));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return result;
        }

        public PersonRole? GetById(int id)
        {
            PersonRole? result = null;
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "SELECT Id, Name, [Description] FROM PersonRoles WHERE Id = @Id";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Id", id);
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                result = MapRole(reader);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return result;
        }

        public bool Update(PersonRole personRole)
        {
            bool valid = false;
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sql = "UPDATE PersonRoles SET [Name] = @name, [Description] = @description WHERE [Id] = @id";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@id", personRole.Id);
                        command.Parameters.AddWithValue("@name", personRole.Name);
                        command.Parameters.AddWithValue("@description", string.IsNullOrEmpty(personRole.Description) ? (object)DBNull.Value : personRole.Description);

                        int rowsAffected = command.ExecuteNonQuery();
                        Console.WriteLine($"Số dòng đã cập nhật: {rowsAffected}");
                        valid = rowsAffected > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return valid;
        }

        public bool Delete(int id)
        {
            bool valid = false;
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "DELETE FROM PersonRoles WHERE Id = @Id";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Id", id);
                        valid = command.ExecuteNonQuery() > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return valid;
        }

        private static PersonRole MapRole(SqlDataReader reader)
        {
            int descriptionOrdinal = reader.GetOrdinal("Description");
            return new PersonRole
            {
                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                Name = reader.GetString(reader.GetOrdinal("Name")),
                Description = reader.IsDBNull(descriptionOrdinal) ? null : reader.GetString(descriptionOrdinal)
            };
        }
    }
}
