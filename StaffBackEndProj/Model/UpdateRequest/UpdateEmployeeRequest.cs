using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StaffBackEndProj.Model
{
    public class UpdateEmployeeRequest
    {
        [Required]
        [Range(1, Int32.MaxValue, ErrorMessage = "Значение параметра id должно быть больше 0.")]
        public int id { get; set; }
        [StringLength(50, ErrorMessage = "Имя не должно быть длиннее 50 символов.")]
        public string? name { get; set; }
        [StringLength(50, ErrorMessage = "Фамилия не должно быть длиннее 50 символов.")]
        public string? surname { get; set; }
        [StringLength(12, ErrorMessage = "Номер телефона не должен быть длиннее 12 символов.")]
        public string? phone { get; set; }
        [Range(1, Int32.MaxValue, ErrorMessage = "Значение параметра companyid должно быть больше 0.")]
        public int? companyid { get; set; }
        public UpdatePassportRequest? passport { get; set; }

        public UpdateDepartmentRequest? department { get; set; }
    }
}
