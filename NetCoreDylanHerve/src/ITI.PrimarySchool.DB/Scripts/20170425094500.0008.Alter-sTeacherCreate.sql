DROP procedure iti.sTeacherCreate;

GO;

CREATE proc iti.sTeacherCreate
(
    @FirstName nvarchar(32),
    @LastName  nvarchar(32),
	@IsHere int
)
as
begin
    insert into iti.tTeacher(FirstName, LastName, IsHere) values(@FirstName, @LastName, @IsHere);
    return 0;
end;