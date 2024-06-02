using EmployeAPI.Models;
using EmployeAPI.Repository;
using Microsoft.AspNetCore.Mvc;

namespace EmployeAPI.Service
{
    public class EmployeService : IEmployeService
    {
        private readonly IEmployeRepository _employeRepository;
        public EmployeService(IEmployeRepository employeRepository)
        {
            _employeRepository = employeRepository;
        }
        public async Task<ApiMessage<List<Employe>>> GetEmployes()
        {
            ApiMessage<List<Employe>> result = new ApiMessage<List<Employe>>();
            result.Data = new List<Employe>();
            result.ModuleName = "Employe";
            try
            {
                var repo = await _employeRepository.GetEmployes();
                if (!repo.IsSucces)
                {
                    result.Message.AddRange(repo.Message);
                    result.IsSucces = false;
                } else
                {
                    result.Data = repo.Data;
                }
            } catch (Exception ex)
            {
                result.Message.Add(new AdditionalMessage() { Code = 417, Description = ex.Message, Source = "GetEmployes" });
                result.IsSucces = false;
            }

            return result;
        }

        public async Task<ApiMessage<List<Employe>>> GetEmployesByFilter(EmployeRq request)
        {
            ApiMessage<List<Employe>> result = new ApiMessage<List<Employe>>();
            result.Data = new List<Employe>();
            result.ModuleName = "Employe";
            try
            {
                var repo = await _employeRepository.GetEmployesByFilter(request);
                if (!repo.IsSucces)
                {
                    result.Message.AddRange(repo.Message);
                    result.IsSucces = false;
                } else
                {
                    result.Data = repo.Data;
                }
            }
            catch(Exception ex)
            {
                result.Message.Add(new AdditionalMessage() { Code = 417, Description = ex.Message, Source = "GetEmployesByFilter" });
                result.IsSucces = false;
            }

            return result;
        }

        public async Task<ApiMessage<Employe>> GetEmployeByNik(string nik)
        {
            ApiMessage<Employe> result = new ApiMessage<Employe>();
            result.Data = new Employe();
            result.ModuleName = "Employe";
            try
            {
                var repo = await _employeRepository.GetEmployeByNik(nik);
                if (!repo.IsSucces)
                {
                    result.Message.AddRange(repo.Message);
                    result.IsSucces = false;
                }
                else
                {
                    result.Data = repo.Data;
                }
            }
            catch (Exception ex)
            {
                result.Message.Add(new AdditionalMessage() { Code = 417, Description = ex.Message, Source = "GetEmployeByNik" });
                result.IsSucces = false;
            }

            return result;
        }

        public async Task<ApiMessage<Employe>> GlobalValidate(EmployeRq request, string action)
        {
            ApiMessage<Employe> result = new ApiMessage<Employe>(new Employe());
            try
            {
                if (string.IsNullOrEmpty(request.Nik))
                    result.Message.Add(new AdditionalMessage() { Code = 400, Description = "NIK Wajib Diisi", Source = "GlobalValidate" });

                if (action != "Delete")
                {

                    if (action == "Create")
                    {
                        var resp = await _employeRepository.GetEmployeByNik(request.Nik);
                        if (resp.Data != null)
                            result.Message.Add(new AdditionalMessage() { Code = 400, Description = "NIK Sudah Terdaftar", Source = "GlobalValidate" });

                    }

                    if (string.IsNullOrEmpty(request.Gender))
                        result.Message.Add(new AdditionalMessage() { Code = 400, Description = "Gender Wajib Diisi", Source = "GlobalValidate" });

                    if (!string.IsNullOrEmpty(request.Gender) && request.Gender != "Laki-Laki" && request.Gender != "Perempuan")
                        result.Message.Add(new AdditionalMessage() { Code = 400, Description = "Gender Hanya Bisa Diisi Laki-Laki / Perempuan", Source = "GlobalValidate" });

                    if (string.IsNullOrEmpty(request.Nama))
                        result.Message.Add(new AdditionalMessage() { Code = 400, Description = "Nama Wajib Diisi", Source = "GlobalValidate" });

                    if (string.IsNullOrEmpty(request.TglLahir.ToString()))
                        result.Message.Add(new AdditionalMessage() { Code = 400, Description = "Tanggal Lahir Wajib Diisi", Source = "GlobalValidate" });

                    if (!string.IsNullOrEmpty(request.TglLahir.ToString()) && request.TglLahir > DateTime.Now)
                        result.Message.Add(new AdditionalMessage() { Code = 400, Description = "Tanggal Lahir tidak boleh lebih besar dari hari ini", Source = "GlobalValidate" });

                    if (string.IsNullOrEmpty(request.Alamat))
                        result.Message.Add(new AdditionalMessage() { Code = 400, Description = "Alamat Wajib Diisi", Source = "GlobalValidate" });

                    if (string.IsNullOrEmpty(request.TglBergabung.ToString()) && action == "Create")
                        result.Message.Add(new AdditionalMessage() { Code = 400, Description = "TglBergabung Wajib Diisi", Source = "GlobalValidate" });

                    if (!string.IsNullOrEmpty(request.TglBergabung.ToString()) && action == "Create" && request.TglBergabung > DateTime.Now)
                        result.Message.Add(new AdditionalMessage() { Code = 400, Description = "Tanggal Bergabung tidak boleh lebih besar dari hari ini", Source = "GlobalValidate" });

                }
            } 
            catch (Exception ex)
            {
                result.Message.Add(new AdditionalMessage() { Code = 417, Description = ex.Message, Source = "GlobalValidate" });
                result.IsSucces = false;
            }

            if (result.Message.Count > 0)
                result.IsSucces = false;

            return result;
        }

        public async Task<ApiMessage<Employe>> OrcestratorProcess(EmployeRq request, string action)
        {
            ApiMessage<Employe> result = new ApiMessage<Employe>(new Employe());
            result.ModuleName = "Employe";
            try
            {
                var respValidate = await GlobalValidate(request, action);
                if (!respValidate.IsSucces)
                {
                    result.Message.AddRange(respValidate.Message);
                    result.IsSucces = respValidate.IsSucces;
                }

                if (result.IsSucces)
                {
                    ApiMessage<Employe> resp = new ApiMessage<Employe>();
                    resp.Data = new Employe();
                    if (action == "Create")
                    {
                        resp = await _employeRepository.CreateEmploye(request);
                    }

                    if (action == "Update")
                    {
                        resp = await _employeRepository.UpdateEmploye(request);
                    }

                    if (action == "Delete")
                    {
                        resp = await _employeRepository.DeleteEmploye(request.Nik);
                    }

                    if (!resp.IsSucces)
                    {
                        result.Message.AddRange(resp.Message);
                        result.IsSucces = false;
                    } else
                    {
                        result.Data = result.Data;
                        result.IsSucces = true;
                    }
                }
            }
            catch (Exception ex)
            {
                result.Message.Add(new AdditionalMessage() { Code = 417, Description = ex.Message, Source = "OrcestratorProcess" });
                result.IsSucces = false;
            }

            return result;
        }
    }
}
