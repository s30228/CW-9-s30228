using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CW9.Models;

[Table("Prescription_Medicament")]
[PrimaryKey("IdMedicament", "IdPrescription")]
public class PrescriptionMedicament
{
    public int IdMedicament { get; set; }
    public int IdPrescription { get; set; }
    public int? Dose { get; set; }
    public string Details { get; set; } = null!;
    
    [ForeignKey("IdMedicament")]
    public virtual Medicament Medicament { get; set; } = null!;
    [ForeignKey("IdPrescription")]
    public virtual Prescription Prescription { get; set; } = null!;
}