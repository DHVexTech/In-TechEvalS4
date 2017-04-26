﻿drop procedure iti.sStudentUpdate;

go;

create proc iti.sStudentUpdate
(
    @StudentId   int,
    @FirstName   nvarchar(64),
    @LastName    nvarchar(64),
    @BirthDate   datetime2,
	@Photo		 nvarchar(256),
    @GitHubLogin nvarchar(64),
	@ClassId int
)
as
begin
    update iti.tStudent
    set FirstName = @FirstName,
        LastName = @LastName,
        BirthDate = @BirthDate,
		Photo = @Photo,
		ClassId = @ClassId
    where StudentId = @StudentId;

    if(@GitHubLogin <> N'')
    begin
        if exists(select * from iti.tGitHubStudent s where s.StudentId = @StudentId)
        begin
            update iti.tGitHubStudent
            set GitHubLogin = @GitHubLogin
            where StudentId = @StudentId;
        end
        else
        begin
            insert into iti.tGitHubStudent(StudentId, GitHubLogin) values(@StudentId, @GitHubLogin);
        end;
    end
    else
    begin
        if exists(select * from iti.tGitHubStudent s where s.StudentId = @StudentId)
        begin
            delete from iti.tGitHubStudent where StudentId = @StudentId;
        end;
    end;
    return 0;
end;