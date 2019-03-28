using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace StudentExercisesAPI.Models
{
    public class Exercise
    {
        public int Id { get; set; }

        [Required]
        public string ExerciseName { get; set; }

        [Required]
        public string ExerciseLanguage { get; set; }
    }
}
