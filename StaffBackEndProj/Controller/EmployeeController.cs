using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using StaffBackEndProj.Model;
using StaffBackEndProj.Model.CreateRequest;
using StaffBackEndProj.Repository;
using System.ComponentModel.DataAnnotations;

namespace StaffBackEndProj.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployeeRepo repo;
        public EmployeeController(IEmployeeRepo repo)
        {
            this.repo = repo;

        }

        [HttpGet("GetAllByCompanyId")]
        public async Task<IActionResult> GetAllByCompanyId(int companyid)
        {
            var _list = await this.repo.GetbyCompanyId(companyid);
            if (_list.Value != null)
            {
                return Ok(_list.Value);
            }
            else
            {
                return NotFound();
            }
        }

        [HttpGet("GetAllByCompanyAndDepartment")]
        public async Task<IActionResult> GetAllByCompanyAndDepartment(int companyid, string departmentname)
        {
            var _list = await this.repo.GetbyCompanyIdAndDepartment(companyid, departmentname);
            if (_list.Value != null)
            {
                return Ok(_list.Value);
            }
            else
            {
                return NotFound();
            }
        }

        [HttpGet("GetbyId/{id}")]
        public async Task<IActionResult> GetbyCode(int id)
        {
            var _list = await this.repo.GetbyId(id);

            if (_list.IsError)
            {
                return NotFound();
            }
            if (_list.Value != null)
            {
                return Ok(_list.Value);
            }
            else
            {
                return NotFound();
            }
        }

        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromBody] EmployeeRequest employee)
        {
            var _result = await this.repo.Create(employee);
            if (_result.IsError)
            {
                return NotFound();
            }
            return Ok(_result.Value);
        }


        [HttpPut("Update")]
        public async Task<IActionResult> Update([FromBody] UpdateEmployeeRequest employee, int id)
        {

            var _result = await this.repo.Update(employee, id);
            if (_result.IsError)
            {
                return Problem(statusCode: 400, title: _result.FirstError.Description);
            }
            return Ok(_result.Value);
        }

        //[HttpPut("Update")]
        //public async Task<IActionResult> Update([FromBody] UpdateEmployeeRequest employee)
        //{

        //    var _result = await this.repo.Update(employee);
        //    if (_result.IsError)
        //    {
        //        return Problem(statusCode: 400, title: _result.FirstError.Description);
        //    }
        //    return Ok(_result.Value);
        //}

        //[HttpPut("Update")]
        //public async Task<IActionResult> Update([FromBody] UpdateEmployeeRequest employee, int id = 0)
        //{ 

        //    var _result = (id != 0 ? await this.repo.Update(employee, id) : await this.repo.Update(employee));
        //    if (_result.IsError)
        //    {
        //        return Problem(statusCode: 400, title: _result.FirstError.Description);
        //    }
        //    return Ok(_result.Value);
        //}


        [HttpDelete("Remove")]
        public async Task<IActionResult> Remove(int id)
        {
            var _result = await this.repo.Remove(id);
            return Ok();
        }
    }
}
