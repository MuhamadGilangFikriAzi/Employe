using EmployeAPI.Models;

namespace EmployeAPI.Repository
{
    public interface IEmployeRepository
    {
        Task<ApiMessage<List<Employe>>> GetEmployes();
        Task<ApiMessage<Employe>> GetEmployeByNik(string nik);
        Task<ApiMessage<Employe>> CreateEmploye(EmployeRq request);
        Task<ApiMessage<Employe>> UpdateEmploye(EmployeRq request);
        Task<ApiMessage<Employe>> DeleteEmploye(string nik);
        Task<ApiMessage<List<Employe>>> GetEmployesByFilter(EmployeRq request);
    }
}
