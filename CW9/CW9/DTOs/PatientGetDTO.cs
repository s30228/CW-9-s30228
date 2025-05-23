using CW9.Models;

namespace CW9.DTOs;

public class PatientGetDTO
{
    public int IdPatient { get; set; }
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public DateOnly Birthdate { get; set; }
    public IEnumerable<PrescriptionDTO> Prescriptions { get; set; }
}

public class PrescriptionDTO
{
    public int IdPrescription { get; set; }
    public DateOnly Date { get; set; }
    public DateOnly DueDate { get; set; }
    public IEnumerable<MedicamentDTO> Medicaments { get; set; }
    public DoctorDTO Doctor { get; set; }
}

public class MedicamentDTO
{
    public int IdMedicament { get; set; }
    public string Name { get; set; }  = null!;
    public string Description { get; set; } = null!; 
    public string Type { get; set; }  = null!;
    public int? Dose { get; set; }
    public string Details { get; set; } = null!;
}

public class DoctorDTO
{
    public int IdDoctor { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
}