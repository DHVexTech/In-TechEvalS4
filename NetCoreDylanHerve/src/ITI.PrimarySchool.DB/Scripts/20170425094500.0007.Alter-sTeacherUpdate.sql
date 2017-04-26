drop procedure iti.sTeacherUpdate;

GO;

CREATE proc iti.sTeacherUpdate
(
    @TeacherId int,
    @FirstName nvarchar(32),
    @LastName  nvarchar(32),
	@IsHere int
)
as
begin
    update iti.tTeacher
    set FirstName = @FirstName,
        LastName = @LastName,
		IsHere = @IsHere
    where TeacherId = @TeacherId;
    return 0;
end;