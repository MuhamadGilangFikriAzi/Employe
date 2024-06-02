using EmployeAPI.Models;
using Humanizer;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Data;
using System.Text.Json;

namespace EmployeAPI.Repository
{
    public class EmployeRepository : IEmployeRepository
    {
        private readonly APIDbContext _dbContext;
        public EmployeRepository(APIDbContext context)
        {
            _dbContext = context;
        }

        public async Task<ApiMessage<List<Employe>>> GetEmployes()
        {
            ApiMessage<List<Employe>> result = new ApiMessage<List<Employe>>(new List<Employe>());

            try
            {
                var resp = await _dbContext.Employes.FromSql($"EXEC GetEmployes").ToListAsync();

                var getData = resp.Select(x => new Employe()
                {
                    Nik = x.Nik,
                    Nama = x.Nama,
                    TglBergabung = x.TglBergabung,
                    TglLahir = x.TglLahir,
                    Alamat = x.Alamat,
                    Gender = x.Gender,
                    CreateDate = x.CreateDate,
                    UpdateDate = x.UpdateDate
                });

                result.Data = getData.ToList();
            }
            catch (Exception ex)
            {
                result.Message.Add(new AdditionalMessage() { Code = 417, Description = ex.Message, Source = "GetEmployes" });
                result.IsSucces = false;
            }
            
            return result;
        }

        public async Task<ApiMessage<Employe>> GetEmployeByNik(string nik)
        {
            ApiMessage<Employe> result = new ApiMessage<Employe>(new Employe());

            try
            {
                var resp = await _dbContext.Employes.FromSql($"EXEC GetEmployeByNik {nik}").ToListAsync();
                result.Data = resp.FirstOrDefault();
                
            }
            catch (Exception ex)
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

            try
            {
                var query = "select Nik, Nama, Gender, TglLahir, Alamat, TglBergabung, CreateDate, UpdateDate from Employes";
                var queryCondition = new List<string>();
                if (!string.IsNullOrEmpty(request.Nik))
                    queryCondition.Add($"upper(Nik) like upper('%{request.Nik}%')");

                if (!string.IsNullOrEmpty(request.Nama))
                    queryCondition.Add($"upper(Nama) like upper('%{request.Nama}%')''");

                if (!string.IsNullOrEmpty(request.Gender))
                    queryCondition.Add($"Gender = {request.Gender}");

                if (!string.IsNullOrEmpty(request.TglLahir.ToString()))
                    queryCondition.Add($"TglLahir = {request.TglLahir.ToString()}");

                if (!string.IsNullOrEmpty(request.Alamat))
                    queryCondition.Add($"upper(Alamat) like upper('%{request.Alamat}%')");

                if (!string.IsNullOrEmpty(request.TglBergabung.ToString()))
                    queryCondition.Add($"TglBergabung = {request.TglBergabung.ToString()}");

                if (queryCondition.Count > 0)
                {
                    query += "where " + string.Join(" and ", queryCondition);
                }

                var resp = await _dbContext.Employes.FromSql($"{query}").ToListAsync();
                var getData = resp.Select(x => new Employe()
                {
                    Nik = x.Nik,
                    Nama = x.Nama,
                    TglBergabung = x.TglBergabung,
                    TglLahir = x.TglLahir,
                    Alamat = x.Alamat,
                    Gender = x.Gender,
                    CreateDate = x.CreateDate,
                    UpdateDate = x.UpdateDate
                });

                result.Data = getData.ToList();
            } 
            catch (Exception ex)
            {
                result.Message.Add(new AdditionalMessage() { Code = 417, Description = ex.Message, Source = "GetEmployeUsingFilter" });
                result.IsSucces = false;
            }

            return result;
        }

        public async Task<ApiMessage<Employe>> CreateEmploye(EmployeRq request)
        {
            ApiMessage<Employe> result = new ApiMessage<Employe>(new Employe());
            try
            {
                var resp = await _dbContext.Employes.FromSql($"EXEC CreateEmploye {request.Nik}, {request.Nama}, {request.Gender}, {request.Alamat}, {request.TglLahir}, {request.TglBergabung}").ToListAsync();
                result.Data = resp.FirstOrDefault();
            }
            catch (Exception ex)
            {
                result.Message.Add(new AdditionalMessage() { Code = 417, Description = ex.Message, Source = "CreateEmploye" });
                result.IsSucces = false;
            }
            result.Data = MappingObject<EmployeRq, Employe>(request);
            return result;
        }

        public async Task<ApiMessage<Employe>> UpdateEmploye(EmployeRq request)
        {
            ApiMessage<Employe> result = new ApiMessage<Employe>(new Employe());
            try
            {
                var resp = await _dbContext.Employes.FromSql($"EXEC UpdateEmploye {request.Nik}, {request.Nama}, {request.Gender}, {request.Alamat}, {request.TglLahir}, {request.TglBergabung}").ToListAsync();
                result.Data = resp.FirstOrDefault();
            }
            catch (Exception ex)
            {
                result.Message.Add(new AdditionalMessage() { Code = 417, Description = ex.Message, Source = "UpdateEmploye" });
                result.IsSucces = false;
            }
            result.Data = MappingObject<EmployeRq, Employe>(request);
            return result;
        }

        public async Task<ApiMessage<Employe>> DeleteEmploye(string nik)
        {
            ApiMessage<Employe> result = new ApiMessage<Employe>(new Employe());
            result.Data.Nik = nik;

            try
            {
                var resp = await _dbContext.Employes.FromSql($"EXEC DeleteEmploye {nik}").ToListAsync();
                result.Data = resp.FirstOrDefault();
            }
            catch (Exception ex)
            {
                result.Message.Add(new AdditionalMessage() { Code = 417, Description = ex.Message, Source = "CreateEmploye" });
                result.IsSucces = false;
            }
            return result;
        }


        public static TDest MappingObject<TSource, TDest>(TSource source)
            where TSource : class//We are not creating an instance of source, no need to restrict parameterless constructor
            where TDest : class, new()//We are creating an instance of destination, parameterless constructor is needed
        {
            if (source == null)
                return null;

            TDest destination = new TDest();

            var typeOfSource = source.GetType();
            var typeOfDestination = destination.GetType();

            foreach (var fieldOfSource in typeOfSource.GetFields())
            {
                var fieldOfDestination = typeOfDestination.GetField(fieldOfSource.Name);
                if (fieldOfDestination != null)
                {
                    try
                    { fieldOfDestination.SetValue(destination, fieldOfSource.GetValue(source)); }
                    catch (ArgumentException) { }//If datatype is mismatch, skip the mapping
                }
            }

            foreach (var propertyOfSource in typeOfSource.GetProperties())
            {
                var propertyOfDestination = typeOfDestination.GetProperty(propertyOfSource.Name);
                if (propertyOfDestination != null)
                {
                    try
                    { propertyOfDestination.SetValue(destination, propertyOfSource.GetValue(source)); }
                    catch (ArgumentException) { }//If datatype is mismatch, skip the mapping
                }
            }

            return destination;
        }
    }
}
