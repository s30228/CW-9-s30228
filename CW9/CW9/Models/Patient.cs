using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CW9.Models;

[Table("Patient")]
public class Patient
{
    [Key] public int IdPatient { get; set; }
    [MaxLength(100)] public string FirstName { get; set; }
    [MaxLength(100)] public string LastName { get; set; }
    public DateOnly Birthdate { get; set; }
    
    // wlasciwosc nawigacyjna
    public virtual ICollection<Prescription> Prescriptions { get; set; } = null!;
}