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
    public class TestsRepository : ITestsRepository
    {


        public void DeleteItem(Guid id)
        {
            string _connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            using (SqlConnection connection = new SqlConnection())
            {

                connection.ConnectionString = _connectionString;



                var cmd = new SqlCommand("DeleteTest",
                    connection);
                cmd.Parameters.AddWithValue("@id", id);
                cmd.CommandType = CommandType.StoredProcedure;

                connection.Open();
                cmd.ExecuteNonQuery();



            }
        }

        public Test GetTest(Guid id)
        {
            string _connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            Test test = null;
            using (SqlConnection connection = new SqlConnection())
            {
                connection.ConnectionString = _connectionString;



                var cmd = new SqlCommand("GetTest",
                                         connection);
                cmd.Parameters.AddWithValue("@id", id);
                cmd.CommandType = CommandType.StoredProcedure;

                connection.Open();
                var reader = cmd.ExecuteReader();

                int indexId = reader.GetOrdinal("Id");
                int indexTheme = reader.GetOrdinal("Theme");
                int indexAuthor = reader.GetOrdinal("Author");
                int indexVersion = reader.GetOrdinal("Version");
                int indexQuestionId = reader.GetOrdinal("QuestionId");
                int indexTask = reader.GetOrdinal("Task");
                int indexNotes = reader.GetOrdinal("Notes");
                int indexNumber = reader.GetOrdinal("Number");
                int indexAnswerId = reader.GetOrdinal("AnswerId");
                int indexAnswerText = reader.GetOrdinal("AnswerText");
                int indexIsCorrect = reader.GetOrdinal("IsCorrect");



                if (reader.HasRows)
                {

                    while (reader.Read())
                    {
                        if (test == null)
                        {
                            test = new Test()
                            {
                                Id = reader.GetGuid(indexId),
                                Theme = reader.GetString(indexTheme),
                                Author = reader.GetString(indexAuthor),
                                Version = reader.GetInt32(indexVersion),
                                Question = new List<Question>()

                            };
                        }
                        var questions = test.Question.Where(q => q.Id == reader.GetGuid(indexQuestionId)).ToList();
                        if (!questions.Any())
                        {
                            test.Question.Add(new Question()
                            {
                                Id = reader.GetGuid(indexQuestionId),
                                Task = reader.GetString(indexTask),
                                Notes = reader.GetString(indexNotes),
                                Number = reader.GetInt32(indexNumber),
                                Answers = new List<Answer>() {
                                    new Answer()
                                {
                                    Id = reader.GetGuid(indexAnswerId),
                                    AnswerText = reader.GetString(indexAnswerText),
                                    IsCorrect = reader.GetBoolean(indexIsCorrect)
                                } }
                            });

                        }
                        else
                        {
                            questions.First().Answers.Add(new Answer()
                            {
                                Id = reader.GetGuid(indexAnswerId),
                                AnswerText = reader.GetString(indexAnswerText),
                                IsCorrect = reader.GetBoolean(indexIsCorrect)
                            });
                        }

                    }

                }

            }
            return test;

        }

        public IList<Test> GetTests()
        {
            string _connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            List<Test> testsList = new List<Test>();
            using (SqlConnection connection = new SqlConnection())
            {
                connection.ConnectionString = _connectionString;



                var cmd = new SqlCommand("GetTests",
                                         connection);
                cmd.CommandType = CommandType.StoredProcedure;

                connection.Open();
                var reader = cmd.ExecuteReader();

                int indexId = reader.GetOrdinal("Id");
                int indexTheme = reader.GetOrdinal("Theme");
                int indexAuthor = reader.GetOrdinal("Author");
                int indexVersion = reader.GetOrdinal("Version");
                int indexQuestionId = reader.GetOrdinal("QuestionId");
                int indexTask = reader.GetOrdinal("Task");
                int indexNotes = reader.GetOrdinal("Notes");
                int indexNumber = reader.GetOrdinal("Number");
                int indexAnswerId = reader.GetOrdinal("AnswerId");
                int indexAnswerText = reader.GetOrdinal("AnswerText");
                int indexIsCorrect = reader.GetOrdinal("IsCorrect");




                if (reader.HasRows)
                {

                    while (reader.Read())
                    {
                        var tests = testsList.Where(q => q.Id == reader.GetGuid(indexId)).ToList();
                        if (!tests.Any())
                        {
                            testsList.Add(new Test()
                            {
                                Id = reader.GetGuid(indexId),
                                Theme = reader.GetString(indexTheme),
                                Author = reader.GetString(indexAuthor),
                                Version = reader.GetInt32(indexVersion),
                                Question = new List<Question>() { new Question()
                                {
                                    Id = reader.GetGuid(indexQuestionId),
                                    Task = reader.GetString(indexTask),
                                    Notes = reader.GetString(indexNotes),
                                    Number = reader.GetInt32(indexNumber),
                                    Answers = new List<Answer>()
                                {
                                    new Answer()
                                    {
                                        Id = reader.GetGuid(indexAnswerId),
                                        AnswerText = reader.GetString(indexAnswerText),
                                        IsCorrect = reader.GetBoolean(indexIsCorrect)
                                    }
                                }
                                }}

                            });
                        }
                        else
                        {
                            var questions = tests.First().Question.Where(q => q.Id == reader.GetGuid(indexQuestionId)).ToList();
                            if (!questions.Any())
                            {
                                tests.First().Question.Add(new Question()
                                {
                                    Id = reader.GetGuid(indexQuestionId),
                                    Task = reader.GetString(indexTask),
                                    Notes = reader.GetString(indexNotes),
                                    Number = reader.GetInt32(indexNumber),
                                    Answers = new List<Answer>()
                                {
                                    new Answer()
                                    {
                                        Id = reader.GetGuid(indexAnswerId),
                                        AnswerText = reader.GetString(indexAnswerText),
                                        IsCorrect = reader.GetBoolean(indexIsCorrect)
                                    }
                                }
                                });

                            }
                            else
                            {
                                questions.First().Answers.Add(new Answer()
                                {
                                    Id = reader.GetGuid(indexAnswerId),
                                    AnswerText = reader.GetString(indexAnswerText),
                                    IsCorrect = reader.GetBoolean(indexIsCorrect)
                                });
                            }
                        }




                    }

                }

            }
            return testsList;
        }

        public void SaveTest(Test test)
        {
            string _connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            var dbTest = GetTest(test.Id);
            if (dbTest == null)
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
                        command.CommandText = "AddTest";
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@id", test.Id);
                        command.Parameters.AddWithValue("@theme", test.Theme);
                        command.Parameters.AddWithValue("@author", test.Author);
                        command.Parameters.AddWithValue("@version", test.Version);
                        command.Parameters.AddWithValue("@deleted", false);
                        command.ExecuteNonQuery();

                        foreach (var question in test.Question)
                        {
                            command.CommandText = "AddQuestion";
                            command.CommandType = CommandType.StoredProcedure;
                            command.Parameters.Clear();
                            command.Parameters.AddWithValue("@id", question.Id);
                            command.Parameters.AddWithValue("@testId", test.Id);
                            command.Parameters.AddWithValue("@task", question.Task);
                            command.Parameters.AddWithValue("@notes", question.Notes);
                            command.Parameters.AddWithValue("@taskNumber", question.Number);
                            command.ExecuteNonQuery();

                            foreach (var answer in question.Answers)
                            {
                                command.CommandText = "AddAnswer";
                                command.CommandType = CommandType.StoredProcedure;
                                command.Parameters.Clear();
                                command.Parameters.AddWithValue("@id", answer.Id);
                                command.Parameters.AddWithValue("@questionId", question.Id);
                                command.Parameters.AddWithValue("@answerText", answer.AnswerText);
                                command.Parameters.AddWithValue("@isCorrect", answer.IsCorrect);
                                command.ExecuteNonQuery();
                            }
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
            else
            {
                var isModified = false;


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

                        foreach (var question in test.Question)
                        {
                            var dbQuestion = dbTest.Question.FirstOrDefault(q => q.Id == question.Id);
                            if (dbQuestion == null)
                            {
                                command.CommandText = "AddQuestion";
                                command.CommandType = CommandType.StoredProcedure;
                                command.Parameters.Clear();
                                command.Parameters.AddWithValue("@id", question.Id);
                                command.Parameters.AddWithValue("@testId", test.Id);
                                command.Parameters.AddWithValue("@task", question.Task);
                                command.Parameters.AddWithValue("@notes", question.Notes);
                                command.Parameters.AddWithValue("@taskNumber", question.Number);
                                command.ExecuteNonQuery();
                                isModified = true;
                            }
                            else if (!dbQuestion.Notes.Equals(question.Notes) || !dbQuestion.Task.Equals(question.Task))
                            {
                                command.CommandText = "UpdateQuestion";
                                command.CommandType = CommandType.StoredProcedure;
                                command.Parameters.Clear();
                                command.Parameters.AddWithValue("@id", question.Id);
                                command.Parameters.AddWithValue("@task", question.Task);
                                command.Parameters.AddWithValue("@notes", question.Notes);
                                command.ExecuteNonQuery();
                                isModified = true;
                            }



                            foreach (var answer in question.Answers)
                            {


                                if (dbQuestion == null ||
                                    dbQuestion.Answers.FirstOrDefault(q => q.Id == answer.Id) == null)
                                {
                                    command.CommandText = "AddAnswer";
                                    command.CommandType = CommandType.StoredProcedure;
                                    command.Parameters.Clear();
                                    command.Parameters.AddWithValue("@id", answer.Id);
                                    command.Parameters.AddWithValue("@questionId", question.Id);
                                    command.Parameters.AddWithValue("@answerText", answer.AnswerText);
                                    command.Parameters.AddWithValue("@isCorrect", answer.IsCorrect);
                                    command.ExecuteNonQuery();
                                    isModified = true;
                                }
                                else
                                {
                                    var dbAnswer = dbQuestion.Answers.FirstOrDefault(q => q.Id == answer.Id);
                                    if (!dbAnswer.AnswerText.Equals(answer.AnswerText) || !dbAnswer.IsCorrect.Equals(answer.IsCorrect))
                                    {
                                        command.CommandText = "UpdateAnswer";
                                        command.CommandType = CommandType.StoredProcedure;
                                        command.Parameters.Clear();
                                        command.Parameters.AddWithValue("@id", answer.Id);
                                        command.Parameters.AddWithValue("@answerText", answer.AnswerText);
                                        command.Parameters.AddWithValue("@isCorrect", answer.IsCorrect);
                                        command.ExecuteNonQuery();
                                        isModified = true;
                                    }
                                }



                            }
                        }

                        if (!dbTest.Theme.Equals(test.Theme) || isModified)
                        {
                            command.CommandText = "UpdateTest";
                            command.CommandType = CommandType.StoredProcedure;
                            command.Parameters.Clear();
                            command.Parameters.AddWithValue("@id", test.Id);
                            command.Parameters.AddWithValue("@theme", test.Theme);
                            command.Parameters.AddWithValue("@version", test.Version + 1);
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
