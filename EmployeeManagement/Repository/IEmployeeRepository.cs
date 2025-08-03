using EmployeeManagement.Models;
using EmployeeManagement.ViewModel;
using Gender = EmployeeManagement.Models.Gender;

namespace EmployeeManagement.Repository
{
    public interface IEmployeeRepository
    {
        Task<List<Gender>> GetGenders();
        Task<EmployeeViewModel> SetEmployeeViewModel();
        Task<bool> AddOrUpdateEmployee(EmployeeInfo model);
        Task<EmployeeInfo> GetEmployeeByID(int employeeId);
        Task<bool> DeleteEmployee(int employeeId);
    }

}
