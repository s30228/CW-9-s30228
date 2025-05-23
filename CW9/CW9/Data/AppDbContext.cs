using CW9.Models;
using Microsoft.EntityFrameworkCore;

namespace CW9.Data;

public class AppDbContext : DbContext
{
    public DbSet<Doctor> Doctors { get; set; }
    public DbSet<Patient> Patients { get; set; }
    public DbSet<Prescription> Prescriptions { get; set; }
    public DbSet<Medicament> Medicaments { get; set; }
    public DbSet<PrescriptionMedicament> PrescriptionMedicaments { get; set; }
    
    public AppDbContext(DbContextOptions options) : base(options)
    {
        
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        var doctor = new Doctor
        {
            IdDoctor = 1,
            FirstName = "John",
            LastName = "Doe",
            Email = "johndoe@gmail.com"
        };

        var patient = new Patient
        {
            IdPatient = 1,
            FirstName = "Jane",
            LastName = "Doe",
            Birthdate = DateOnly.FromDateTime(DateTime.Now)
        };

        var prescription = new Prescription
        {
            IdPrescription = 1,
            Date = DateOnly.FromDateTime(DateTime.Now),
            DueDate = DateOnly.FromDateTime(DateTime.Now),
            IdPatient = 1,
            IdDoctor = 1
        };

        var medicament = new Medicament
        {
            IdMedicament = 1,
            Name = "Ibuprofen",
            Description = "Relieves pain, fever, and inflammation",
            Type = "NSAID"
        };

        var prescriptionMedicament = new PrescriptionMedicament
        {
            IdMedicament = 1,
            IdPrescription = 1,
            Dose = null,
            Details = "Welp, hope this will help"
        };

        modelBuilder.Entity<Doctor>().HasData([doctor]);
        modelBuilder.Entity<Patient>().HasData([patient]);
        modelBuilder.Entity<Prescription>().HasData([prescription]);
        modelBuilder.Entity<Medicament>().HasData([medicament]);
        modelBuilder.Entity<PrescriptionMedicament>().HasData([prescriptionMedicament]);
    }
}