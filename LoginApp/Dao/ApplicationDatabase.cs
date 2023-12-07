using LoginApp.Interfaces;
using LoginApp.Models;
using Oracle.ManagedDataAccess.Client;
using System.Data;


namespace LoginApp.Dao
{
    public class ApplicationDatabase: IApplicationDatabase
    {
        public void RegisterUser(UserModel User)
        {
            var connectionString = Environment.GetEnvironmentVariable("OracleDBConnection");
            using (OracleConnection conn = new OracleConnection(connectionString))
            {
                OracleCommand cmd = new OracleCommand("INSERT INTO PRAC_USER (Name, Email, Password) VALUES (:Name, :Email, :Password)", conn)
                {
                    CommandType = CommandType.Text,
                    BindByName = true
                };
                try
                {
                    cmd.Parameters.Add("Name", OracleDbType.Varchar2, ParameterDirection.Input).Value =User.Name;
                    cmd.Parameters.Add("Email", OracleDbType.Varchar2, ParameterDirection.Input).Value =User.Email;
                    cmd.Parameters.Add("Password", OracleDbType.Varchar2, ParameterDirection.Input).Value =User.Password;

                    conn.Open();
                    cmd.ExecuteNonQuery();

                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.StackTrace);
                }
                finally
                {
                    if (conn.State == ConnectionState.Open)
                    {
                        conn.Dispose();
                        conn.Close();
                    }
                    cmd.Dispose();
                }
            }
        }

        public List<UserModel> CheckEmail(string Email)
        {
            List<UserModel> UserData = new List<UserModel>();
            var connectionString = Environment.GetEnvironmentVariable("OracleDBConnection");
            using (OracleConnection conn = new OracleConnection(connectionString))
            {
                OracleCommand cmd = new OracleCommand("SELECT * FROM PRAC_USER WHERE Email = :Email", conn)
                {
                    CommandType = CommandType.Text,
                    BindByName = true
                };
                try
                {
                    cmd.Parameters.Add("Email", OracleDbType.Varchar2, ParameterDirection.Input).Value = Email;

                    conn.Open();
                    using (OracleDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            UserModel user = new UserModel
                            {
                                Id = Convert.ToInt32(reader["Id"]),
                                Name = reader["Name"].ToString(),
                                Email = reader["Email"].ToString(),
                                Password = reader["Password"].ToString()

                            };

                            UserData.Add(user);
                        }
                    }

                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.StackTrace);

                }
                finally
                {
                    if (conn.State == ConnectionState.Open)
                    {
                        conn.Dispose();
                        conn.Close();
                    }
                    cmd.Dispose();
                }
            }
            return UserData;
        
    }

        public List<DevModel> GetDevList()
        {
            List<DevModel> DevData = new List<DevModel>();
            var connectionString = Environment.GetEnvironmentVariable("OracleDBConnection");
            using (OracleConnection conn = new OracleConnection(connectionString))
            {
                OracleCommand cmd = new OracleCommand("SELECT * FROM PRAC_LA_DEV", conn)
                {
                    CommandType = CommandType.Text,
                    BindByName = true
                };
                try
                {
                    conn.Open();
                    using (OracleDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            DevModel dev = new DevModel
                            {
                                Id = Convert.ToInt32(reader["Id"]),
                                Name = reader["Name"].ToString(),
                            };

                            DevData.Add(dev);
                        }
                    }

                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.StackTrace);

                }
                finally
                {
                    if (conn.State == ConnectionState.Open)
                    {
                        conn.Dispose();
                        conn.Close();
                    }
                    cmd.Dispose();
                }
            }
            return DevData;
        }
    }
}
