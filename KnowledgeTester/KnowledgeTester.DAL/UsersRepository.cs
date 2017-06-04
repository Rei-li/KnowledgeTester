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

                        
                        var tests = user.TakenTests.Where(q => q.Id == reader.GetGuid(indexUserTestId)).ToList();
                        if (!tests.Any())
                        {
                            var test = TestsRepo.GetTest(reader.GetGuid(indexTestId));

                            Answer answer = null;
                            foreach (var a in from q in test.Question where q.Number == reader.GetInt32(indexQuestionNumber) from a in q.Answers where a.Id == reader.GetGuid(indexAnswerId) select a)
                            {
                                answer = a;
                            }

                            user.TakenTests.Add(new UserTest()
                            {
                                Id = reader.GetGuid(indexUserTestId),
                                Test = test,
                                Score = reader.GetDecimal(indexScore),
                                Time = reader.GetDateTimeOffset(indexTime).DateTime,
                                
                                Answers = new List<UserAnswer>() {
                                    new UserAnswer()
                                {
                                    Id = reader.GetGuid(indexUserAnswerId),
                                    QuestionNumber = reader.GetInt32(indexQuestionNumber),
                                    Answer = answer

                                } }
                            });

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
                
            }
            else
            {
                
            }
        }
    }
}
