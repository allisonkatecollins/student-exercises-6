﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace StudentExercisesAPI.Models
{
    public class Cohort
    {
        public int Id { get; set; }

        [Required]
        [StringLength(11, MinimumLength = 5)]
        public string CohortName { get; set; }

        public List<Student> students { get; set; } = new List<Student>();
        public List<Instructor> instructors { get; set; } = new List<Instructor>();
    }
}
