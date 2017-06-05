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

-- =============================================
-- Author:		NS
-- Create date: 04.06.2017
-- Description: returns data needed to create User instance
-- =============================================
CREATE PROCEDURE GetUser @id uniqueidentifier
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT u.*, ut.Id as UserTestId, ut.TestId, ut.Score, ut.Time,  ua.Id as UserAnswerId, ua.QuestionNumber, ua.AnswerId
	FROM [TesterDb].[dbo].[User] u
	LEFT JOIN [TesterDb].[dbo].[UserTest] ut on u.Id = ut.UserId
	LEFT JOIN [TesterDb].[dbo].[UserAnswer] ua on ut.Id = ua.UserTestId
	WHERE TestId = @id
	 
END
GO


-- =============================================
-- Author:		NS
-- Create date: 04.06.2017
-- Description: returns data needed to create User instance
-- =============================================
CREATE PROCEDURE GetUserByLoginPassword
 @login nvarchar(50),
 @password nvarchar(250)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT u.*, ut.Id as UserTestId, ut.TestId, ut.Score, ut.Time,  ua.Id as UserAnswerId, ua.QuestionNumber, ua.AnswerId
	FROM [TesterDb].[dbo].[User] u
	LEFT JOIN [TesterDb].[dbo].[UserTest] ut on u.Id = ut.UserId
	LEFT JOIN [TesterDb].[dbo].[UserAnswer] ua on ut.Id = ua.UserTestId
	WHERE Login = @login AND Password = @password
	 
END
GO



-- =============================================
-- Author:		NS
-- Create date: 04.06.2017
-- Description: insert user
-- =============================================
CREATE PROCEDURE AddUser
@id uniqueidentifier,
@name nvarchar(50),
@lastName nvarchar(50),
@login nvarchar(50),
@password nvarchar(250)

AS
BEGIN

	INSERT INTO [dbo].[User]
           ([Id]
           ,[Name]
           ,[LastName]
           ,[Login]
           ,[Password])
     VALUES
           (@id
           ,@name
           ,@lastName
           ,@login
           ,@password)
	 
END
GO

-- =============================================
-- Author:		NS
-- Create date: 05.06.2017
-- Description: insert user test
-- =============================================
CREATE PROCEDURE AddUserTest
@id uniqueidentifier,
@testId uniqueidentifier,
@score decimal(18,0),
@time datetime,
@userId uniqueidentifier

AS
BEGIN

	INSERT INTO [dbo].[UserTest]
           ([Id]
           ,[TestId]
           ,[Score]
           ,[Time]
           ,[UserId])
     VALUES
           (@id
           ,@testId
           ,@score
           ,@time
           ,@userId)
	 
END
GO


-- =============================================
-- Author:		NS
-- Create date: 05.06.2017
-- Description: insert user answer
-- =============================================
CREATE PROCEDURE AddUserAnswer
@id uniqueidentifier,
@questionNumber int,
@userTestId uniqueidentifier,
@answerId uniqueidentifier


AS
BEGIN

	INSERT INTO [dbo].[UserAnswer]
           ([Id]
           ,[QuestionNumber]
           ,[UserTestId]
           ,[AnswerId])
     VALUES
           (@id
           ,@questionNumber
           ,@userTestId
           ,@answerId)
	 
END
GO
