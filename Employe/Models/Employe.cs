using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EmployeAPI.Models
{
    public class Employe
    {
        [Key]
        public string Nik { get; set; }
        public string? Nama { get; set; }
        public string? Gender { get; set; }
        public DateTime TglLahir { get; set; }
        public string? Alamat { get; set; }
        public DateTime TglBergabung { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }
    }

    public class EmployeRq
    {
        public string Nik { get; set; }
        public string? Nama { get; set; }
        public string? Gender { get; set; }
        public DateTime TglLahir { get; set; }
        public string? Alamat { get; set; }
        public DateTime TglBergabung { get; set; }
    }
}
