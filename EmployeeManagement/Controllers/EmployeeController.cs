using EmployeeManagement.Models;
using EmployeeManagement.Repository;
using EmployeeManagement.ViewModel;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeManagement.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly IEmployeeRepository _employeeRepository;
        public EmployeeController(IEmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository;
        }
        public async Task<IActionResult> Index()
        {
            var employeeViewModel = await _employeeRepository.SetEmployeeViewModel();
            return View(employeeViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> AddOrUpdateEmployee(EmployeeInfo employee)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var errors = ModelState
                        .Where(x => x.Value.Errors.Count > 0)
                        .ToDictionary(
                            kvp => kvp.Key,
                            kvp => kvp.Value.Errors.Select(e => e.ErrorMessage).ToArray()
                        );

                    return BadRequest(errors);
                }

                bool isAddedOrUpdated = await _employeeRepository.AddOrUpdateEmployee(employee);
                if (isAddedOrUpdated)
                {
                    return Ok(new { success = true });
                }
                else
                {
                    return StatusCode(500);
                }
            }
            catch (Exception ex)
            {
               return StatusCode(500, ex.Message);
            }
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteEmployee(int employeeId)
        {
            try
            {
                if (employeeId == 0)
                { 
                    return BadRequest("Please provide the Employee ID.");
                }

                bool isDeleted = await _employeeRepository.DeleteEmployee(employeeId);

                if (isDeleted)
                {
                    return Ok(new { success = true });
                }
                else
                {
                    return StatusCode(500);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetEmployeesAfterActions()
        {
            try
            {
                var employeeViewModel = await _employeeRepository.SetEmployeeViewModel();
                return PartialView("_EmployeeList", employeeViewModel);
            }
            catch(Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet]
        public async Task<EmployeeInfo> GetEmployeeByID(int employeeId)
        {
            try
            {
                var employee = await _employeeRepository.GetEmployeeByID(employeeId);
                return employee;
            }
            catch(Exception ex)
            {
                return new EmployeeInfo();
            }
        }
    }
}
