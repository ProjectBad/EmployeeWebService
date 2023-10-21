using System.ComponentModel.DataAnnotations;

namespace StaffBackEndProj.Model.CreateRequest
{
    public class EmployeeRequest
    {
        public int id { get; set; }
        [StringLength(50, ErrorMessage = "Имя не должно быть длиннее 50 символов.")]
        public string name { get; set; }
        [StringLength(50, ErrorMessage = "Фамилия не должно быть длиннее 50 символов.")]
        public string surname { get; set; }
        [StringLength(12, ErrorMessage = "Номер телефона не должен быть длиннее 12 символов.")]
        public string phone { get; set; }
        [Range(1, Int32.MaxValue, ErrorMessage = "Значение параметра companyid должно быть больше 0.")]
        public int companyid { get; set; }
        public PassportRequest passport { get; set; }

        public DepartmentRequest department { get; set; }
    }
}
