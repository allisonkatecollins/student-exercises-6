using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.SqlClient;
using StudentExercisesAPI.Models;

//Student JSON response should have all exercises that are assigned to them if the include=exercise query string parameter is there.

namespace StudentExercisesAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private readonly IConfiguration _config;

        public StudentController(IConfiguration config)
        {
            _config = config;
        }

        public SqlConnection Connection
        {
            get
            {
                string connectionString = _config.GetConnectionString("DefaultConnection");
                return new SqlConnection(connectionString);
            }
        }

        // GET: api/Student?include=exercise&q=bob
        //Code for getting a list of students
        [HttpGet]
        public IEnumerable<Student> Get(string include, string q)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    if (include == "exercise")
                    {
                        //provide support and the q query string parameter
                        //SQL should search relevant property for a match, search all properties of the resource for a match
                        //should search for FirstName, LastName, and SlackHandle

                        //use LIKE in the SQL query 
                        //The LIKE condition allows wildcards to be used in the WHERE clause of a SELECT, INSERT, UPDATE, or DELETE statement.
                        //This allows you to perform pattern matching.

                        cmd.CommandText = @"
                                    SELECT 
                                        s.Id AS StudentId, s.FirstName, s.LastName, s.SlackHandle, s.CohortId, 
                                        c.CohortName,
                                        e.Id AS ExerciseId, e.ExerciseName, e.ExerciseLanguage 
                                    FROM Student s 
                                        LEFT JOIN Cohort c ON s.CohortId = c.Id
                                        LEFT JOIN StudentExercise se ON s.Id = se.StudentId
                                        LEFT JOIN Exercise e ON se.ExerciseId = e.Id
                                    WHERE 1 = 1";
                    }
                    else
                    {
                        cmd.CommandText = @"
                                    SELECT
                                        s.Id as StudentId, s.FirstName, s.LastName, s.SlackHandle, s.CohortId,
                                        c.CohortName
                                    FROM Student s
                                        LEFT JOIN Cohort c on s.CohortId = c.Id
                                    WHERE 1 = 1";
                    }
                    if (!string.IsNullOrWhiteSpace(q))
                    {
                        cmd.CommandText += @" AND
                                             (s.FirstName LIKE @q OR
                                              s.LastName LIKE @q OR
                                              s.SlackHandle LIKE @q)";
                        cmd.Parameters.Add(new SqlParameter("@q", $"%{q}%"));
                    }

                    SqlDataReader reader = cmd.ExecuteReader();

                    Dictionary<int, Student> students = new Dictionary<int, Student>();
                    while (reader.Read())
                    {
                        int studentId = reader.GetInt32(reader.GetOrdinal("StudentId"));
                        if (!students.ContainsKey(studentId))
                        {
                            Student newStudent = new Student
                            {
                                Id = studentId,
                                FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                                LastName = reader.GetString(reader.GetOrdinal("LastName")),
                                SlackHandle = reader.GetString(reader.GetOrdinal("SlackHandle")),
                                CohortId = reader.GetInt32(reader.GetOrdinal("CohortId")),
                                Cohort = new Cohort
                                {
                                    Id = reader.GetInt32(reader.GetOrdinal("CohortId")),
                                    CohortName = reader.GetString(reader.GetOrdinal("CohortName"))
                                }
                            };
                            students.Add(studentId, newStudent);
                        }
                        if (include == "exercise")
                        {
                            if (!reader.IsDBNull(reader.GetOrdinal("ExerciseId")))
                            {
                                Student currentStudent = students[studentId];
                                currentStudent.Exercises.Add(
                                    new Exercise
                                    {
                                        Id = reader.GetInt32(reader.GetOrdinal("ExerciseId")),
                                        ExerciseName = reader.GetString(reader.GetOrdinal("ExerciseName")),
                                        ExerciseLanguage = reader.GetString(reader.GetOrdinal("ExerciseLanguage"))
                                    }
                                );
                            }
                        }
                    }
                    reader.Close();
                    return students.Values.ToList();

                }
            }
        }

        // GET: api/Student/5
        //get single student
        [HttpGet("{id}", Name = "GetSingleStudent")]
        public Student Get(int id, string include)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    if (include == "exercise")
                    {
                        cmd.CommandText = @"
                                    SELECT 
                                        s.Id AS StudentId, s.FirstName, s.LastName, s.SlackHandle, s.CohortId, 
                                        c.CohortName,
                                        e.Id AS ExerciseId, e.ExerciseName, e.ExerciseLanguage 
                                    FROM Student s 
                                        LEFT JOIN StudentExercise se ON s.Id = se.StudentId
                                        LEFT JOIN Exercise e ON se.ExerciseId = e.Id
                                        LEFT JOIN Cohort c ON s.CohortId = c.Id";
                    }
                    else
                    {
                        cmd.CommandText = @"
                                    SELECT
                                        s.id as StudentId, s.FirstName, s.LastName, s.SlackHandle, s.CohortId,
                                        c.CohortName
                                    FROM Student s
                                        LEFT JOIN Cohort c on s.CohortId = c.id";
                    }

                    cmd.CommandText += "WHERE s.id = @id";
                    cmd.Parameters.Add(new SqlParameter("@id", id));

                    SqlDataReader reader = cmd.ExecuteReader();

                    Student student = null;

                    while (reader.Read())
                    {
                        if (student == null)
                        {
                            student = new Student
                            {
                                Id = id,
                                FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                                LastName = reader.GetString(reader.GetOrdinal("LastName")),
                                SlackHandle = reader.GetString(reader.GetOrdinal("SlackHandle")),
                                CohortId = reader.GetInt32(reader.GetOrdinal("CohortId")),
                                Cohort = new Cohort
                                {
                                    Id = reader.GetInt32(reader.GetOrdinal("CohortId")),
                                    CohortName = reader.GetString(reader.GetOrdinal("CohortName"))
                                }
                            };
                        }

                        if (include == "exercise")
                        {
                            if (!reader.IsDBNull(reader.GetOrdinal("ExerciseId")))
                            {
                                student.Exercises.Add(
                                    new Exercise
                                    {
                                        Id = reader.GetInt32(reader.GetOrdinal("ExerciseId")),
                                        ExerciseLanguage = reader.GetString(reader.GetOrdinal("ExerciseLanguage")),
                                        ExerciseName = reader.GetString(reader.GetOrdinal("ExerciseName")),
                                    }
                                );
                            }
                        }
                    }

                    reader.Close();
                    return student;
                }
            }
        }

        // POST: api/Student
        //add a new student
        [HttpPost]
        public ActionResult Post([FromBody] Student newStudent)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"INSERT INTO student (firstname, lastname, slackhandle, cohortid)
                                        OUTPUT INSERTED.Id
                                        VALUES (@firstName, @lastName, @slackHandle, @cohortId)";
                    cmd.Parameters.Add(new SqlParameter("@firstName", newStudent.FirstName));
                    cmd.Parameters.Add(new SqlParameter("@lastName", newStudent.LastName));
                    cmd.Parameters.Add(new SqlParameter("@slackHandle", newStudent.SlackHandle));
                    cmd.Parameters.Add(new SqlParameter("@cohortId", newStudent.CohortId));

                    int newId = (int)cmd.ExecuteScalar();
                    newStudent.Id = newId;
                    return CreatedAtRoute("GetSingleStudent", new { id = newId }, newStudent);
                }
            }
        }

        // PUT: api/Student/5
        //edit existing student
        [HttpPut("{id}")]
        public void Put([FromRoute] int id, [FromBody] Student student)
        {
            try
            {
                using (SqlConnection conn = Connection)
                {
                    conn.Open();
                    using (SqlCommand cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = @"UPDATE Student
                                            SET Id = @id,
                                                FirstName = @firstName,
                                                LastName = @lastName,
                                                SlackHandle = @slackHandle,
                                                CohortId = @cohortId
                                            WHERE Id = @id";
                        cmd.Parameters.Add(new SqlParameter("@id", student.Id));
                        cmd.Parameters.Add(new SqlParameter("@firstName", student.FirstName));
                        cmd.Parameters.Add(new SqlParameter("@lastName", student.LastName));
                        cmd.Parameters.Add(new SqlParameter("@slackHandle", student.SlackHandle));
                        cmd.Parameters.Add(new SqlParameter("@cohortId", student.CohortId));

                        int rowsAffected = cmd.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            return;
                        }
                        throw new Exception("No rows affected");
                    }
                }
            }
            catch (Exception)
            {
                if (!StudentExists(id))
                {
                    return;
                }
                else
                {
                    throw;
                }
            }
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            try
            {
                using (SqlConnection conn = Connection)
                {
                    conn.Open();
                    using (SqlCommand cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = @"DELETE FROM Student WHERE Id = @id";
                        cmd.Parameters.Add(new SqlParameter("@id", id));

                        int rowsAffected = cmd.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            return new StatusCodeResult(StatusCodes.Status204NoContent);
                        }
                        throw new Exception("No rows affected");
                    }
                }
            }
            catch (Exception)
            {
                if (!StudentExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
        }

        private bool StudentExists(int id)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                        SELECT Id, FirstName, LastName, SlackHandle, CohortId
                        FROM Student
                        WHERE Id = @id";
                    cmd.Parameters.Add(new SqlParameter("@id", id));

                    SqlDataReader reader = cmd.ExecuteReader();
                    return reader.Read();
                }
            }
        }
    }
}
