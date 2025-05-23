using System.ComponentModel.DataAnnotations;

namespace CW9.DTOs;

public class PrescriptionCreateDTO
{
    public required PatientCreateDTO Patient { get; set; }
    public int IdDoctor { get; set; }
    public required IEnumerable<MedicamentDetailsDTO> Medicaments { get; set; }
    public required DateOnly Date { get; set; }
    public required DateOnly DueDate { get; set; }
}

public class MedicamentDetailsDTO
{
    public int IdMedicament { get; set; }
    public int Dose { get; set; }
    public string Description { get; set; }
}