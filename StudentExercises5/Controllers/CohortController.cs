using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.SqlClient;
using StudentExercises6.Models;

//Cohort JSON representation should include array of students, and the instructor.

namespace StudentExercises6.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CohortController : ControllerBase
    {
        private readonly IConfiguration _config;

        public CohortController(IConfiguration config)
        {
            _config = config;
        }

        public SqlConnection Connection
        {
            get
            {
                string connectionString = "Server=ALLISONCOLLINS-\\SQLEXPRESS; Database=StudentExercises; Integrated Security = True; Connect Timeout = 30; Encrypt = False; TrustServerCertificate = False; ApplicationIntent = ReadWrite; MultiSubnetFailover = False";
                return new SqlConnection(connectionString);
            }
        }

        // GET: api/Cohort
        //Code for getting a list of cohorts
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                        SELECT 
                            c.Id AS CohortId, c.CohortName, 
                            s.Id AS StudentId,
                            s.FirstName AS StudentFirstName,
                            s.LastName AS StudentLastName, 
                            s.SlackHandle AS StudentSlack, 
                            s.CohortId AS StudentCohort, 
                            i.id AS InstructorId,
                            i.FirstName AS InstructorFirstName, 
                            i.LastName AS InstructorLastName, 
                            i.SlackHandle AS InstructorSlack, 
                            i.CohortId AS InstructorCohort
                        FROM Cohort c
                        LEFT JOIN Student s ON c.Id = s.CohortId
                        LEFT JOIN Instructor i on c.Id = i.CohortId;";
                    SqlDataReader reader = cmd.ExecuteReader();

                    Dictionary<int, Cohort> cohorts = new Dictionary<int, Cohort>();
                    while (reader.Read())
                    {
                        int cohortId = reader.GetInt32(reader.GetOrdinal("CohortId"));
                        if (!cohorts.ContainsKey(cohortId))
                        {
                            Cohort newCohort = new Cohort
                            {
                                Id = cohortId,
                                CohortName = reader.GetString(reader.GetOrdinal("CohortName")),
                                students = new List<Student>(),
                                instructors = new List<Instructor>()
                            };
                            cohorts.Add(cohortId, newCohort);
                        }
                            Cohort currentCohort = cohorts[cohortId];
                        if (!currentCohort.students.Exists(x => x.Id == reader.GetInt32(reader.GetOrdinal("StudentId"))))
                        {
                            currentCohort.students.Add(
                             new Student
                             {
                                 Id = reader.GetInt32(reader.GetOrdinal("StudentId")),
                                 FirstName = reader.GetString(reader.GetOrdinal("StudentFirstName")),
                                 LastName = reader.GetString(reader.GetOrdinal("StudentLastName")),
                                 SlackHandle = reader.GetString(reader.GetOrdinal("StudentSlack")),
                                 CohortId = reader.GetInt32(reader.GetOrdinal("StudentCohort"))
                             }
                            );
                        }

                        if (!currentCohort.instructors.Exists(x => x.Id == reader.GetInt32(reader.GetOrdinal("InstructorId"))))
                        {
                            
                            currentCohort.instructors.Add(
                             new Instructor
                             {
                                 Id = reader.GetInt32(reader.GetOrdinal("InstructorId")),
                                 FirstName = reader.GetString(reader.GetOrdinal("InstructorFirstName")),
                                 LastName = reader.GetString(reader.GetOrdinal("InstructorLastName")),
                                 SlackHandle = reader.GetString(reader.GetOrdinal("InstructorSlack")),
                                 CohortId = reader.GetInt32(reader.GetOrdinal("InstructorCohort"))
                             }
                            );
                        }
                    }
                    reader.Close();
                    return Ok(cohorts);
                }
            }
        }

        // GET: api/Cohort/5
        //get single cohort
        [HttpGet("{id}", Name = "GetCohort")]
        public async Task<IActionResult> Get([FromRoute] int id)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                        SELECT 
                            c.Id, c.CohortName, 
                            s.Id AS StudentId,
                            s.FirstName AS StudentFirstName, 
                            s.LastName AS StudentLastName, 
                            s.SlackHandle AS StudentSlack, 
                            s.CohortId AS StudentCohort, 
                            i.id AS InstructorId,
                            i.FirstName AS InstructorFirstName, 
                            i.LastName AS InstructorLastName, 
                            i.SlackHandle AS InstuctorSlack, 
                            i.CohortId AS InstructorCohort
                        FROM Cohort c
                        LEFT JOIN Student s ON c.Id = s.CohortId
                        LEFT JOIN Instructor i on c.Id = i.CohortId;";
                    cmd.Parameters.Add(new SqlParameter("@id", id));
                    SqlDataReader reader = cmd.ExecuteReader();

                    Dictionary<int, Cohort> cohorts = new Dictionary<int, Cohort>();
                    if (reader.Read())
                    {
                        int cohortId = reader.GetInt32(reader.GetOrdinal("CohortId"));
                        if (!cohorts.ContainsKey(cohortId))
                        {
                            Cohort newCohort = new Cohort
                            {
                                Id = cohortId,
                                CohortName = reader.GetString(reader.GetOrdinal("CohortName")),
                                students = new List<Student>(),
                                instructors = new List<Instructor>()
                            };
                            cohorts.Add(cohortId, newCohort);
                        }

                        {
                            Cohort currentCohort = cohorts[cohortId];
                            currentCohort.students.Add(
                             new Student
                             {
                                 Id = reader.GetInt32(reader.GetOrdinal("StudentId")),
                                 FirstName = reader.GetString(reader.GetOrdinal("StudentFirstName")),
                                 LastName = reader.GetString(reader.GetOrdinal("StudentLastName")),
                                 SlackHandle = reader.GetString(reader.GetOrdinal("StudentSlack")),
                                 CohortId = reader.GetInt32(reader.GetOrdinal("StudentCohort"))
                             }
                            );
                        }

                        {
                            Cohort currentCohort = cohorts[cohortId];
                            currentCohort.instructors.Add(
                             new Instructor
                             {
                                 Id = reader.GetInt32(reader.GetOrdinal("InstructorId")),
                                 FirstName = reader.GetString(reader.GetOrdinal("InstructorFirstName")),
                                 LastName = reader.GetString(reader.GetOrdinal("InstructorLastName")),
                                 SlackHandle = reader.GetString(reader.GetOrdinal("InstructorSlack")),
                                 CohortId = reader.GetInt32(reader.GetOrdinal("InstructorCohort"))
                             }
                            );
                        }
                    }
                    reader.Close();
                    return Ok(cohorts);
                }
            }
        }

        // POST: api/Cohort
        //add a new cohort
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Cohort cohort)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"INSERT INTO Cohort (Id, CohortName)
                                        OUTPUT INSERTED.Id
                                        VALUES (@id, @cohortName)";
                    cmd.Parameters.Add(new SqlParameter("@id", cohort.Id));
                    cmd.Parameters.Add(new SqlParameter("@cohortName", cohort.CohortName));

                    int newId = (int)cmd.ExecuteScalar();
                    cohort.Id = newId;
                    return CreatedAtRoute("GetCohort", new { id = newId }, cohort);
                }
            }
        }

        // PUT: api/Cohort/5
        //edit existing cohort
        [HttpPut("{id}")]
        public async Task<IActionResult> Put([FromRoute] int id, [FromBody] Cohort cohort)
        {
            try
            {
                using (SqlConnection conn = Connection)
                {
                    conn.Open();
                    using (SqlCommand cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = @"UPDATE Cohort
                                            SET Id = @id,
                                                CohortName = @cohortName
                                            WHERE Id = @id";
                        cmd.Parameters.Add(new SqlParameter("@id", cohort.Id));
                        cmd.Parameters.Add(new SqlParameter("@cohortName", cohort.CohortName));

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
                if (!CohortExists(id))
                {
                    return NotFound();
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
                        cmd.CommandText = @"DELETE FROM Cohort WHERE Id = @id";
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
                if (!CohortExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
        }

        private bool CohortExists(int id)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                        SELECT Id, CohortName
                        FROM Cohort
                        WHERE Id = @id";
                    cmd.Parameters.Add(new SqlParameter("@id", id));

                    SqlDataReader reader = cmd.ExecuteReader();
                    return reader.Read();
                }
            }
        } 
    }
}

