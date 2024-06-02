using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EmployeAPI.Models;
using EmployeAPI.Service;
using Azure.Core;
using Microsoft.AspNetCore.Cors;

namespace EmployeAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeController : ControllerBase
    {
        //private readonly APIDbContext _context;
        private readonly IEmployeService _service;

        public EmployeController(IEmployeService service)
        {
            //_context = context;
            _service = service;
        }

        // GET: api/Employe
        [HttpGet]
        public async Task<ApiMessage<List<Employe>>> GetEmployes()
        {
            var result = await _service.GetEmployes();
            return result;
        }

        [HttpGet("[action]")]
        public async Task<ApiMessage<Employe>> GetEmployeByNik(string nik)
        {
            var result = await _service.GetEmployeByNik(nik);
            return result;
        }

        [HttpPost]
        public async Task<ApiMessage<List<Employe>>> GetEmployesByFilter(EmployeRq request)
        {
            var result = await _service.GetEmployesByFilter(request);
            return result;
        }

        [HttpPost]
        public async Task<ApiMessage<Employe>> CreateEmploye(EmployeRq request)
        {
            var result = await _service.OrcestratorProcess(request, "Create");
            return result;
        }

        [HttpPut]
        public async Task<ApiMessage<Employe>> UpdateEmploye(EmployeRq request)
        {
            var result = await _service.OrcestratorProcess(request, "Update");
            return result;
        }

        [HttpDelete]
        public async Task<ApiMessage<Employe>> DeleteEmploye(string nik)
        {
            var result = await _service.OrcestratorProcess(new EmployeRq { Nik = nik }, "Delete");
            return result;
        }
    }
}
