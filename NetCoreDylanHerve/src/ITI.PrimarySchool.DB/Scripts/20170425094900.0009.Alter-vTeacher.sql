alter view iti.vTeacher
as
    select
        TeacherId = t.TeacherId,
        FirstName = t.FirstName,
        LastName = t.LastName,
		IsHere = t.IsHere
    from iti.tTeacher t
    where t.TeacherId <> 0;