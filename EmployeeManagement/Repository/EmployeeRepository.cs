using EmployeeManagement.Models;
using EmployeeManagement.ViewModel;
using Microsoft.EntityFrameworkCore;
using Gender = EmployeeManagement.Models.Gender;

namespace EmployeeManagement.Repository
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly EmployeeDbContext _context;

        public EmployeeRepository(EmployeeDbContext context)
        {
            _context = context;
        }

        public async Task<List<Gender>> GetGenders()
        {
            try
            {
                var genders = await (from g in _context.Genders
                                     select g).ToListAsync();

                if (genders == null)
                {
                    return new List<Gender>();
                }

                return genders;
            }
            catch (Exception ex)
            {
                return new List<Gender>();
            }
        }

        public async Task<List<Employee>> GetEmployees()
        {
            try
            {
                var employees = await (from e in _context.Employees
                                       select e).ToListAsync();

                if (employees == null)
                {
                    return new List<Employee>();
                }

                return employees;
            }
            catch (Exception ex)
            {
                return new List<Employee>();
            }
        }

        public async Task<EmployeeViewModel> SetEmployeeViewModel()
        {
            try
            {
                var genders = await GetGenders();
                var employees = await GetEmployees();

                var genderViewModels = genders.Select(g => new EmployeeManagement.ViewModel.Gender
                {
                    GenderID = g.GenderId,
                    GenderName = g.GenderName
                }).ToList();

                var employeeInfoViewModel = employees.Select(e => new EmployeeManagement.ViewModel.EmployeeInfo
                {
                    EmployeeID = e.EmployeeId,
                    EmployeeName = e.EmployeeName,
                    GenderID = e.GenderId
                }).ToList();

                var employeeViewModel = new EmployeeViewModel
                {
                    GenderList = genderViewModels,
                    EmployeeList = employeeInfoViewModel
                };

                return employeeViewModel;
            }
            catch
            {
                return new EmployeeViewModel();
            }
        }

        public async Task<bool> AddOrUpdateEmployee(EmployeeInfo model)
        {
            try
            {
                if (model.EmployeeID == 0)
                {
                    Employee employee = new Employee
                    {
                        EmployeeName = model.EmployeeName,
                        GenderId = model.GenderID
                    };
                    // Add using LINQ (method syntax)
                    await _context.Employees.AddAsync(employee);
                    await _context.SaveChangesAsync();
                    return true;
                }
                else
                {
                    var employee = await (from e in _context.Employees
                                          where e.EmployeeId == model.EmployeeID
                                          select e).FirstOrDefaultAsync();

                    if (employee == null)
                    {
                        return false; // Employee not found
                    }

                    // Update employee properties
                    employee.EmployeeName = model.EmployeeName;
                    employee.GenderId = model.GenderID;

                    await _context.SaveChangesAsync(); // Save changes to the database

                    return true;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> DeleteEmployee(int employeeId)
        {
            try
            {
                var employee = await (from e in _context.Employees
                                      where e.EmployeeId == employeeId
                                      select e).FirstOrDefaultAsync();

                if (employee == null)
                {
                    return false;
                }

                _context.Employees.Remove(employee);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<EmployeeInfo> GetEmployeeByID(int employeeId)
        {
            try
            {
                var employee = await (from e in _context.Employees
                                      where e.EmployeeId == employeeId
                                      select e).FirstOrDefaultAsync();

                if (employee == null)
                {
                    return new EmployeeInfo();
                }
                else
                {
                    EmployeeInfo employeeData = new EmployeeInfo
                    {
                        EmployeeID = employee.EmployeeId,
                        EmployeeName = employee.EmployeeName,
                        GenderID = employee.GenderId
                    };

                    return employeeData;
                }
            }
            catch (Exception ex)
            {
                return new EmployeeInfo();
            }
        }
    }
}
