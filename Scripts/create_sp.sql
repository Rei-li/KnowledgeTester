USE [TesterDb]
GO


SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON

GO
-- =============================================
-- Author:		NS
-- Create date: 04.06.2017
-- Description: returns data needed to create Test instance
-- =============================================
CREATE PROCEDURE GetTest @id uniqueidentifier
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT t.*, q.Id as QuestionId, q.Task, q.Notes, q.Number, a.Id as AnswerId, a.AnswerText, a.IsCorrect
	FROM [TesterDb].[dbo].[Test] t 
	JOIN [TesterDb].[dbo].[Question] q on t.Id = q.TestId
	JOIN [TesterDb].[dbo].[Answer] a on q.Id = a.QuestionId
	WHERE TestId = @id
	 
END
GO


-- =============================================
-- Author:		NS
-- Create date: 04.06.2017
-- Description: returns data needed to create list of all Test instances
-- =============================================
CREATE PROCEDURE GetTests 
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT t.*, q.Id as QuestionId, q.Task, q.Notes, q.Number, a.Id as AnswerId, a.AnswerText, a.IsCorrect
	FROM [TesterDb].[dbo].[Test] t 
	JOIN [TesterDb].[dbo].[Question] q on t.Id = q.TestId
	JOIN [TesterDb].[dbo].[Answer] a on q.Id = a.QuestionId
	WHERE t.Deleted = 0
	 
END
GO


-- =============================================
-- Author:		NS
-- Create date: 04.06.2017
-- Description: insert answer
-- =============================================
CREATE PROCEDURE AddAnswer
@id uniqueidentifier,
@questionId uniqueidentifier,
@answerText nvarchar(500),
@isCorrect bit

AS
BEGIN

	INSERT INTO [dbo].[Answer]
           ([Id]
           ,[QuestionId]
           ,[AnswerText]
           ,[IsCorrect])
     VALUES
           (@id
           ,@questionId
           ,@answerText
           ,@isCorrect)
	 
END
GO


-- =============================================
-- Author:		NS
-- Create date: 04.06.2017
-- Description: insert question
-- =============================================
CREATE PROCEDURE AddQuestion
@id uniqueidentifier,
@testId uniqueidentifier,
@task nvarchar(500),
@notes nvarchar(500),
@taskNumber int

AS
BEGIN

	INSERT INTO [dbo].[Question]
           ([Id]
           ,[TestId]
           ,[Task]
           ,[Notes]
           ,[Number])
     VALUES
           (@id
           ,@testId
           ,@task
           ,@notes
           ,@taskNumber)
	 
END
GO


-- =============================================
-- Author:		NS
-- Create date: 04.06.2017
-- Description: insert test
-- =============================================
CREATE PROCEDURE AddTest
@id uniqueidentifier,
@theme nvarchar(500),
@author nvarchar(500),
@version int,
@deleted bit

AS
BEGIN

	INSERT INTO [dbo].[Test]
           ([Id]
           ,[Theme]
           ,[Author]
           ,[Version]
		   ,[Deleted])
     VALUES
           (@id
           ,@theme
           ,@author
           ,@version
		   ,@deleted)
	 
END
GO

-- =============================================
-- Author:		NS
-- Create date: 04.06.2017
-- Description: delete test logicaly
-- =============================================
CREATE PROCEDURE DeleteTest
@id uniqueidentifier

AS
BEGIN

	UPDATE [dbo].[Test]
    SET [Deleted] = 1
    WHERE [Id] = @id
	 
END
GO


-- =============================================
-- Author:		NS
-- Create date: 04.06.2017
-- Description: update test 
-- =============================================
CREATE PROCEDURE UpdateTest
@id uniqueidentifier,
@theme nvarchar(500),
@version int

AS
BEGIN

	UPDATE [dbo].[Test]
   SET [Theme] = @theme
      ,[Version] = @version
    WHERE [Id] = @id
	 
END
GO


-- =============================================
-- Author:		NS
-- Create date: 04.06.2017
-- Description: update Question 
-- =============================================
CREATE PROCEDURE UpdateQuestion
@id uniqueidentifier,
@task nvarchar(500),
@notes nvarchar(500)

AS
BEGIN

	UPDATE [dbo].[Question]
   SET [Task] = @task
      ,[Notes] = @notes
    WHERE [Id] = @id
	 
END
GO

-- =============================================
-- Author:		NS
-- Create date: 04.06.2017
-- Description: update Answer 
-- =============================================
CREATE PROCEDURE UpdateAnswer
@id uniqueidentifier,
@answerText nvarchar(500),
@isCorrect bit

AS
BEGIN

	UPDATE [dbo].[Answer]
    SET 
	   [AnswerText] = @answerText
      ,[IsCorrect] = @isCorrect
    WHERE [Id] = @id
	 
END
GO


