using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentExercises6.Models
{
    public class Cohort
    {
        public int Id { get; set; }
        public string CohortName { get; set; }
        public List<Student> students { get; set; } = new List<Student>();
        public List<Instructor> instructors { get; set; } = new List<Instructor>();
    }
}
