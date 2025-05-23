using CW9.Data;
using CW9.DTOs;
using CW9.Exceptions;
using CW9.Models;
using Microsoft.EntityFrameworkCore;

namespace CW9.Services;

public class DbService(AppDbContext data) : IDbService
{
    public async Task<PatientGetDTO> GetPatientInfoByIdAsync(int patientId)
    {
        var result = await data.Patients.Select(p => new PatientGetDTO
        {
            IdPatient = p.IdPatient,
            FirstName = p.FirstName,
            LastName = p.LastName,
            Birthdate = p.Birthdate,
            Prescriptions = p.Prescriptions
                .OrderBy(pr => pr.DueDate)
                .Select(pr => new PrescriptionDTO
            {
                IdPrescription = pr.IdPrescription,
                Date = pr.Date,
                DueDate = pr.DueDate,
                Medicaments = pr.PrescriptionMedicaments.Select(pm => new MedicamentDTO
                {
                    IdMedicament = pm.IdMedicament,
                    Name = pm.Medicament.Name,
                    Description = pm.Medicament.Description,
                    Type = pm.Medicament.Type,
                    Dose = pm.Dose,
                    Details = pm.Details
                }).ToList(),
                Doctor = new DoctorDTO
                {
                    IdDoctor = pr.IdDoctor,
                    FirstName = pr.Doctor.FirstName,
                    LastName = pr.Doctor.LastName,
                    Email = pr.Doctor.Email
                }
            }).ToList()
        }).FirstOrDefaultAsync(p => p.IdPatient == patientId);
        
        return result ?? throw new NotFoundException($"Patient with id: {patientId} not found");
    }

    public async Task<ICollection<PrescriptionGetDTO>> GetPrescriptionsAsync()
    {
        return await data.Prescriptions.Select(p => new PrescriptionGetDTO
        {
            IdPrescription = p.IdPrescription,
            Date = p.Date,
            DueDate = p.DueDate,
            IdDoctor = p.IdDoctor,
            IdPatient = p.IdPatient
        }).ToListAsync();
    }

    public async Task<PrescriptionGetDTO> CreatePrescriptionAsync(PrescriptionCreateDTO dto)
    {
        if (dto.Medicaments.Count() > 10)
            throw new ArgumentException("Cannot prescribe more than 10 medicaments");
        
        if (dto.DueDate < dto.Date)
            throw new ArgumentException("DueDate cannot be earlier than Date");
        
        var patient = await data.Patients
            .FirstOrDefaultAsync(p => p.FirstName == dto.Patient.FirstName &&
                                      p.LastName == dto.Patient.LastName &&
                                      p.Birthdate == dto.Patient.Birthdate);
        
        if (patient == null)
        {
            patient = new Patient
            {
                FirstName = dto.Patient.FirstName,
                LastName = dto.Patient.LastName,
                Birthdate = dto.Patient.Birthdate
            };
            data.Patients.Add(patient);
            await data.SaveChangesAsync();
        }
        
        var doctorExists = await data.Doctors.AnyAsync(d => d.IdDoctor == dto.IdDoctor);
        if (!doctorExists)
            throw new ArgumentException($"Doctor with ID {dto.IdDoctor} does not exist.");
        
        var medIds = dto.Medicaments.Select(m => m.IdMedicament).ToList();
        var existingMeds = await data.Medicaments
            .Where(m => medIds.Contains(m.IdMedicament))
            .Select(m => m.IdMedicament)
            .ToListAsync();

        var missing = medIds.Except(existingMeds).ToList();
        if (missing.Any())
            throw new ArgumentException($"The following medicament IDs do not exist: {string.Join(", ", missing)}");
        
        var transaction = await data.Database.BeginTransactionAsync();
        try
        {
            var prescription = new Prescription
            {
                Date = dto.Date,
                DueDate = dto.DueDate,
                IdPatient = patient.IdPatient,
                IdDoctor = dto.IdDoctor
            };
            data.Prescriptions.Add(prescription);
            await data.SaveChangesAsync();

            foreach (var med in dto.Medicaments)
            {
                data.PrescriptionMedicaments.Add(new PrescriptionMedicament
                {
                    IdPrescription = prescription.IdPrescription,
                    IdMedicament = med.IdMedicament,
                    Dose = med.Dose,
                    Details = med.Description
                });
            }

            await data.SaveChangesAsync();
            
            return new PrescriptionGetDTO
            {
                IdPrescription = prescription.IdPrescription,
                Date = prescription.Date,
                DueDate = prescription.DueDate,
                IdDoctor = prescription.IdDoctor,
                IdPatient = patient.IdPatient
            };
        } catch (Exception)
        { 
            await transaction.RollbackAsync();
            throw;
        }
    }
}