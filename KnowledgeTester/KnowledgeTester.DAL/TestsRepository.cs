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
    public class TestsRepository : Repository, ITestsRepository
    {

        private int IndexId { get; set; }
        int IndexTheme { get; set; }
        int IndexAuthor { get; set; }
        int IndexVersion { get; set; }
        int IndexQuestionId { get; set; }
        int IndexTask { get; set; }
        int IndexNotes { get; set; }
        int IndexNumber { get; set; }
        int IndexAnswerId { get; set; }
        int IndexAnswerText { get; set; }
        int IndexIsCorrect { get; set; }




        public void DeleteItem(Guid id)
        {
           
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
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
           
            Test test = null;
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                var cmd = new SqlCommand("GetTest",
                                         connection);
                cmd.Parameters.AddWithValue("@id", id);
                cmd.CommandType = CommandType.StoredProcedure;

                connection.Open();
                var reader = cmd.ExecuteReader();

                 SetReaderIndexes(reader);


                if (reader.HasRows)
                {

                    while (reader.Read())
                    {
                        test = ReadTest(reader, IndexId, IndexTheme, IndexAuthor, IndexVersion);

                        AddQuestionToTest(test, reader, IndexQuestionId, IndexTask, IndexNotes, IndexNumber, IndexAnswerId, IndexAnswerText, IndexIsCorrect);
                    }
                }

            }
            return test;

        }

        private void SetReaderIndexes(SqlDataReader reader)
        {
            IndexId = reader.GetOrdinal("Id");
            IndexTheme = reader.GetOrdinal("Theme");
            IndexAuthor = reader.GetOrdinal("Author");
            IndexVersion = reader.GetOrdinal("Version");
            IndexQuestionId = reader.GetOrdinal("QuestionId");
            IndexTask = reader.GetOrdinal("Task");
            IndexNotes = reader.GetOrdinal("Notes");
            IndexNumber = reader.GetOrdinal("Number");
            IndexAnswerId = reader.GetOrdinal("AnswerId");
            IndexAnswerText = reader.GetOrdinal("AnswerText");
            IndexIsCorrect = reader.GetOrdinal("IsCorrect");
        }

        private static Test ReadTest(SqlDataReader reader, int indexId, int indexTheme, int indexAuthor, int indexVersion)
        {
            return new Test()
            {
                Id = reader.GetGuid(indexId),
                Theme = reader.GetString(indexTheme),
                Author = reader.GetString(indexAuthor),
                Version = reader.GetInt32(indexVersion),
                Question = new List<Question>()

            };
        }

        private static void AddQuestionToTest(Test test, SqlDataReader reader, int indexQuestionId, int indexTask, int indexNotes,
            int indexNumber, int indexAnswerId, int indexAnswerText, int indexIsCorrect)
        {
            var questions = test.Question.Where(q => q.Id == reader.GetGuid(indexQuestionId)).ToList();
            if (!questions.Any())
            {
                test.Question.Add(ReadQuestion(reader, indexQuestionId, indexTask, indexNotes, indexNumber, indexAnswerId, indexAnswerText, indexIsCorrect));
            }
            else
            {
                questions.First().Answers.Add(ReadAnswer(reader, indexAnswerId, indexAnswerText, indexIsCorrect));
            }
        }

        private static Question ReadQuestion(SqlDataReader reader, int indexQuestionId, int indexTask, int indexNotes, int indexNumber, int indexAnswerId, int indexAnswerText, int indexIsCorrect)
        {
            return new Question()
            {
                Id = reader.GetGuid(indexQuestionId),
                Task = reader.GetString(indexTask),
                Notes = reader.GetString(indexNotes),
                Number = reader.GetInt32(indexNumber),
                Answers = new List<Answer>()
                {
                    ReadAnswer(reader, indexAnswerId, indexAnswerText, indexIsCorrect)
                }
            };
        }

        private static Answer ReadAnswer(SqlDataReader reader, int indexAnswerId, int indexAnswerText, int indexIsCorrect)
        {
            return new Answer()
            {
                Id = reader.GetGuid(indexAnswerId),
                AnswerText = reader.GetString(indexAnswerText),
                IsCorrect = reader.GetBoolean(indexIsCorrect)
            };
        }

        public IList<Test> GetTests()
        {
            
            List<Test> testsList = new List<Test>();
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                var cmdText = "GetTests";
                var reader = ExecuteReadCommand(cmdText, connection);

                SetReaderIndexes(reader);



                if (reader.HasRows)
                {

                    while (reader.Read())
                    {
                        var tests = testsList.Where(q => q.Id == reader.GetGuid(IndexId)).ToList();
                        if (!tests.Any())
                        {
                            Test test = ReadTest(reader, IndexId, IndexTheme, IndexAuthor, IndexVersion);
                            testsList.Add(test);
                            AddQuestionToTest(test, reader, IndexQuestionId, IndexTask, IndexNotes, IndexNumber, IndexAnswerId, IndexAnswerText, IndexIsCorrect);
                        }
                        else
                        {
                            AddQuestionToTest(tests.First(), reader, IndexQuestionId, IndexTask, IndexNotes, IndexNumber, IndexAnswerId, IndexAnswerText, IndexIsCorrect);
                           
                        }

                    }

                }

            }
            return testsList;
        }

        private static SqlDataReader ExecuteReadCommand(string cmdText, SqlConnection connection)
        {
            var cmd = new SqlCommand(cmdText,
                connection);
            cmd.CommandType = CommandType.StoredProcedure;

            connection.Open();
            var reader = cmd.ExecuteReader();
            return reader;
        }

        public void SaveTest(Test test)
        {
           
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
                        ExecuteAddTestCmd(test, command);

                        foreach (var question in test.Question)
                        {
                            ExecuteAddQuestionCmd(test, command, question);

                            foreach (var answer in question.Answers)
                            {
                                ExecuteAddAnswerCmd(command, answer, question);
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
                    SqlTransaction sqlTran = connection.BeginTransaction();
                    SqlCommand command = connection.CreateCommand();
                    command.Transaction = sqlTran;


                    try
                    {

                        foreach (var question in test.Question)
                        {
                            var dbQuestion = dbTest.Question.FirstOrDefault(q => q.Id == question.Id);
                            if (dbQuestion == null)
                            {
                                ExecuteAddQuestionCmd(test, command, question);
                                isModified = true;
                            }
                            else if (!dbQuestion.Notes.Equals(question.Notes) || !dbQuestion.Task.Equals(question.Task))
                            {
                                ExecuteUpdateQuestionCmd(command, question);
                                isModified = true;
                            }


                            foreach (var answer in question.Answers)
                            {

                                if (dbQuestion == null ||
                                    dbQuestion.Answers.FirstOrDefault(q => q.Id == answer.Id) == null)
                                {
                                    ExecuteAddAnswerCmd(command, answer, question);
                                    isModified = true;
                                }
                                else
                                {
                                    var dbAnswer = dbQuestion.Answers.FirstOrDefault(q => q.Id == answer.Id);
                                    if (!dbAnswer.AnswerText.Equals(answer.AnswerText) || !dbAnswer.IsCorrect.Equals(answer.IsCorrect))
                                    {
                                        ExecuteUpdateAnswerCmd(command, answer);
                                        isModified = true;
                                    }
                                }

                            }
                        }

                        if (!dbTest.Theme.Equals(test.Theme) || isModified)
                        {
                            ExecuteUpdateTestrCmd(test, command);
                        }
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

        private static void ExecuteUpdateTestrCmd(Test test, SqlCommand command)
        {
            command.CommandText = "UpdateTest";
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Clear();
            command.Parameters.AddWithValue("@id", test.Id);
            command.Parameters.AddWithValue("@theme", test.Theme);
            command.Parameters.AddWithValue("@version", test.Version + 1);
            command.ExecuteNonQuery();
        }

        private static void ExecuteUpdateAnswerCmd(SqlCommand command, Answer answer)
        {
            command.CommandText = "UpdateAnswer";
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Clear();
            command.Parameters.AddWithValue("@id", answer.Id);
            command.Parameters.AddWithValue("@answerText", answer.AnswerText);
            command.Parameters.AddWithValue("@isCorrect", answer.IsCorrect);
            command.ExecuteNonQuery();
        }

        private static void ExecuteUpdateQuestionCmd(SqlCommand command, Question question)
        {
            command.CommandText = "UpdateQuestion";
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Clear();
            command.Parameters.AddWithValue("@id", question.Id);
            command.Parameters.AddWithValue("@task", question.Task);
            command.Parameters.AddWithValue("@notes", question.Notes);
            command.ExecuteNonQuery();
        }

        private static void ExecuteAddAnswerCmd(SqlCommand command, Answer answer, Question question)
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

        private static void ExecuteAddQuestionCmd(Test test, SqlCommand command, Question question)
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
        }

        private static void ExecuteAddTestCmd(Test test, SqlCommand command)
        {
            command.CommandText = "AddTest";
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@id", test.Id);
            command.Parameters.AddWithValue("@theme", test.Theme);
            command.Parameters.AddWithValue("@author", test.Author);
            command.Parameters.AddWithValue("@version", test.Version);
            command.Parameters.AddWithValue("@deleted", false);
            command.ExecuteNonQuery();
        }
    }
}
