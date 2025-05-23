using CW9.DTOs;
using CW9.Models;

namespace CW9.Services;

public interface IDbService
{
    public Task<PatientGetDTO> GetPatientInfoByIdAsync(int patientId);
    public Task<ICollection<PrescriptionGetDTO>> GetPrescriptionsAsync();
    public Task<PrescriptionGetDTO> CreatePrescriptionAsync(PrescriptionCreateDTO dto);
}