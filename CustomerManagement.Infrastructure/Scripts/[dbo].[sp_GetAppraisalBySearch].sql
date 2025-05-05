USE [TestAutoProjDb]
GO

/****** Object:  StoredProcedure [dbo].[sp_GetAppraisalBySearch]    Script Date: 5/5/2025 1:45:53 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[sp_GetAppraisalBySearch]
	@DBAction NVARCHAR(100) = '',
    @MenuAction NVARCHAR(50) = '',
    @AppraisalId INT = '',
    @LoginId INT = '',
    @RoleId INT = '',
    @SortColumn NVARCHAR(50) = '',
    @SortDir NVARCHAR(4) = '',
    @SearchVal NVARCHAR(100) = '',
    @Skip INT = 0,
    @PageSize INT = 10,
    @UrlDetail NVARCHAR(200) = '',
    @UrlDelete NVARCHAR(200) = '',
	@OutParam int = null output
AS
BEGIN
    SET NOCOUNT ON;

	declare @ActionIdVal int, @RoleIdVal int, @EmpData int;
	declare @TitleVal NVARCHAR(200) = null, @MessageVal NVARCHAR(2000) = null, @WeightVal DECIMAL(18,2) = null,
    @AchievementVal DECIMAL(18,2) = null, @EmployeeScoreVal DECIMAL(18,2) = null, @ManagerScoreVal DECIMAL(18,2) = null, 
	@EmployeeCommentVal NVARCHAR(2000) = null, @ManagerCommentVal NVARCHAR(2000) = null;

	if not exists(Select * from Employees where EmployeeId = @LoginId and RoleId = @RoleId)
		Select @RoleId=RoleId,@RoleIdVal=RoleId from Employees where EmployeeId = @LoginId;
	else
		Select @RoleIdVal=RoleId from Employees where EmployeeId = @LoginId;

	IF (@DBAction = 'GetAppraisalByUId')
	begin
		SELECT TOP 1 @TitleVal=Title, @MessageVal=Message,@WeightVal=Weight FROM Appraisals  WHERE Title IS NOT NULL AND (AssignBy = @LoginId or AssignTo = @LoginId) ORDER BY AppraisalId DESC
		SELECT TOP 1 @AchievementVal=Achievement FROM Appraisals  WHERE Achievement IS NOT NULL	AND (AssignBy = @LoginId or AssignTo = @LoginId) ORDER BY AppraisalId DESC
		SELECT TOP 1 @EmployeeScoreVal=EmployeeScore FROM Appraisals  WHERE EmployeeScore IS NOT NULL AND (AssignBy = @LoginId or AssignTo = @LoginId) ORDER BY AppraisalId DESC
		SELECT TOP 1 @EmployeeCommentVal=EmployeeComment FROM Appraisals  WHERE EmployeeComment IS NOT NULL AND (AssignBy = @LoginId or AssignTo = @LoginId) ORDER BY AppraisalId DESC

		SELECT TOP 1 @ManagerScoreVal=ManagerScore FROM Appraisals  WHERE ManagerScore IS NOT NULL AND (AssignBy = @LoginId or AssignTo = @LoginId) ORDER BY AppraisalId DESC
		SELECT TOP 1 @ManagerCommentVal=ManagerComment FROM Appraisals  WHERE ManagerComment IS NOT NULL AND (AssignBy = @LoginId or AssignTo = @LoginId) ORDER BY AppraisalId DESC
		
		if exists(Select AppraisalId from Appraisals WHERE IsLatest=1 and (AssignBy = @LoginId or AssignTo = @LoginId))
		begin
			SELECT distinct AppraisalId,a.RoleId,AssignDate,AssignBy,AssignTo,a.ActionId,ActionDate
			--,Title,Message,Weight,Achievement,EmployeeScore,ManagerScore,EmployeeComment,ManagerComment
			,@TitleVal as Title,@MessageVal as Message,@WeightVal as Weight,@AchievementVal as Achievement,@EmployeeScoreVal as EmployeeScore,@ManagerScoreVal as ManagerScore
			,@EmployeeCommentVal as EmployeeComment,@ManagerCommentVal as ManagerComment
			,CurrentStatus,IsLock,IsLatest
			,ActionType,ActionCode,ActionName,ActionStatus,FormType,null as DBAction,null as LoginId, null as RoleName, null as AssignByName, null as AssignToName
			FROM Appraisals a inner join TaskActions t on a.ActionId = t.ActionId WHERE IsLatest=1 and (AssignBy = @LoginId or AssignTo = @LoginId);
		end
		else
		begin
			Select @EmpData=count(*) from Appraisals WHERE RoleId=1 and (AssignBy = @LoginId or AssignTo = @LoginId)
			if(@RoleIdVal = 1 and @EmpData = 0) --RoleId = 1 -> Employee, RoleId = 2 -> Manager
			begin
				--if donot have any data for an employee initialized it
				if exists(Select * from Employees where EmployeeId = @LoginId)
				begin
					--Phase_1_Emp_New => Performance Appraisal has been initialized
					
					Select @ActionIdVal=ActionId from TaskActions where ActionCode = 'Phase_1_Emp_New';

					INSERT INTO Appraisals (RoleId, AssignDate, AssignBy, AssignTo, ActionId, ActionDate, CurrentStatus, IsLock, IsLatest) 
					VALUES (@RoleIdVal, GETDATE(), @LoginId,NULL, @ActionIdVal, NULL, 'New', 1, 1)
					SELECT @OutParam = CAST(scope_identity() AS int);
				end

				SELECT distinct AppraisalId,a.RoleId,AssignDate,AssignBy,AssignTo,a.ActionId,ActionDate
				--,Title,Message,Weight,Achievement,EmployeeScore,ManagerScore,EmployeeComment,ManagerComment
				,@TitleVal as Title,@MessageVal as Message,@WeightVal as Weight,@AchievementVal as Achievement,@EmployeeScoreVal as EmployeeScore,@ManagerScoreVal as ManagerScore
				,@EmployeeCommentVal as EmployeeComment,@ManagerCommentVal as ManagerComment
				,CurrentStatus ,IsLock,IsLatest
				,ActionType,ActionCode,ActionName,ActionStatus,FormType,null as DBAction,null as LoginId, null as RoleName, null as AssignByName, null as AssignToName
				FROM Appraisals a inner join TaskActions t on a.ActionId = t.ActionId WHERE IsLatest=1 and (AssignBy = @LoginId or AssignTo = @LoginId);
			end
		end
	END
	ELSE IF (@DBAction = 'GetAppraisalDetailById') --sp_help Appraisals
	begin
	
		SELECT TOP 1 @TitleVal=Title, @MessageVal=Message,@WeightVal=Weight FROM Appraisals  WHERE Title IS NOT NULL AND (AssignBy IN ((Select AssignBy FROM Appraisals where AppraisalId = @AppraisalId),(Select AssignTo FROM Appraisals where AppraisalId = @AppraisalId)) OR AssignTo IN ((Select AssignBy FROM Appraisals where AppraisalId = @AppraisalId),(Select AssignTo FROM Appraisals where AppraisalId = @AppraisalId))) ORDER BY AppraisalId DESC
		SELECT TOP 1 @AchievementVal=Achievement FROM Appraisals  WHERE Achievement IS NOT NULL	AND (AssignBy IN ((Select AssignBy FROM Appraisals where AppraisalId = @AppraisalId),(Select AssignTo FROM Appraisals where AppraisalId = @AppraisalId)) OR AssignTo IN ((Select AssignBy FROM Appraisals where AppraisalId = @AppraisalId),(Select AssignTo FROM Appraisals where AppraisalId = @AppraisalId))) ORDER BY AppraisalId DESC
		SELECT TOP 1 @EmployeeScoreVal=EmployeeScore FROM Appraisals  WHERE EmployeeScore IS NOT NULL AND (AssignBy IN ((Select AssignBy FROM Appraisals where AppraisalId = @AppraisalId),(Select AssignTo FROM Appraisals where AppraisalId = @AppraisalId)) OR AssignTo IN ((Select AssignBy FROM Appraisals where AppraisalId = @AppraisalId),(Select AssignTo FROM Appraisals where AppraisalId = @AppraisalId))) ORDER BY AppraisalId DESC
		SELECT TOP 1 @EmployeeCommentVal=EmployeeComment FROM Appraisals  WHERE EmployeeComment IS NOT NULL AND (AssignBy IN ((Select AssignBy FROM Appraisals where AppraisalId = @AppraisalId),(Select AssignTo FROM Appraisals where AppraisalId = @AppraisalId)) OR AssignTo IN ((Select AssignBy FROM Appraisals where AppraisalId = @AppraisalId),(Select AssignTo FROM Appraisals where AppraisalId = @AppraisalId))) ORDER BY AppraisalId DESC

		SELECT TOP 1 @ManagerScoreVal=ManagerScore FROM Appraisals  WHERE ManagerScore IS NOT NULL AND (AssignBy IN ((Select AssignBy FROM Appraisals where AppraisalId = @AppraisalId),(Select AssignTo FROM Appraisals where AppraisalId = @AppraisalId)) OR AssignTo IN ((Select AssignBy FROM Appraisals where AppraisalId = @AppraisalId),(Select AssignTo FROM Appraisals where AppraisalId = @AppraisalId))) ORDER BY AppraisalId DESC
		SELECT TOP 1 @ManagerCommentVal=ManagerComment FROM Appraisals  WHERE ManagerComment IS NOT NULL AND (AssignBy IN ((Select AssignBy FROM Appraisals where AppraisalId = @AppraisalId),(Select AssignTo FROM Appraisals where AppraisalId = @AppraisalId)) OR AssignTo IN ((Select AssignBy FROM Appraisals where AppraisalId = @AppraisalId),(Select AssignTo FROM Appraisals where AppraisalId = @AppraisalId))) ORDER BY AppraisalId DESC
		
		
		--SELECT AppraisalId,a.RoleId,AssignDate,AssignBy,AssignTo,a.ActionId,ActionDate,Title,Message,Weight
		--	,Achievement,EmployeeScore,ManagerScore,EmployeeComment,ManagerComment,CurrentStatus,IsLock,IsLatest,0 as LoginId
		--	,ActionType,ActionCode,ActionName,ActionStatus,FormType,'' as DBAction
		--	FROM Appraisals a inner join TaskActions t on a.ActionId = t.ActionId WHERE IsLatest=1 and AppraisalId = @AppraisalId;
		Select * from Appraisals WHERE IsLatest=1 and AppraisalId = @AppraisalId;
		SELECT AppraisalId,a.RoleId,AssignDate,AssignBy,AssignTo,a.ActionId,ActionDate
		,(Select top 1 EmployeeName from Employees where EmployeeId=AssignBy) as AssignByName,(Select top 1 EmployeeName from Employees where EmployeeId=AssignTo) as AssignToName
		,@TitleVal as Title,@MessageVal as Message,@WeightVal as Weight,@AchievementVal as Achievement,@EmployeeScoreVal as EmployeeScore,@ManagerScoreVal as ManagerScore
		,@EmployeeCommentVal as EmployeeComment,@ManagerCommentVal as ManagerComment,CurrentStatus,IsLock,IsLatest,0 as LoginId
			,ActionType,ActionCode,ActionName,ActionStatus,FormType,'' as DBAction
			FROM Appraisals a inner join TaskActions t on a.ActionId = t.ActionId WHERE IsLatest=1 and AppraisalId = @AppraisalId;
		SELECT distinct * FROM Appraisals a inner join TaskActions t on a.ActionId = t.ActionId 
		WHERE AssignBy IN (Select AssignBy FROM Appraisals where AppraisalId = @AppraisalId) or AssignTo IN (Select AssignTo FROM Appraisals where AppraisalId = @AppraisalId);
	END
   
END;

GO


