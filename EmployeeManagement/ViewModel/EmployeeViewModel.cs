using EmployeeManagement.Models;
using System.ComponentModel.DataAnnotations;

namespace EmployeeManagement.ViewModel
{
    public class EmployeeViewModel
    {
        public List<Gender> GenderList { get; set; }
        public List<EmployeeInfo> EmployeeList { get; set; }
    }
}
