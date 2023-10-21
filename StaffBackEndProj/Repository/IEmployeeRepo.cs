using ErrorOr;
using StaffBackEndProj.Model;
using StaffBackEndProj.Model.CreateRequest;

namespace StaffBackEndProj.Repository
{
    public interface IEmployeeRepo
    {

       // Task<List<EmployeeRequest>> GetAll();

        Task<ErrorOr<EmployeeRequest>> GetbyId(int id);
        Task<ErrorOr<List<EmployeeRequest>>> GetbyCompanyId(int companyid);
        Task<ErrorOr<List<EmployeeRequest>>> GetbyCompanyIdAndDepartment(int companyid, string departmentname);
        Task<ErrorOr<EmployeeRequest>> Create(EmployeeRequest employee);
        Task<ErrorOr<EmployeeRequest>> Update(UpdateEmployeeRequest employee, int id);
        Task<ErrorOr<EmployeeRequest>> Update(UpdateEmployeeRequest employee);

        Task<string> Remove(int id);


    }
}
