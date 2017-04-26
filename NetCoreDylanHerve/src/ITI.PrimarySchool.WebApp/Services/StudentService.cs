using System;
using System.Collections.Generic;
using ITI.PrimarySchool.DAL;
using System.Linq;

namespace ITI.PrimarySchool.WebApp.Services
{
    public class StudentService
    {
        readonly StudentGateway _studentGateway;

        public StudentService( StudentGateway studentGateway )
        {
            _studentGateway = studentGateway;
        }

        public Result<Student> CreateStudent( string firstName, string lastName, DateTime birthDate, string photo, string gitHubLogin, int classe)
        {
            if( !IsNameValid( firstName ) ) return Result.Failure<Student>( Status.BadRequest, "The first name is not valid." );
            if( !IsNameValid( lastName ) ) return Result.Failure<Student>( Status.BadRequest, "The last name is not valid." );
            if( _studentGateway.FindByName( firstName, lastName ) != null ) return Result.Failure<Student>( Status.BadRequest, "A student with this name already exists." );
            if( !string.IsNullOrEmpty( gitHubLogin ) && _studentGateway.FindByGitHubLogin( gitHubLogin ) != null ) return Result.Failure<Student>( Status.BadRequest, "A student with GitHub login already exists." );
            _studentGateway.Create( firstName, lastName, birthDate, photo, gitHubLogin, classe);
            Student student = _studentGateway.FindByName( firstName, lastName );
            return Result.Success( Status.Created, student );
        }

        public Result<Student> UpdateStudent( int studentId, string firstName, string lastName, DateTime birthDate, string photo, string gitHubLogin, int classe)
        {
            if( !IsNameValid( firstName ) ) return Result.Failure<Student>( Status.BadRequest, "The first name is not valid." );
            if( !IsNameValid( lastName ) ) return Result.Failure<Student>( Status.BadRequest, "The last name is not valid." );
            Student student;
            if( ( student = _studentGateway.FindById( studentId ) ) == null )
            {
                return Result.Failure<Student>( Status.NotFound, "Student not found." );
            }

            {
                Student s = _studentGateway.FindByName( firstName, lastName );
                if( s != null && s.StudentId != student.StudentId ) return Result.Failure<Student>( Status.BadRequest, "A student with this name already exists." );
            }

            if( !string.IsNullOrEmpty( gitHubLogin ) )
            {
                Student s = _studentGateway.FindByGitHubLogin( gitHubLogin );
                if( s != null && s.StudentId != student.StudentId ) return Result.Failure<Student>( Status.BadRequest, "A student with this GitHub login already exists." );
            }

            _studentGateway.Update( studentId, firstName, lastName, birthDate, photo, gitHubLogin, classe );
            student = _studentGateway.FindById( studentId );
            return Result.Success( Status.Ok, student );
        }

        public Result<Student> GetById( int id )
        {
            Student student;
            if( ( student = _studentGateway.FindById( id ) ) == null ) return Result.Failure<Student>( Status.NotFound, "Student not found." );
            return Result.Success( Status.Ok, student );
        }

        public Result<int> Delete( int classId )
        {
            if( _studentGateway.FindById( classId ) == null ) return Result.Failure<int>( Status.NotFound, "Student not found." );
            _studentGateway.Delete( classId );
            return Result.Success( Status.Ok, classId );
        }

        public Result<IEnumerable<Student>> GetAll()
        {
            return Result.Success( Status.Ok, _studentGateway.GetAll() );
        }

        public Result<IEnumerable<Student>> GetByClassId(int classId)
        {
            return Result.Success(Status.Ok, _studentGateway.GetByClassId(classId));
        }

        public Result<IEnumerable<Student>> FilterName(string name)
        {
            string firstName = "";
            string lastName = "";

            if (name.Contains(" "))
            {
                lastName = name.Substring(0, name.IndexOf(" ")).ToLower();
                firstName = name.Substring(name.IndexOf(" ")+1).ToLower();
            }
            else
            {
                firstName = name.ToLower();
            }


            bool matchOne = false, matchTwo = false;

            IEnumerable<Student> resultGetAll = _studentGateway.GetAll();
            List<Student> resultFilter = new List<Student>();

            foreach(Student t in resultGetAll)
            {
                matchOne = matchTwo = false;
                if (t.FirstName.ToLower().Contains(firstName))
                {
                    matchOne = true;
                    resultFilter.Add(t);
                }

                if (t.LastName.ToLower().Contains(lastName) && lastName != "")
                {
                    matchTwo = true;
                    if(!matchOne)
                        resultFilter.Add(t);
                }

                if(matchOne && matchTwo && (t.FirstName.Length == firstName.Length && t.LastName.Length == lastName.Length || t.FirstName.Length == lastName.Length && t.LastName.Length == firstName.Length))
                {
                    resultFilter.Clear();
                    resultFilter.Add(t);
                    break;
                }
            }
            resultGetAll = resultFilter;
            return Result.Success(Status.Ok, resultGetAll);
        }

        bool IsNameValid( string name ) => !string.IsNullOrWhiteSpace( name );
    }
}
