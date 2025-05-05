USE [TestAutoProjDb]
GO

/****** Object:  StoredProcedure [dbo].[sp_AppraisalWorkflow]    Script Date: 5/5/2025 1:44:12 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[sp_AppraisalWorkflow]
	@DBAction NVARCHAR(100) = null,
    @AppraisalId INT = null,
    @RoleId INT = null,
    @AssignDate DATETIME = null,
    @AssignBy INT = null,
    @AssignTo INT = null,
    @ActionId INT = null,
    @ActionDate DATETIME = null,
    @Title NVARCHAR(200) = null,
    @Message NVARCHAR(2000) = null,
    @Weight DECIMAL(18,2) = null,
    @Achievement DECIMAL(18,2) = null,
    @EmployeeScore DECIMAL(18,2) = null,
    @ManagerScore DECIMAL(18,2) = null,
    @EmployeeComment NVARCHAR(2000) = null,
    @ManagerComment NVARCHAR(2000) = null,
    @CurrentStatus NVARCHAR(50) = null,
	@OutParam int = null output
AS
BEGIN
	declare @ActionIdVal int, @AssignToVal int;

	--if RoleId is not correct
	if not exists(Select * from Employees where EmployeeId = @AssignBy and RoleId = @RoleId or @RoleId = null)
		Select @RoleId=RoleId from Employees where EmployeeId = @AssignBy;
	
	--RoleId = 1 -> Employee, RoleId = 2 -> Manager
	if(@RoleId = 1) Select @AssignToVal = ManagerId from Employees where EmployeeId = @AssignBy; 
	else if(@RoleId = 2) Select @AssignToVal = EmployeeId from Employees where ManagerId = @AssignBy;

	BEGIN --Phase 1 - Start
	if(@DBAction = 'Phase1EmpToMngAssign')
	begin
		--AppraisalId,RoleId,AssignDate,AssignBy,AssignTo,ActionId,ActionDate,Title,Message,Weight,Achievement
		--,EmployeeScore,ManagerScore,EmployeeComment,ManagerComment,CurrentStatus,IsLock,IsLatest

		--Common => Update or Insert => Title,Message,Weight,Achievement,EmployeeScore,ManagerScore,EmployeeComment,ManagerComment
		--if Reject,Approve,Close it has only update current task
		--Update => ActionId,ActionDate,AssignBy,AssignTo,EmployeeComment,ManagerComment,CurrentStatus,IsLatest

		--if Assign it has both update current task and inset new task
		--Update => ActionId,ActionDate,AssignBy,AssignTo,EmployeeComment,ManagerComment,IsLatest
		--Insert => RoleId,AssignDate,AssignBy,AssignTo,ActionId,Title,Message,Weight,Achievement,EmployeeScore,ManagerScore,EmployeeComment,ManagerComment,CurrentStatus,IsLock,IsLatest
		
		----1. Phase_1_Emp_New => Performance Appraisal has been initialized
		--Select @ActionIdVal=ActionId from TaskActions where ActionCode = 'Phase_1_Emp_New';
		--INSERT INTO Appraisals (RoleId, AssignDate, AssignBy, AssignTo, ActionId, ActionDate, Title, Message,Weight, EmployeeComment, CurrentStatus, IsLock, IsLatest) --, Achievement, EmployeeScore, ManagerScore, ,  ManagerComment, CurrentStatus
		--VALUES (@RoleId, GETDATE(), @AssignBy,@AssignToVal, @ActionIdVal, NULL, @Title, @Message, @Weight, @EmployeeComment, 'New', 1, 1) --, @Achievement, @EmployeeScore, @ManagerScore,	@ManagerComment, @CurrentStatus
		
		--2. Phase_1_Emp_ToMng_Assign => Employee submits Performance Objection to Line Manager with Title, Message, and Weight %
		Select @ActionIdVal=ActionId from TaskActions where ActionCode = 'Phase_1_Emp_ToMng_Assign'; --, AssignBy = @AssignBy, AssignTo = @AssignTo
		Update Appraisals Set 
		ActionId = @ActionIdVal, ActionDate = GetDate(), EmployeeComment = @EmployeeComment, IsLatest = 0
		Where AppraisalId = @AppraisalId;

		--3. Phase_1_Mng_FromEmp_New => Line Manager receives Performance Objection from Employee with Title, Message, and Weight %
		Select @ActionIdVal=ActionId from TaskActions where ActionCode = 'Phase_1_Mng_FromEmp_New';
		INSERT INTO Appraisals (RoleId, AssignDate, AssignBy, AssignTo, ActionId, Title, Message, Weight, EmployeeComment, CurrentStatus, IsLock, IsLatest) --, EmployeeComment, Achievement, EmployeeScore, ManagerScore, ,  ManagerComment, CurrentStatus
		VALUES (@RoleId, GETDATE(), @AssignBy,@AssignToVal, @ActionIdVal, @Title, @Message, @Weight, @EmployeeComment, 'In Process', 1, 1) --,  @EmployeeComment, @Achievement, @EmployeeScore, @ManagerScore,	@ManagerComment, @CurrentStatus

		SELECT @OutParam = CAST(scope_identity() AS int);
		
	end
	else if(@DBAction = 'Phase1MngToEmpReject')
	begin
		if exists(SELECT * FROM Appraisals WHERE AppraisalId = @AppraisalId)
		begin
			--Phase_1_Mng_ToEmp_Reject => Line Manager: Rejects Performance Objection and returns it to Employee with comments
			Select @ActionIdVal=ActionId from TaskActions where ActionCode = 'Phase_1_Mng_ToEmp_Reject'; --, AssignBy = @AssignBy, AssignTo = @AssignTo
			Update Appraisals Set 
			ActionId = @ActionIdVal, ActionDate = GetDate(), ManagerComment = @ManagerComment, IsLatest = 0 
			Where AppraisalId = @AppraisalId;

			--Phase_1_Emp_New => Performance Appraisal has been initialized
			Select @ActionIdVal=ActionId from TaskActions where ActionCode = 'Phase_1_Emp_New';
			INSERT INTO Appraisals (RoleId, AssignDate, AssignBy, AssignTo, ActionId, ManagerComment, CurrentStatus, IsLock, IsLatest) 
			VALUES (@RoleId, GETDATE(), @AssignBy,@AssignToVal, @ActionIdVal, @ManagerComment, 'Reject', 1, 1) 

			SELECT @OutParam = CAST(scope_identity() AS int);
		end
	end
	else if(@DBAction = 'Phase1MngToEmpApprove')
	begin
		if exists(SELECT * FROM Appraisals WHERE AppraisalId = @AppraisalId)
		begin
			--Phase_1_Mng_ToEmp_Approve => Line Manager: Approves Performance Objection and forwards it to Employee to begin Phase 2
			Select @ActionIdVal=ActionId from TaskActions where ActionCode = 'Phase_1_Mng_ToEmp_Approve';--, AssignBy = @AssignBy, AssignTo = @AssignTo
			Update Appraisals Set 
			ActionId = @ActionIdVal, ActionDate = GetDate(), ManagerComment = @ManagerComment, IsLatest = 0
			Where AppraisalId = @AppraisalId;

			--Phase_2_Emp_FromMng_New => Employee receives Phase 1 approval from Line Manager and proceeds to Phase 2
			Select @ActionIdVal=ActionId from TaskActions where ActionCode = 'Phase_2_Emp_FromMng_New';
			INSERT INTO Appraisals (RoleId, AssignDate, AssignBy, AssignTo, ActionId, ManagerComment, CurrentStatus, IsLock, IsLatest) 
			VALUES (@RoleId, GETDATE(), @AssignBy,@AssignToVal, @ActionIdVal, @ManagerComment, 'Approve', 1, 1) 

			SELECT @OutParam = CAST(scope_identity() AS int);
		end
	end
	END --Phase 1 - End

	BEGIN --Phase 2 - Start
	if(@DBAction = 'Phase2EmpToMngAssign')
	begin
		--Phase_2_Emp_ToMng_Assign => Employee submits Achievement % to Line Manager
		Select @ActionIdVal=ActionId from TaskActions where ActionCode = 'Phase_2_Emp_ToMng_Assign'; --, AssignBy = @AssignBy, AssignTo = @AssignTo
		Update Appraisals Set 
		ActionId = @ActionIdVal, ActionDate = GetDate(), EmployeeComment = @EmployeeComment, IsLatest = 0
		Where AppraisalId = @AppraisalId;

		--Phase_2_Mng_FromEmp_New => Line Manager receives Achievement % from Employee
		Select @ActionIdVal=ActionId from TaskActions where ActionCode = 'Phase_2_Mng_FromEmp_New';
		INSERT INTO Appraisals (RoleId, AssignDate, AssignBy, AssignTo, ActionId, Achievement, EmployeeComment, CurrentStatus, IsLock, IsLatest)
		VALUES (@RoleId, GETDATE(), @AssignBy, @AssignToVal, @ActionIdVal, @Achievement, @EmployeeComment, 'In Process', 1, 1) 

		SELECT @OutParam = CAST(scope_identity() AS int);
		
	end
	else if(@DBAction = 'Phase2MngToEmpReject')
	begin
		if exists(SELECT * FROM Appraisals WHERE AppraisalId = @AppraisalId)
		begin
			--Phase_1_Mng_ToEmp_Reject => Line Manager: Rejects Performance Objection and returns it to Employee with comments
			Select @ActionIdVal=ActionId from TaskActions where ActionCode = 'Phase_2_Mng_ToEmp_Reject'; --, AssignBy = @AssignBy, AssignTo = @AssignTo
			Update Appraisals Set 
			ActionId = @ActionIdVal, ActionDate = GetDate(), ManagerComment = @ManagerComment, IsLatest = 0 
			Where AppraisalId = @AppraisalId;

			--Phase_1_Emp_New => Performance Appraisal has been initialized
			Select @ActionIdVal=ActionId from TaskActions where ActionCode = 'Phase_2_Emp_FromMng_New';
			INSERT INTO Appraisals (RoleId, AssignDate, AssignBy, AssignTo, ActionId, ManagerComment, CurrentStatus, IsLock, IsLatest) 
			VALUES (@RoleId, GETDATE(), @AssignBy,@AssignToVal, @ActionIdVal, @ManagerComment, 'Reject', 1, 1) 

			SELECT @OutParam = CAST(scope_identity() AS int);
		end
	end
	else if(@DBAction = 'Phase2MngToEmpApprove')
	begin
		if exists(SELECT * FROM Appraisals WHERE AppraisalId = @AppraisalId)
		begin
			--Phase_1_Mng_ToEmp_Approve => Line Manager: Approves Performance Objection and forwards it to Employee to begin Phase 2
			Select @ActionIdVal=ActionId from TaskActions where ActionCode = 'Phase_2_Mng_ToEmp_Approve'; --, AssignBy = @AssignBy, AssignTo = @AssignTo
			Update Appraisals Set 
			ActionId = @ActionIdVal, ActionDate = GetDate(), ManagerComment = @ManagerComment, IsLatest = 0
			Where AppraisalId = @AppraisalId;

			--Phase_2_Emp_FromMng_New => Employee receives Phase 1 approval from Line Manager and proceeds to Phase 2
			Select @ActionIdVal=ActionId from TaskActions where ActionCode = 'Phase_3_Emp_FromMng_New';
			INSERT INTO Appraisals (RoleId, AssignDate, AssignBy, AssignTo, ActionId, ManagerComment, CurrentStatus, IsLock, IsLatest) 
			VALUES (@RoleId, GETDATE(), @AssignBy,@AssignToVal, @ActionIdVal, @ManagerComment, 'Approve', 1, 1) 

			SELECT @OutParam = CAST(scope_identity() AS int);
		end
	end
	END --Phase 2 - End

	BEGIN --Phase 3 - Start
	if(@DBAction = 'Phase3EmpToMngAssign')
	begin
		--Phase_2_Emp_ToMng_Assign => Employee submits Achievement % to Line Manager
		Select @ActionIdVal=ActionId from TaskActions where ActionCode = 'Phase_3_Emp_ToMng_Assign'; --, AssignBy = @AssignBy, AssignTo = @AssignTo
		Update Appraisals Set 
		ActionId = @ActionIdVal, ActionDate = GetDate(), EmployeeComment = @EmployeeComment, IsLatest = 0
		Where AppraisalId = @AppraisalId;

		--Phase_2_Mng_FromEmp_New => Line Manager receives Achievement % from Employee
		Select @ActionIdVal=ActionId from TaskActions where ActionCode = 'Phase_3_Mng_FromEmp_New';
		INSERT INTO Appraisals (RoleId, AssignDate, AssignBy, AssignTo, ActionId, EmployeeScore, EmployeeComment, CurrentStatus, IsLock, IsLatest)
		VALUES (@RoleId, GETDATE(), @AssignBy, @AssignToVal, @ActionIdVal, @EmployeeScore, @EmployeeComment, 'In Process', 1, 1) 

		SELECT @OutParam = CAST(scope_identity() AS int);
		
	end
	else if(@DBAction = 'Phase3MngToEmpReject')
	begin
		if exists(SELECT * FROM Appraisals WHERE AppraisalId = @AppraisalId)
		begin
			--Phase_1_Mng_ToEmp_Reject => Line Manager: Rejects Performance Objection and returns it to Employee with comments
			Select @ActionIdVal=ActionId from TaskActions where ActionCode = 'Phase_3_Mng_ToEmp_Reject'; --, AssignBy = @AssignBy, AssignTo = @AssignTo
			Update Appraisals Set 
			ActionId = @ActionIdVal, ActionDate = GetDate(), ManagerComment = @ManagerComment, IsLatest = 0 
			Where AppraisalId = @AppraisalId;

			--Phase_1_Emp_New => Performance Appraisal has been initialized
			Select @ActionIdVal=ActionId from TaskActions where ActionCode = 'Phase_3_Emp_FromMng_New';
			INSERT INTO Appraisals (RoleId, AssignDate, AssignBy, AssignTo, ActionId, ManagerScore, ManagerComment, CurrentStatus, IsLock, IsLatest) 
			VALUES (@RoleId, GETDATE(), @AssignBy,@AssignToVal, @ActionIdVal, @ManagerScore, @ManagerComment, 'Reject', 1, 1) 

			SELECT @OutParam = CAST(scope_identity() AS int);
		end
	end
	else if(@DBAction = 'Phase3MngToEmpApprove')
	begin
		if exists(SELECT * FROM Appraisals WHERE AppraisalId = @AppraisalId)
		begin
			--Phase_1_Mng_ToEmp_Approve => Line Manager: Approves Performance Objection and forwards it to Employee to begin Phase 2
			Select @ActionIdVal=ActionId from TaskActions where ActionCode = 'Phase_3_Mng_ToHR_Approve'; --, AssignBy = @AssignBy, AssignTo = @AssignTo
			Update Appraisals Set 
			ActionId = @ActionIdVal, ActionDate = GetDate(), ManagerComment = @ManagerComment, IsLatest = 0
			Where AppraisalId = @AppraisalId;

			--Phase_2_Emp_FromMng_New => Employee receives Phase 1 approval from Line Manager and proceeds to Phase 2
			Select @ActionIdVal=ActionId from TaskActions where ActionCode = 'Phase_4_HR_FromMng_New';
			INSERT INTO Appraisals (RoleId, AssignDate, AssignBy, AssignTo, ActionId, ManagerComment, CurrentStatus, IsLock, IsLatest) 
			VALUES (@RoleId, GETDATE(), @AssignBy,@AssignToVal, @ActionIdVal, @ManagerComment, 'Approve', 1, 1) 

			SELECT @OutParam = CAST(scope_identity() AS int);
		end
	end
	END --Phase 2 - End

	BEGIN --Phase 4 - Start
	if(@DBAction = 'Phase4EmpToMngAssign')
	begin
		--Phase_2_Emp_ToMng_Assign => Employee submits Achievement % to Line Manager
		Select @ActionIdVal=ActionId from TaskActions where ActionCode = 'Phase_4_HR_ToHRMng_Assign';
		Update Appraisals Set 
		ActionId = @ActionIdVal, ActionDate = GetDate(), EmployeeComment = @EmployeeComment, IsLatest = 0
		Where AppraisalId = @AppraisalId;

		--Phase_2_Mng_FromEmp_New => Line Manager receives Achievement % from Employee
		Select @ActionIdVal=ActionId from TaskActions where ActionCode = 'Phase_4_HRMng_FromHR_New';
		INSERT INTO Appraisals (RoleId, AssignDate, AssignBy, AssignTo, ActionId, EmployeeComment, CurrentStatus, IsLock, IsLatest)
		VALUES (@RoleId, GETDATE(), @AssignBy, @AssignToVal, @ActionIdVal, @EmployeeComment, 'In Process', 1, 1) 

		SELECT @OutParam = CAST(scope_identity() AS int);
		
	end
	else if(@DBAction = 'Phase4MngToEmpReject')
	begin
		if exists(SELECT * FROM Appraisals WHERE AppraisalId = @AppraisalId)
		begin
			--Phase_1_Mng_ToEmp_Reject => Line Manager: Rejects Performance Objection and returns it to Employee with comments
			Select @ActionIdVal=ActionId from TaskActions where ActionCode = 'Phase_4_HRMng_ToHR_Reject';
			Update Appraisals Set 
			ActionId = @ActionIdVal, ActionDate = GetDate(), ManagerComment = @ManagerComment, IsLatest = 0 
			Where AppraisalId = @AppraisalId;

			--Phase_1_Emp_New => Performance Appraisal has been initialized
			Select @ActionIdVal=ActionId from TaskActions where ActionCode = 'Phase_4_HR_FromMng_New';
			INSERT INTO Appraisals (RoleId, AssignDate, AssignBy, AssignTo, ActionId, ManagerComment, CurrentStatus, IsLock, IsLatest) 
			VALUES (@RoleId, GETDATE(), @AssignBy,@AssignToVal, @ActionIdVal, @ManagerComment, 'Reject', 1, 1) 

			SELECT @OutParam = CAST(scope_identity() AS int);
		end
	end
	else if(@DBAction = 'Phase4MngToEmpApprove')
	begin
		if exists(SELECT * FROM Appraisals WHERE AppraisalId = @AppraisalId)
		begin
			--Phase_1_Mng_ToEmp_Approve => Line Manager: Approves Performance Objection and forwards it to Employee to begin Phase 2
			Select @ActionIdVal=ActionId from TaskActions where ActionCode = 'Phase_4_HRMng_Close';
			Update Appraisals Set 
			ActionId = @ActionIdVal, ActionDate = GetDate(), ManagerComment = @ManagerComment, CurrentStatus = 'Close', IsLatest = 1
			Where AppraisalId = @AppraisalId;

			SELECT @OutParam = @AppraisalId;
		end
	end
	END --Phase 4 - End
   Select * from Appraisals where AppraisalId = @OutParam;
END


GO


