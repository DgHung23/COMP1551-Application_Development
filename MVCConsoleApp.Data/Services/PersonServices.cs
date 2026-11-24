using Microsoft.Data.SqlClient;
using MVCConsoleApp.Data.Entities;

namespace MVCConsoleApp.Data.Services
{
    public class PersonServices
    {
        private readonly string connectionString = DbConfig.ConnectionString;

        public bool Create(Person person)
        {
            bool valid = false;
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sql = "INSERT INTO People (FullName, Telephone, Email, RoleId) OUTPUT INSERTED.Id VALUES (@fullName, @telephone, @email, @roleId)";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        AddParameters(command, person);
                        command.ExecuteScalar();
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

        public List<Person> GetAll()
        {
            var result = new List<Person>();
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "SELECT p.Id, p.FullName, p.Telephone, p.Email, p.RoleId, r.Name AS RoleName " +
                                   "FROM People p INNER JOIN PersonRoles r ON r.Id = p.RoleId " +
                                   "ORDER BY p.Id";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            result.Add(MapPerson(reader));
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

        public Person? GetById(int id)
        {
            Person? result = null;
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "SELECT p.Id, p.FullName, p.Telephone, p.Email, p.RoleId, r.Name AS RoleName " +
                                   "FROM People p INNER JOIN PersonRoles r ON r.Id = p.RoleId " +
                                   "WHERE p.Id = @Id";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Id", id);
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                result = MapPerson(reader);
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

        public bool Update(Person person)
        {
            bool valid = false;
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sql = "UPDATE People SET FullName = @fullName, Telephone = @telephone, Email = @email, RoleId = @roleId WHERE Id = @id";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@id", person.Id);
                        AddParameters(command, person);
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

        public bool Delete(int id)
        {
            bool valid = false;
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "DELETE FROM People WHERE Id = @Id";
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

        private static void AddParameters(SqlCommand command, Person person)
        {
            command.Parameters.AddWithValue("@fullName", person.FullName);
            command.Parameters.AddWithValue("@telephone", string.IsNullOrEmpty(person.Telephone) ? (object)DBNull.Value : person.Telephone);
            command.Parameters.AddWithValue("@email", string.IsNullOrEmpty(person.Email) ? (object)DBNull.Value : person.Email);
            command.Parameters.AddWithValue("@roleId", person.PersonRoleId);
        }

        private static Person MapPerson(SqlDataReader reader)
        {
            int telephoneOrdinal = reader.GetOrdinal("Telephone");
            int emailOrdinal = reader.GetOrdinal("Email");
            return new Person
            {
                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                FullName = reader.GetString(reader.GetOrdinal("FullName")),
                Telephone = reader.IsDBNull(telephoneOrdinal) ? null : reader.GetString(telephoneOrdinal),
                Email = reader.IsDBNull(emailOrdinal) ? null : reader.GetString(emailOrdinal),
                PersonRoleId = reader.GetInt32(reader.GetOrdinal("RoleId")),
                PersonRoleName = reader.GetString(reader.GetOrdinal("RoleName"))
            };
        }
    }
}
