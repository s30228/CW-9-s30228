using System.ComponentModel.DataAnnotations;

namespace CW9.DTOs;

public class PatientCreateDTO
{
    [MaxLength(100)] public required string FirstName { get; set; }
    [MaxLength(100)] public required string LastName { get; set; }
    public required DateOnly Birthdate { get; set; }
}