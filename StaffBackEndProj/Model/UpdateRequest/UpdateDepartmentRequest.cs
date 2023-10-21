using System.ComponentModel.DataAnnotations;

namespace StaffBackEndProj.Model
{
    public class UpdateDepartmentRequest
    {
        [StringLength(50, ErrorMessage = "Название не должно быть длиннее 50 символов.")]
        public string? Name { get; set; }
        [StringLength(6, ErrorMessage = "Добавочный не должен быть длиннее 6 символов.")]
        public string? Phone { get; set; }


    }
}
