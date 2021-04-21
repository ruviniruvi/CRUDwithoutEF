using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CRUDwithoutEF.Data;
using CRUDwithoutEF.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Data.SqlClient;
using System.Data;

namespace CRUDwithoutEF.Controllers
{
    public class EmployeesController : Controller
    {
        private readonly IConfiguration _configuration;

        //  private readonly CRUDwithoutEFContext _context;

        public EmployeesController(IConfiguration configuration)
        {
            this._configuration = configuration;
        }

        // GET: Employees
        public IActionResult Index()
        {
            DataTable dataTable = new DataTable();
            using (SqlConnection sqlConnection = new SqlConnection(_configuration.GetConnectionString("DevConnection")))
            {
                sqlConnection.Open();
                SqlDataAdapter sqlDataAd = new SqlDataAdapter("EMPLOYEEViewAll", sqlConnection);
                sqlDataAd.SelectCommand.CommandType = System.Data.CommandType.StoredProcedure;
                sqlDataAd.Fill(dataTable);
                
              
            }



            return View(dataTable);
        }



        // GET: Employees/Create
        public IActionResult Create()
        {
            return View();
        }



        // GET: Employees/Edit/
        public IActionResult Edit(int? id)
        {
            Employee employee = new Employee();
            if (id > 0)
                employee = FetchEmployeeByID(id);
            return View(employee);
        }

        // POST: Employees/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, [Bind("EMPLOYEE_ID,EMPLOYEE_FIRST_NAME,EMPLOYEE_LAST_NAME,AGE,POSITION,SALARY")] Employee employee)
        {


            if (ModelState.IsValid)
            {

                using (SqlConnection sqlConnection = new SqlConnection(_configuration.GetConnectionString("DevConnection")))
                {
                    sqlConnection.Open();
                    SqlCommand sqlCmd = new SqlCommand("EMPLOYEEEdit", sqlConnection);
                    sqlCmd.CommandType = System.Data.CommandType.StoredProcedure;
                    sqlCmd.Parameters.AddWithValue("EMPLOYEE_ID", employee.EMPLOYEE_ID);
                    sqlCmd.Parameters.AddWithValue("EMPLOYEE_FIRST_NAME", employee.EMPLOYEE_FIRST_NAME);
                    sqlCmd.Parameters.AddWithValue("EMPLOYEE_LAST_NAME", employee.EMPLOYEE_LAST_NAME);
                    sqlCmd.Parameters.AddWithValue("AGE", employee.AGE);
                    sqlCmd.Parameters.AddWithValue("POSITION", employee.POSITION);
                    sqlCmd.Parameters.AddWithValue("SALARY", employee.SALARY);
                    sqlCmd.ExecuteNonQuery();
                }


                return RedirectToAction(nameof(Index));
            }
            return View(employee);
        }

        // GET: Employees/Delete/5
        public IActionResult Delete(int? id)
        {


            Employee employee = FetchEmployeeByID(id);
           
            return View(employee);
        }

        // POST: Employees/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {

            using (SqlConnection sqlConnection = new SqlConnection(_configuration.GetConnectionString("DevConnection")))
            {
                sqlConnection.Open();
                SqlCommand sqlCmd = new SqlCommand("EMPLOYEEDeleteByID", sqlConnection);
                sqlCmd.CommandType = System.Data.CommandType.StoredProcedure;
                sqlCmd.Parameters.AddWithValue("EMPLOYEE_ID", id);
               
                sqlCmd.ExecuteNonQuery();
            }


            return RedirectToAction(nameof(Index));
        }
        [NonAction]
        public Employee FetchEmployeeByID(int? id)
        {
            Employee employee = new Employee();

            DataTable dataTable = new DataTable();
            using (SqlConnection sqlConnection = new SqlConnection(_configuration.GetConnectionString("DevConnection")))
            {
                sqlConnection.Open();
                SqlDataAdapter sqlDataAd = new SqlDataAdapter("EMPLOYEEViewByID", sqlConnection);

                sqlDataAd.SelectCommand.CommandType = System.Data.CommandType.StoredProcedure;
                sqlDataAd.SelectCommand.Parameters.AddWithValue("EMPLOYEE_ID",id) ;
                sqlDataAd.Fill(dataTable);

                if(dataTable.Rows.Count == 1)
                {
                    employee.EMPLOYEE_ID = Convert.ToInt32(dataTable.Rows[0]["EMPLOYEE_ID"].ToString());
                    employee.EMPLOYEE_FIRST_NAME = dataTable.Rows[0]["EMPLOYEE_FIRST_NAME"].ToString();
                    employee.EMPLOYEE_LAST_NAME = dataTable.Rows[0]["EMPLOYEE_LAST_NAME"].ToString();
                    employee.AGE = Convert.ToInt32(dataTable.Rows[0]["AGE"].ToString());
                    employee.POSITION = dataTable.Rows[0]["POSITION"].ToString();
                    employee.SALARY = (int)Convert.ToDecimal(dataTable.Rows[0]["SALARY"].ToString());
                }
                return employee;
            }
        }
    }
}
