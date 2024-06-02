using EmployeAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace EmployeAPI.Service
{
    public interface IEmployeService
    {
        Task<ApiMessage<List<Employe>>> GetEmployes();
        Task<ApiMessage<Employe>> GetEmployeByNik(string nik);
        Task<ApiMessage<Employe>> OrcestratorProcess(EmployeRq request, string action);
        Task<ApiMessage<List<Employe>>> GetEmployesByFilter(EmployeRq request);
    }
}
