using LoginApp.Interfaces;
using LoginApp.Models;
using Oracle.ManagedDataAccess.Client;
using System.Data;
using System.Threading.Tasks;


namespace LoginApp.Dao
{
    public class ApplicationDatabase : IApplicationDatabase
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

        public List<TaskModel> GetTaskList()
        {
            List<TaskModel> TaskData = new List<TaskModel>();
            var connectionString = Environment.GetEnvironmentVariable("OracleDBConnection");
            using (OracleConnection conn = new OracleConnection(connectionString))
            {
                OracleCommand cmd = new OracleCommand("SELECT * FROM PRAC_LA_TASK ORDER BY ID", conn)
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
                            TaskModel task = new TaskModel
                            {
                                Id = Convert.ToInt32(reader["Id"]),
                                TaskTitle = reader["Task_Title"].ToString(),
                                AssignedTo = Convert.ToInt32(reader["Assigned_To"]),
                                AssignedBy = Convert.ToInt32(reader["Assigned_By"]),
                                TaskDetails = reader.GetString(reader.GetOrdinal("Details")),                                
                                AssignedDate = reader.GetDateTime("Created_At"),
                                Status = Convert.ToInt32(reader["status"]),

                            };

                            TaskData.Add(task);
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

                return TaskData;
            }
        }

        public List<UserModel> GetUserById(int id)
        {
            List<UserModel> IdUser = new List<UserModel>();
            var connectionString = Environment.GetEnvironmentVariable("OracleDBConnection");
            using (OracleConnection conn = new OracleConnection(connectionString))
            {
                OracleCommand cmd = new OracleCommand("SELECT * FROM PRAC_USER WHERE id =:id ", conn)
                {
                    CommandType = CommandType.Text,
                    BindByName = true
                };
                try
                {
                    cmd.Parameters.Add("Id", OracleDbType.Varchar2, ParameterDirection.Input).Value = id;
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

                            };

                            IdUser.Add(user);
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

                return IdUser;
            }
        }

        public List<DevModel> GetDevById(int id)
        {
            List<DevModel> IdUser = new List<DevModel>();
            var connectionString = Environment.GetEnvironmentVariable("OracleDBConnection");
            using (OracleConnection conn = new OracleConnection(connectionString))
            {
                OracleCommand cmd = new OracleCommand("SELECT * FROM PRAC_LA_DEV WHERE id =:id ", conn)
                {
                    CommandType = CommandType.Text,
                    BindByName = true
                };
                try
                {
                    cmd.Parameters.Add("Id", OracleDbType.Varchar2, ParameterDirection.Input).Value = id;
                    conn.Open();
                    using (OracleDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            DevModel user = new DevModel
                            {
                                Id = Convert.ToInt32(reader["Id"]),
                                Name = reader["Name"].ToString(),

                            };

                            IdUser.Add(user);
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

                return IdUser;
            }
        }

        public List<TaskModel> GetTaskById(int id)
        {
            List<TaskModel> IdTask = new List<TaskModel>();
            var connectionString = Environment.GetEnvironmentVariable("OracleDBConnection");
            using (OracleConnection conn = new OracleConnection(connectionString))
            {
                OracleCommand cmd = new OracleCommand("SELECT * FROM PRAC_LA_TASK WHERE id =:id ", conn)
                {
                    CommandType = CommandType.Text,
                    BindByName = true
                };
                try
                {
                    cmd.Parameters.Add("Id", OracleDbType.Varchar2, ParameterDirection.Input).Value = id;
                    conn.Open();
                    using (OracleDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            TaskModel task = new TaskModel
                            {
                                Id = Convert.ToInt32(reader["Id"]),
                                TaskTitle = reader["Title"].ToString(),
                                AssignedTo = Convert.ToInt32(reader["Assigned_To"]),
                                AssignedBy = Convert.ToInt32(reader["Assigned_By"]),
                                TaskDetails = reader.GetString(reader.GetOrdinal("Details")),
                                AssignedDate = reader.GetDateTime("Created_At"),

                            };

                            IdTask.Add(task);
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

                return IdTask;
            }
        }

        public void AddTask(TaskModel task)
        {
            var connectionString = Environment.GetEnvironmentVariable("OracleDBConnection");
            using (OracleConnection conn = new OracleConnection(connectionString))
            {
                OracleCommand cmd = new OracleCommand("INSERT INTO PRAC_LA_TASK (TASK_TITLE, ASSIGNED_TO, ASSIGNED_BY, DETAILS) VALUES (:TaskTitle, :AssignedTo, :AssignedBy, :Details)", conn)
                {
                    CommandType = CommandType.Text,
                    BindByName = true
                };
                try
                {
                    cmd.Parameters.Add("TaskTitle", OracleDbType.Varchar2, ParameterDirection.Input).Value =task.TaskTitle;
                    cmd.Parameters.Add("AssignedTo", OracleDbType.Varchar2, ParameterDirection.Input).Value =task.AssignedTo;
                    cmd.Parameters.Add("AssignedBy", OracleDbType.Varchar2, ParameterDirection.Input).Value =task.AssignedBy;
                    cmd.Parameters.Add("Details", OracleDbType.Varchar2, ParameterDirection.Input).Value =task.TaskDetails;

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

        public void DeleteTask(int id)
        {
            var connectionString = Environment.GetEnvironmentVariable("OracleDBConnection");
            using (OracleConnection conn = new OracleConnection(connectionString))
            {
                OracleCommand cmd = new OracleCommand("DELETE PRAC_LA_TASK WHERE ID =: id", conn)
                {
                    CommandType = CommandType.Text,
                    BindByName = true
                };
                try
                {
                    cmd.Parameters.Add("id", OracleDbType.Varchar2, ParameterDirection.Input).Value =id;
                    

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

        public List<TaskModel> TaskDetailsById(int id)
        {
            List<TaskModel> TaskData = new List<TaskModel>();
            var connectionString = Environment.GetEnvironmentVariable("OracleDBConnection");
            using (OracleConnection conn = new OracleConnection(connectionString))
            {
                OracleCommand cmd = new OracleCommand("SELECT * FROM PRAC_LA_TASK WHERE ID =: id", conn)
                {
                    CommandType = CommandType.Text,
                    BindByName = true
                };
                try
                {
                    cmd.Parameters.Add("id", OracleDbType.Varchar2, ParameterDirection.Input).Value =id;
                    conn.Open();
                    using (OracleDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            TaskModel task = new TaskModel
                            {
                                Id = Convert.ToInt32(reader["Id"]),
                                TaskTitle = reader["Task_Title"].ToString(),
                                AssignedTo = Convert.ToInt32(reader["Assigned_To"]),
                                AssignedBy = Convert.ToInt32(reader["Assigned_By"]),
                                TaskDetails = reader.GetString(reader.GetOrdinal("Details")),
                                AssignedDate = reader.GetDateTime("Created_At"),

                            };

                            TaskData.Add(task);
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

                return TaskData;
            }
        }

        public void TaskStatusUpdate(int id)
        {
            var connectionString = Environment.GetEnvironmentVariable("OracleDBConnection");
            using (OracleConnection conn = new OracleConnection(connectionString))
            {
                OracleCommand cmd = new OracleCommand("UPDATE PRAC_LA_TASK SET STATUS = 1 WHERE ID =: id", conn)
                {
                    CommandType = CommandType.Text,
                    BindByName = true
                };
                try
                {
                    cmd.Parameters.Add("id", OracleDbType.Varchar2, ParameterDirection.Input).Value =id;


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
    }

}


