USE [IIL_Adverse]
GO
/****** Object:  StoredProcedure [dbo].[USP_GetCustomerInfo]    Script Date: 25/04/2022 7:19:31 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- USP_GetCustomerInfo 'TWAA0003'
alter procedure [dbo].[USP_GetCustomerInfo](
@custId varchar(40)
)
as
begin
	select 
	rtrim(ltrim(CustomerCode)) as CustomerCode, LanguageDesc as language, a.Name, b.Description as gender, c.occupation as occupation, DOB, Age, Phone, Email, StreetAddress as stAddress, DoctorRegNo, City, State, PostalCode, e.Name + '@' + e.Code  as country
	from tbl_Customer_Registration_T  a
	join tbl_Adv_Gender_M b on a.GenderCode = b.id
	join tbl_Adv_Occupation_M c on a.Occupation = c.id 
	join tbl_Langauge_M d on a.Language = d.id
	join tbl_Adv_CountryCode_M e on a.CountryCode = e.Code
	where a.IsActive = 1 and b.IsActive = 1 and c.IsActive = 1 and d.IsActive = 1  and CustomerCode = @custId
end

select * from [dbo].[tbl_Adv_AdverseEvent_T]
select * from [dbo].[tbl_Adverse_submit_T]
select * from tbl_Adv_MedicalHistory_T

CustId, Password, CreatedBy, Status 

exec USP_TableColumnsList tbl_Adv_PasswordLog_T
@CustId, @Password, @Broswer, @BroswerVersion, @Latitude, @Longitude, @LastLogin, @CreatedDate, @ClientIP
CustId, Password, Broswer, BroswerVersion, Latitude, Longitude, LastLogin, CreatedDate, ClientIP
 USP_ConvertTableToParameter tbl_Adv_MedicalHistory_T
 -- USP_TableColumnPrefixParamater 'tbl_Adv_MedicalHistory_T','History_'

alter Procedure USP_SubmitAdverseEvent
(
@Patient_Name varchar(200),
@Patient_Email varchar(200),
@Patient_Phone numeric,
@Patient_StAddress varchar(200),
@Patient_City varchar(100),
@Patient_State varchar(100),
@Patient_PostalCode varchar(20),
@Patient_CountryCode char,
@Patient_Gender char,
@Patient_Age int,
@Patient_Dob date,

@Vaccine_DateTimeVaccination datetime,
@Vaccine_VarccineName varchar(100),
@Vaccine_Manufacturer varchar(100),
@Vaccine_BatchNo varchar(100),
@Vaccine_ManufacturingDate datetime,
@Vaccine_ExpiryDate datetime,

@Vaccination_Route varchar(200),
@Vaccination_SiteOfAdministration char,
@Vaccination_DoseNumber char,

@Facility_Facility varchar(100),
@Facility_Email varchar(100),
@Facility_StreetAddress varchar(200),
@Facility_City varchar(100),
@Facility_Country char,
@Facility_Phone numeric,
@Facility_State varchar(100),

@AdverseEvent_AdverseEvent varchar(200),
@AdverseEvent_StartDate date,
@AdverseEvent_StopDate date,
@AdverseEvent_AdverseEventContinuing bit,
@AdverseEvent_Clinic varchar(100),
@AdverseEvent_DoctorName varchar(100),
@AdverseEvent_City varchar(100),
@AdverseEvent_DoctorConsultationTaken bit,
@AdverseEvent_State varchar(100),
@AdverseEvent_CountryCode char,
@AdverseEvent_StreetAddress varchar(200),
@AdverseEvent_Email varchar(100),
@AdverseEvent_Phone numeric,
@AdverseEvent_ResultOfMedicalLabTest varchar(500),
@AdverseEvent_TreatmentGiven varchar(500),

@History_PatientPregnant char,
@History_Allergies varchar(500),
@History_Medications varchar(500),
@History_Illnesses varchar(500),
@History_Chronic varchar(500)
)
as
begin

 BEGIN TRY
        BEGIN TRANSACTION;
        
		-- Patient Info
		insert into tbl_Adv_Patient_T(Patient_Name, Patient_Email, Patient_Phone, Patient_StAddress, Patient_City, Patient_State, Patient_PostalCode, Patient_CountryCode, Patient_Gender, Patient_Age, Patient_Dob)
		select @Patient_Name, @Patient_Email, @Patient_Phone, @Patient_StAddress, @Patient_City, @Patient_State, @Patient_PostalCode, @Patient_CountryCode, @Patient_Gender, @Patient_Age, @Patient_Dob

		-- Varccine Info
		insert into tbl_Adv_Vaccine_T(DateTimeVaccination, VarccineName, Manufacturer, BatchNo, ManufacturingDate, ExpiryDate)
		select @Vaccine_DateTimeVaccination, @Vaccine_VarccineName, @Vaccine_Manufacturer, @Vaccine_BatchNo, @Vaccine_ManufacturingDate, @Vaccine_ExpiryDate

		-- Vaccination Info 
		insert into tbl_Adv_Vaccination_T(Route, SiteOfAdministration, DoseNumber)
		select @Vaccination_Route, @Vaccination_SiteOfAdministration, @Vaccination_DoseNumber

		-- Facility
		insert into tbl_Adv_Facility_T(Facility, Email, StreetAddress, City, Country, Phone, State)
		select @Facility_Facility, @Facility_Email, @Facility_StreetAddress, @Facility_City, @Facility_Country, @Facility_Phone, @Facility_State

		-- Adverse Event
		insert into tbl_Adv_AdverseEvent_T(AdverseEvent, StartDate, StopDate, AdverseEventContinuing, Clinic, DoctorName, City, DoctorConsultationTaken, State, CountryCode, StreetAddress, Email, Phone, ResultOfMedicalLabTest, TreatmentGiven)
		select @AdverseEvent_AdverseEvent, @AdverseEvent_StartDate, @AdverseEvent_StopDate, @AdverseEvent_AdverseEventContinuing, @AdverseEvent_Clinic, @AdverseEvent_DoctorName, @AdverseEvent_City, @AdverseEvent_DoctorConsultationTaken, @AdverseEvent_State, @AdverseEvent_CountryCode, @AdverseEvent_StreetAddress, @AdverseEvent_Email, @AdverseEvent_Phone, @AdverseEvent_ResultOfMedicalLabTest, @AdverseEvent_TreatmentGiven

		-- Medical History
		insert into tbl_Adv_MedicalHistory_T(PatientPregnant, Allergies, Medications, Illnesses, Chronic)
		select @History_PatientPregnant, @History_Allergies, @History_Medications, @History_Illnesses, @History_Chronic

		select 1

        COMMIT TRANSACTION;  
    END TRY
    BEGIN CATCH
        -- report exception
        EXEC usp_report_error;
        
        -- Test if the transaction is uncommittable.  
        IF (XACT_STATE()) = -1  
        BEGIN  
            PRINT  N'The transaction is in an uncommittable state.' +  
                    'Rolling back transaction.'  
			select 0
            ROLLBACK TRANSACTION;  
        END;  
        
        -- Test if the transaction is committable.  
        IF (XACT_STATE()) = 1  
        BEGIN  
            PRINT N'The transaction is committable.' +  
                'Committing transaction.'  
			select 1
            COMMIT TRANSACTION;     
        END;  
    END CATCH
end

usp_report_error

CREATE PROC usp_report_error
AS
	INSERT INTO TBL_ADV_ERRORREPORT_T(ERRORNUMBER, ERRORSEVERITY, ERRORSTATE, ERRORLINE, ERRORPROCEDURE, ERRORMESSAGE)
    SELECT   
        ERROR_NUMBER() AS ErrorNumber  
        ,ERROR_SEVERITY() AS ErrorSeverity  
        ,ERROR_STATE() AS ErrorState  
        ,ERROR_LINE () AS ErrorLine  
        ,ERROR_PROCEDURE() AS ErrorProcedure  
        ,ERROR_MESSAGE() AS ErrorMessage;  
GO

alter Procedure USP_TableColumnPrefixParamater
(
@TableName varchar(100),
@Prefix varchar(100) = null
)
as
begin
		SELECT distinct
		stuff((
        select coalesce('@' + case when @Prefix is null then '' else @Prefix end + cast(c.name as varchar(100)) + ', ', 'null')
        from sys.columns c
		INNER JOIN 
			sys.types t ON c.user_type_id = t.user_type_id
		LEFT OUTER JOIN 
			sys.index_columns ic ON ic.object_id = c.object_id AND ic.column_id = c.column_id
		LEFT OUTER JOIN 
			sys.indexes i ON ic.object_id = i.object_id AND ic.index_id = i.index_id
		WHERE
			c.object_id = OBJECT_ID(''+ @TableName +'')
        for xml path('')
    )
    , 1, 1, ''
) as [File_viewer]
		FROM    
			sys.columns c
		INNER JOIN 
			sys.types t ON c.user_type_id = t.user_type_id
		LEFT OUTER JOIN 
			sys.index_columns ic ON ic.object_id = c.object_id AND ic.column_id = c.column_id
		LEFT OUTER JOIN 
			sys.indexes i ON ic.object_id = i.object_id AND ic.index_id = i.index_id
		WHERE
			c.object_id = OBJECT_ID(''+ @TableName +'')
end

-- USP_ValidateUser 'INAA0001','R8DSCr'
alter procedure USP_ValidateUser 
(
@Username varchar(10),
@Password varchar(30)
)
as
begin
	
	if exists(select COUNT(id) from [dbo].[tbl_Adv_CustLog_T] where CustId = @Username and Password = @Password)
	begin		
		select CustId as UserCode, Name as UserName, Email, LastLogin from [tbl_Adv_CustLog_T] a join [dbo].[tbl_Customer_Registration_T] b
		on a.CustId = b.CustomerCode where a.CustId = @Username
		update [tbl_Adv_CustLog_T] set LastLogin= GETDATE() where CustId = @Username
	end
	else
	begin
		select '' UserCode, '' UserName, '' Email, '' LastLogin
	end
end

select * from tbl_Customer_Registration_T where CustomerCode

select dbo.customerNumber(SELECT (count(*) + 1) FROM tbl_Customer_Registration_T Nolock)
SELECT (count(*) + 1) FROM tbl_Customer_Registration_T Nolock
select dbo.customerNumber(120)
select dbo.customerNumber(121)
select * from tbl_Adv_CustLog_T
select * from tbl_Adv_PasswordLog_T 
select newid()


create procedure USP_ResetPassword
(
	@CustId				varchar(20),
	@Password			varchar(1000),
	@ResetPasswordCode	varchar(500)
)
as
begin
	if exists(select CustId from tbl_Adv_CustLog_T nolock where CustId = @CustId and ResetPasswordCode = @ResetPasswordCode)
	begin
		update tbl_Adv_CustLog_T set Password = @Password where CustId = @CustId 

		if exists(select custid from tbl_Adv_PasswordLog_T where CustId = @CustId)
		begin
			update tbl_Adv_PasswordLog_T set Status = 0 where CustId = @CustId
		end
	
		insert into tbl_Adv_PasswordLog_T(CustId, Password, CreatedBy, Status)
		select @CustId,@Password,@CustId,1 

		select 'valid'
	end
	else 
	begin
		select 'invalid'
	end
end


create procedure USP_ChangePassword
(
	@CustId				varchar(20),
	@Password			varchar(1000)
)
as
begin
	if exists(select CustId from tbl_Adv_CustLog_T nolock where CustId = @CustId)
	begin
		update tbl_Adv_CustLog_T set Password = @Password where CustId = @CustId 

		if exists(select custid from tbl_Adv_PasswordLog_T where CustId = @CustId)
		begin
			update tbl_Adv_PasswordLog_T set Status = 0 where CustId = @CustId
		end
	
		insert into tbl_Adv_PasswordLog_T(CustId, Password, CreatedBy, Status)
		select @CustId,@Password,@CustId,1 

		select 'valid'
	end
	else 
	begin
		select 'invalid'
	end
end

select * from tbl_Adv_CustLog_T
select * from tbl_Adv_PasswordLog_T
update tbl_Adv_CustLog_T set CustId = 'INAA0005' where CustId = 'AA0009'r

select * from tbl_Customer_Registration_T
select * from tbl_Adv_AdverseEvent_T
select * from tbl_Adv_CountryCode_M
select * from tbl_Adv_Doses_M
select * from tbl_Adv_Facility_T
select * from tbl_Adv_MedicalHistory_T
select * from tbl_Adv_Occupation_M
select * from tbl_Adv_Patient_T
select * from tbl_Adv_Route_M
select * from tbl_Adv_Vaccination_T
select * from tbl_Adv_Vaccine_T
select * from tbl_Adverse_submit_T
select * from 

