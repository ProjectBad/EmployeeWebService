using Dapper;
using StaffBackEndProj.Model.Data;
using System.Data;
using StaffBackEndProj.Model.CreateRequest;
using StaffBackEndProj.Model;
using Microsoft.AspNetCore.Mvc;
using ErrorOr;
using System.Collections.Generic;

namespace StaffBackEndProj.Repository
{
    public class EmployeeRepo : IEmployeeRepo
    {
        private readonly DapperDBContext context;
        public EmployeeRepo(DapperDBContext context)
        {
            this.context = context;
        }

        public async Task<ErrorOr<EmployeeRequest>> Create(EmployeeRequest employee)
        {
            
            string query = "INSERT INTO passport(passport.type, passport.number) VALUES (@passtype, @passnum); " +
                "SET @passid = (SELECT LAST_INSERT_ID()); " +
                "INSERT INTO department (department.name, department.phone) VALUES ( @depname, @depphone); " +
                "SET @depid = (SELECT LAST_INSERT_ID()); " +
                "INSERT INTO employee (name, surname, phone, companyid, passportid, departmentid) " +
                    "VALUES (@name, @surname, @phone, @companyid, @passid, @depid); " +
                "SELECT LAST_INSERT_ID()";
            var parameters = new DynamicParameters();
            parameters.Add("passtype", employee.passport.Type, DbType.String);
            parameters.Add("passnum", employee.passport.Number, DbType.String);

            parameters.Add("depname", employee.department.Name, DbType.String);
            parameters.Add("depphone", employee.department.Phone, DbType.String);

            parameters.Add("name", employee.name, DbType.String);
            parameters.Add("surname", employee.surname, DbType.String);
            parameters.Add("phone", employee.phone, DbType.String);
            parameters.Add("companyid", employee.companyid, DbType.Int32);
            using (var connectin = this.context.CreateConnection())
            {
                var response = await connectin.QueryAsync<int>(query, parameters);
                return await GetbyId(response.Single());
            }
            
        }

        public async Task<ErrorOr<List<EmployeeRequest>>> GetbyCompanyId(int companyid)
        {
            string query = "SELECT `employee`.*, `passport`.`type`, `passport`.`number`, `department`.`name`, `department`.`phone` FROM `employee` LEFT JOIN `department` ON `employee`.`departmentid` = `department`.`id` LEFT JOIN `passport` ON `employee`.`passportid` = `passport`.`id` WHERE `employee`.`companyid` = ";
            using (var connectin = this.context.CreateConnection())
            {
                var emplist = await connectin.QueryAsync<EmployeeRequest, PassportRequest,
                    DepartmentRequest, EmployeeRequest>(query + companyid, (employee, passport, department) =>
                                                                                    {
                                                                                        employee.passport = passport;
                                                                                        employee.department = department;
                                                                                        return employee;
                                                                                    }, splitOn: "type,name");
                return emplist.ToList();
            }
        }

        public async Task<ErrorOr<List<EmployeeRequest>>> GetbyCompanyIdAndDepartment(int companyid, string departmentname)
        {
            string query = "SELECT `employee`.*, `passport`.`type`, `passport`.`number`, `department`.`name`, `department`.`phone` FROM `employee`" +
                " LEFT JOIN `department` ON `employee`.`departmentid` = `department`.`id` " +
                "LEFT JOIN `passport` ON `employee`.`passportid` = `passport`.`id` " +
                "WHERE `employee`.`companyid` = @cid AND `department`.`name` = @dname;";
            var parameters = new DynamicParameters();

            parameters.Add("cid", companyid, DbType.Int32);
            parameters.Add("dname", departmentname, DbType.String);


            using (var connectin = this.context.CreateConnection())
            {
                var emplist = await connectin.QueryAsync<EmployeeRequest, PassportRequest,
                    DepartmentRequest, EmployeeRequest>(query, (employee, passport, department) =>
                    {
                        employee.passport = passport;
                        employee.department = department;
                        return employee;
                    }, parameters, splitOn: "type,name");
                return emplist.ToList();
            }
        }

        //public async Task<List<EmployeeRequest>> GetAll()
        //{
        //    string query = "Select * From tbl_employee";
        //    using (var connectin = this.context.CreateConnection())
        //    {
        //        var emplist = await connectin.QueryAsync<EmployeeRequest>(query);
        //        return emplist.ToList();
        //    }
        //}

        //public Task<List<EmployeeRequest>> GetAllbyCompanyId(int companyId)
        //{
        //    throw new NotImplementedException();
        //}

        public async Task<ErrorOr<EmployeeRequest>> GetbyId(int id)
        {
            string query = "SELECT `employee`.*, `passport`.`type`, `passport`.`number`, `department`.`name`, `department`.`phone` " +
                "FROM `employee` " +
                "LEFT JOIN `department` ON `employee`.`departmentid` = `department`.`id` " +
                "LEFT JOIN `passport` ON `employee`.`passportid` = `passport`.`id` " +
                "WHERE `employee`.`id` = ";
            using (var connectin = this.context.CreateConnection())
            {
                IEnumerable<EmployeeRequest> emplist = await connectin.QueryAsync<EmployeeRequest, PassportRequest, 
                    DepartmentRequest, EmployeeRequest>(query + id, (employee, passport, department) =>
                                                                                                        { employee.passport = passport;
                                                                                                           employee.department = department;
                                                                                                            return employee;
                                                                                                        }, splitOn: "type,name");
                if (emplist.Count() == 0)
                {
                    return Error.NotFound(description: string.Format("Сотрудника с id = {0} не существует.", id));
                }

                    return emplist.Single();
            }
        }

        public async Task<string> Remove(int id)
        {
            string response = string.Empty;
            string query = "SET @pid = (SELECT `passportid` FROM `employee` WHERE `employee`.`id` = @id); " +
                "SET @did = (SELECT `departmentid` FROM `employee` WHERE `employee`.`id` = @id); " +
                "DELETE FROM `passport` WHERE `passport`.`id` = @pid; " +
                "DELETE FROM `department` WHERE `department`.`id` = @did; " +
                "DELETE FROM `employee` WHERE `employee`.`id` = @id";
            using (var connectin = this.context.CreateConnection())
            {
                await connectin.ExecuteAsync(query, new { id });
                response = "Good";
            }
            return response;
        }

        public async Task<ErrorOr<EmployeeRequest>> Update(UpdateEmployeeRequest employee, int id)
        {
            if ((employee.id is not null) && (id != 0) && (id != employee.id) )
            {
                return Error.Validation(description: "Введенные ID не совпадают");
            }
            else
            {
                if((employee.id is not null) && id == 0)
                {
                    id = (int)employee.id;
                }
                
            }
            string query = "";
            var parameters = new DynamicParameters();
            if ((employee.name is not null) || (employee.surname is not null) || (employee.phone is not null) || (employee.companyid is not null))
            {
                
                query += "UPDATE `employee` SET ";
                query += employee.name is not null ? "`name` = @name, " : "";
                query += employee.surname is not null ? "`surname` = @surname, " : "";
                query += employee.phone is not null ? "`phone` = @phone, " : "";
                query += employee.companyid is not null ? "`companyid` = @companyid, " : "";
                query = query.Remove(query.Length - 2);
                query += " WHERE `employee`.`id` = @id;";

                if (employee.name is not null) parameters.Add("name", employee.name, DbType.String);
                if (employee.surname is not null) parameters.Add("surname", employee.surname, DbType.String);
                if (employee.phone is not null) parameters.Add("phone", employee.phone, DbType.String);
                if (employee.companyid is not null) parameters.Add("companyid", employee.companyid, DbType.Int32);

                if (employee.passport is not null)
                {
                    query += " UPDATE `passport` SET ";
                    query += employee.passport.Type is not null ? "`type` = @ptype, " : "";
                    query += employee.passport.Number is not null ? "`number` = @pnumber, " : "";
                    query = query.Remove(query.Length - 2);
                    query += " WHERE `passport`.`id` = (SELECT `employee`.`passportid` FROM `employee` WHERE `employee`.`id` = @id);";

                    if (employee.passport.Type is not null) parameters.Add("ptype", employee.passport.Type, DbType.String);
                    if (employee.passport.Number is not null) parameters.Add("pnumber", employee.passport.Number, DbType.String);

                }

                if (employee.department is not null)
                {
                    query += " UPDATE `department` SET ";
                    query += employee.department.Name is not null ? "`department`.`name` = @dname, " : "";
                    query += employee.department.Phone is not null ? "`department`.`phone` = @dphone, " : "";
                    query = query.Remove(query.Length - 2);
                    query += " WHERE `department`.`id` = (SELECT `employee`.`departmentid` FROM `employee` WHERE `employee`.`id` = @id)";

                    if (employee.department.Name is not null) parameters.Add("dname", employee.department.Name, DbType.String);
                    if (employee.department.Phone is not null) parameters.Add("dphone", employee.department.Phone, DbType.String);
                }
                parameters.Add("id", id, DbType.Int32);

            }

            using (var connectin = this.context.CreateConnection())
            {
                var response = await connectin.ExecuteAsync(query, parameters);
                return await GetbyId(id);
            }

            
        }

        //public async Task<ErrorOr<EmployeeRequest>> Update(UpdateEmployeeRequest employee)
        //{
        //    string query = "";
        //    var parameters = new DynamicParameters();
        //    if ((employee.name is not null) || (employee.surname is not null) || (employee.phone is not null) || (employee.companyid is not null))
        //    {

        //        query += "UPDATE `employee` SET ";
        //        query += employee.name is not null ? "`name` = @name, " : "";
        //        query += employee.surname is not null ? "`surname` = @surname, " : "";
        //        query += employee.phone is not null ? "`phone` = @phone, " : "";
        //        query += employee.companyid is not null ? "`companyid` = @companyid, " : "";
        //        query = query.Remove(query.Length - 2);
        //        query += " WHERE `employee`.`id` = @id;";

        //        if (employee.name is not null) parameters.Add("name", employee.name, DbType.String);
        //        if (employee.surname is not null) parameters.Add("surname", employee.surname, DbType.String);
        //        if (employee.phone is not null) parameters.Add("phone", employee.phone, DbType.String);
        //        if (employee.companyid is not null) parameters.Add("companyid", employee.companyid, DbType.Int32);

        //        if (employee.passport is not null)
        //        {
        //            query += " UPDATE `passport` SET ";
        //            query += employee.passport.Type is not null ? "`type` = @ptype, " : "";
        //            query += employee.passport.Number is not null ? "`number` = @pnumber, " : "";
        //            query = query.Remove(query.Length - 2);
        //            query += " WHERE `passport`.`id` = (SELECT `employee`.`passportid` FROM `employee` WHERE `employee`.`id` = @id);";

        //            if (employee.passport.Type is not null) parameters.Add("ptype", employee.passport.Type, DbType.String);
        //            if (employee.passport.Number is not null) parameters.Add("pnumber", employee.passport.Number, DbType.String);

        //        }

        //        if (employee.department is not null)
        //        {
        //            query += " UPDATE `department` SET ";
        //            query += employee.department.Name is not null ? "`department`.`name` = @dname, " : "";
        //            query += employee.department.Phone is not null ? "`department`.`phone` = @dphone, " : "";
        //            query = query.Remove(query.Length - 2);
        //            query += " WHERE `department`.`id` = (SELECT `employee`.`departmentid` FROM `employee` WHERE `employee`.`id` = @id)";

        //            if (employee.department.Name is not null) parameters.Add("dname", employee.department.Name, DbType.String);
        //            if (employee.department.Phone is not null) parameters.Add("dphone", employee.department.Phone, DbType.String);
        //        }
        //        parameters.Add("id", employee.id, DbType.Int32);

        //    }

        //    using (var connectin = this.context.CreateConnection())
        //    {
        //        var response = await connectin.ExecuteAsync(query, parameters);
        //        return await GetbyId(employee.id);
        //    }


        //}
    }
}
