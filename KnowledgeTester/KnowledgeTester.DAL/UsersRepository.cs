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


        int IndexId { set; get; }
        int IndexName { set; get; }
        int IndexLastName { set; get; }
        int IndexLogin { set; get; }
        int IndexPassword { set; get; }
        int IndexUserTestId { set; get; }
        int IndexTestId { set; get; }
        int IndexScore { set; get; }
        int IndexTime { set; get; }
        int IndexUserAnswerId { set; get; }
        int IndexQuestionNumber { set; get; }
        int IndexAnswerId { set; get; }



        private User GetUser(Guid id)
        {
            User user;
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                var cmd = new SqlCommand("GetUser",
                                         connection);
                cmd.Parameters.AddWithValue("@id", id);
                cmd.CommandType = CommandType.StoredProcedure;

                connection.Open();
                var reader = cmd.ExecuteReader();
                    
                 SetReaderIndexes(reader);


                user = User(reader);

            }
            return user;
        }

        private void SetReaderIndexes(SqlDataReader reader)
        {
            IndexId = reader.GetOrdinal("Id");
            IndexName = reader.GetOrdinal("Name");
            IndexLastName = reader.GetOrdinal("LastName");
            IndexLogin = reader.GetOrdinal("Login");
            IndexPassword = reader.GetOrdinal("Password");
            IndexUserTestId = reader.GetOrdinal("UserTestId");
            IndexTestId = reader.GetOrdinal("TestId");
            IndexScore = reader.GetOrdinal("Score");
            IndexTime = reader.GetOrdinal("Time");
            IndexUserAnswerId = reader.GetOrdinal("UserAnswerId");
            IndexQuestionNumber = reader.GetOrdinal("QuestionNumber");
            IndexAnswerId = reader.GetOrdinal("AnswerId");
        }

        public User GetUserByLoginPassword(string login, string password)
        {
            User user;
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

                SetReaderIndexes(reader);
                user = User(reader);

            }
            return user;
        }

        private User User(SqlDataReader reader)
        {
            User user = null;
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    user = new User()
                    {
                        Id = reader.GetGuid(IndexId),
                        Name = reader.GetString(IndexName),
                        LastName = reader.GetString(IndexLastName),
                        Login = reader.GetString(IndexLogin),
                        Password = reader.GetString(IndexPassword),
                        TakenTests = new List<UserTest>()
                    };

                    if (!(reader[IndexTestId] is System.DBNull))
                    {
                        var tests = user.TakenTests.Where(q => q.Id == reader.GetGuid(IndexUserTestId)).ToList();
                        if (!tests.Any())
                        {
                            var test = TestsRepo.GetTest(reader.GetGuid(IndexTestId));

                            Answer answer = null;
                            foreach (var a in from q in test.Question
                                where q.Number == reader.GetInt32(IndexQuestionNumber)
                                from a in q.Answers
                                where a.Id == reader.GetGuid(IndexAnswerId)
                                select a)
                            {
                                answer = a;
                            }

                            user.TakenTests.Add(new UserTest()
                            {
                                Id = reader.GetGuid(IndexUserTestId),
                                Test = test,
                                Score = reader.GetDecimal(IndexScore),
                                Time = (DateTime) reader[IndexTime],
                                Answers = new List<UserAnswer>()
                                {
                                    new UserAnswer()
                                    {
                                        Id = reader.GetGuid(IndexUserAnswerId),
                                        QuestionNumber = reader.GetInt32(IndexQuestionNumber),
                                        Answer = answer
                                    }
                                }
                            });
                        }
                    }
                }
            }
            return user;
        }

        public void SaveUser(User user)
        {
            var dbUser = GetUser(user.Id);
            if (dbUser == null)
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    connection.Open();
                    SqlCommand cmd = connection.CreateCommand();
                    ExecuteAddUserCmd(user, cmd);
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
                        ExecuteAddUserTestCmd(user, command, newUserTest);

                        foreach (var answer in newUserTest.Answers)
                        {
                            ExecuteAddUserAnswerCmd(command, answer, newUserTest);
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

        private static void ExecuteAddUserCmd(User user, SqlCommand cmd)
        {
            cmd.CommandText = "AddUser";
            cmd.Parameters.AddWithValue("@id", user.Id);
            cmd.Parameters.AddWithValue("@name", user.Name);
            cmd.Parameters.AddWithValue("@lastName", user.LastName);
            cmd.Parameters.AddWithValue("@login", user.Login);
            cmd.Parameters.AddWithValue("@password", user.Password);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.ExecuteNonQuery();
        }

        private static void ExecuteAddUserAnswerCmd(SqlCommand command, UserAnswer answer, UserTest newUserTest)
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

        private static void ExecuteAddUserTestCmd(User user, SqlCommand command, UserTest newUserTest)
        {
            command.CommandText = "AddUserTest";
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@id", newUserTest.Id);
            command.Parameters.AddWithValue("@testId", newUserTest.Test.Id);
            command.Parameters.AddWithValue("@score", newUserTest.Score);
            command.Parameters.AddWithValue("@time", newUserTest.Time);
            command.Parameters.AddWithValue("@userId", user.Id);
            command.ExecuteNonQuery();
        }
    }
}
