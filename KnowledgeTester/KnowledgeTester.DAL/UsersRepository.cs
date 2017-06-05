using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KnowledgeTester.IDAL;
using KnowledgeTester.Model;

namespace KnowledgeTester.DAL
{
    class UsersRepository : Repository, IUsersRepository
    {
        public ITestsRepository TestsRepo { set; get; }

        public User GetUser(Guid id)
        {
            string _connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            User user = null;
            using (SqlConnection connection = new SqlConnection())
            {
                connection.ConnectionString = _connectionString;



                var cmd = new SqlCommand("GetUser",
                                         connection);
                cmd.Parameters.AddWithValue("@id", id);
                cmd.CommandType = CommandType.StoredProcedure;

                connection.Open();
                var reader = cmd.ExecuteReader();
                    
                int indexId = reader.GetOrdinal("Id");
                int indexName = reader.GetOrdinal("Name");
                int indexLastName = reader.GetOrdinal("LastName");
                int indexLogin = reader.GetOrdinal("Login");
                int indexPassword = reader.GetOrdinal("Password");
                int indexUserTestId = reader.GetOrdinal("UserTestId");
                int indexTestId = reader.GetOrdinal("TestId");
                int indexScore = reader.GetOrdinal("Score");
                int indexTime = reader.GetOrdinal("Time");
                int indexUserAnswerId = reader.GetOrdinal("UserAnswerId");
                int indexQuestionNumber = reader.GetOrdinal("QuestionNumber");
                int indexAnswerId = reader.GetOrdinal("AnswerId");



                if (reader.HasRows)
                {

                    while (reader.Read())
                    {
                        if (user == null)
                        {
                            user = new User()
                            {
                                Id = reader.GetGuid(indexId),
                                Name = reader.GetString(indexName),
                                LastName = reader.GetString(indexLastName),
                                Login = reader.GetString(indexLogin),
                                Password = reader.GetString(indexPassword),
                                TakenTests =  new List<UserTest>()


                            };
                        }

                        if (!(reader[indexTestId] is System.DBNull))
                        {

                            var tests = user.TakenTests.Where(q => q.Id == reader.GetGuid(indexUserTestId)).ToList();
                            if (!tests.Any())
                            {
                                var test = TestsRepo.GetTest(reader.GetGuid(indexTestId));

                                Answer answer = null;
                                foreach (var a in from q in test.Question
                                    where q.Number == reader.GetInt32(indexQuestionNumber)
                                    from a in q.Answers
                                    where a.Id == reader.GetGuid(indexAnswerId)
                                    select a)
                                {
                                    answer = a;
                                }

                                user.TakenTests.Add(new UserTest()
                                {
                                    Id = reader.GetGuid(indexUserTestId),
                                    Test = test,
                                    Score = reader.GetDecimal(indexScore),
                                    Time = reader.GetDateTimeOffset(indexTime).DateTime,

                                    Answers = new List<UserAnswer>()
                                    {
                                        new UserAnswer()
                                        {
                                            Id = reader.GetGuid(indexUserAnswerId),
                                            QuestionNumber = reader.GetInt32(indexQuestionNumber),
                                            Answer = answer

                                        }
                                    }
                                });

                            }


                        }


                    }

                }

            }
            return user;
        }

        public User GetUserByLoginPassword(string login, string password)
        {
            string _connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            User user = null;
            using (SqlConnection connection = new SqlConnection())
            {
                connection.ConnectionString = _connectionString;



                var cmd = new SqlCommand("GetUserByLoginPassword",
                                         connection);
                cmd.Parameters.AddWithValue("@login", login);
                cmd.Parameters.AddWithValue("@password", password);
                cmd.CommandType = CommandType.StoredProcedure;

                connection.Open();
                var reader = cmd.ExecuteReader();

                int indexId = reader.GetOrdinal("Id");
                int indexName = reader.GetOrdinal("Name");
                int indexLastName = reader.GetOrdinal("LastName");
                int indexLogin = reader.GetOrdinal("Login");
                int indexPassword = reader.GetOrdinal("Password");
                int indexUserTestId = reader.GetOrdinal("UserTestId");
                int indexTestId = reader.GetOrdinal("TestId");
                int indexScore = reader.GetOrdinal("Score");
                int indexTime = reader.GetOrdinal("Time");
                int indexUserAnswerId = reader.GetOrdinal("UserAnswerId");
                int indexQuestionNumber = reader.GetOrdinal("QuestionNumber");
                int indexAnswerId = reader.GetOrdinal("AnswerId");



                if (reader.HasRows)
                {

                    while (reader.Read())
                    {
                        if (user == null)
                        {
                            user = new User()
                            {
                                Id = reader.GetGuid(indexId),
                                Name = reader.GetString(indexName),
                                LastName = reader.GetString(indexLastName),
                                Login = reader.GetString(indexLogin),
                                Password = reader.GetString(indexPassword),
                                TakenTests = new List<UserTest>()


                            };
                        }
                        if (!(reader[indexTestId] is System.DBNull))
                        {

                            var tests = user.TakenTests.Where(q => q.Id == reader.GetGuid(indexUserTestId)).ToList();
                            if (!tests.Any())
                            {
                                var test = TestsRepo.GetTest(reader.GetGuid(indexTestId));

                                Answer answer = null;
                                foreach (var a in from q in test.Question
                                    where q.Number == reader.GetInt32(indexQuestionNumber)
                                    from a in q.Answers
                                    where a.Id == reader.GetGuid(indexAnswerId)
                                    select a)
                                {
                                    answer = a;
                                }

                                user.TakenTests.Add(new UserTest()
                                {
                                    Id = reader.GetGuid(indexUserTestId),
                                    Test = test,
                                    Score = reader.GetDecimal(indexScore),
                                    Time = reader.GetDateTimeOffset(indexTime).DateTime,

                                    Answers = new List<UserAnswer>()
                                    {
                                        new UserAnswer()
                                        {
                                            Id = reader.GetGuid(indexUserAnswerId),
                                            QuestionNumber = reader.GetInt32(indexQuestionNumber),
                                            Answer = answer

                                        }
                                    }
                                });

                            }

                        }
                    }

                }

            }
            return user;
        }

        public IList<User> GetUsers()
        {
            throw new NotImplementedException();
        }

        public void SaveUser(User user)
        {
            var dbUser = GetUser(user.Id);
            if (dbUser == null)
            {
                using (SqlConnection connection = new SqlConnection())
                {
                    connection.ConnectionString = _connectionString;
                    
                    var cmd = new SqlCommand("AddUser",
                        connection);
                    cmd.Parameters.AddWithValue("@id", user.Id);
                    cmd.Parameters.AddWithValue("@name", user.Name);
                    cmd.Parameters.AddWithValue("@lastName", user.LastName);
                    cmd.Parameters.AddWithValue("@login", user.Login);
                    cmd.Parameters.AddWithValue("@password", user.Password);
                    cmd.CommandType = CommandType.StoredProcedure;

                    connection.Open();
                    cmd.ExecuteNonQuery();
                }

            }
            else
            {
                UserTest newUserTest = null;
                foreach (var test in user.TakenTests.Where(test => dbUser.TakenTests.All(t => t.Id != test.Id)))
                {
                    newUserTest = test;
                }


                if (newUserTest != null)
                {

                

                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    connection.Open();

                    // Start a local transaction.
                    SqlTransaction sqlTran = connection.BeginTransaction();

                    // Enlist a command in the current transaction.
                    SqlCommand command = connection.CreateCommand();
                    command.Transaction = sqlTran;


                    try
                    {
                        command.CommandText = "AddUserTest";
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@id", newUserTest.Id);
                        command.Parameters.AddWithValue("@testId", newUserTest.Test.Id);
                        command.Parameters.AddWithValue("@score", newUserTest.Score);
                        command.Parameters.AddWithValue("@time", newUserTest.Time);
                        command.Parameters.AddWithValue("@userId", user.Id);
                        command.ExecuteNonQuery();

                        foreach (var answer in newUserTest.Answers)
                        {
                            command.CommandText = "AddUserAnswer";
                            command.CommandType = CommandType.StoredProcedure;
                            command.Parameters.Clear();
                            command.Parameters.AddWithValue("@id", answer.Id);
                            command.Parameters.AddWithValue("@questionNumber", answer.QuestionNumber);
                            command.Parameters.AddWithValue("@userTestId", newUserTest.Id);
                            command.Parameters.AddWithValue("@answerId", answer.Answer.Id);
                            command.ExecuteNonQuery();

                        }

                        // Commit the transaction.
                        sqlTran.Commit();
                    }
                    catch (Exception ex)
                    {
                        //TODO add logging

                        try
                        {
                            // Attempt to roll back the transaction.
                            sqlTran.Rollback();
                        }
                        catch (Exception exRollback)
                        {
                            //TODO add logging
                        }
                    }
                }




            }

        }
        }
    }
}
