using System.ComponentModel.DataAnnotations;

namespace EmployeeManagement.ViewModel
{
    public class EmployeeInfo
    {
        public int EmployeeID { get; set; }

        [Required(ErrorMessage = "Name is required.")]
        [RegularExpression(@"^[A-Za-z\s]+$", ErrorMessage = "Only letters are allowed in Name.")]
        public string EmployeeName { get; set; }

        [Required(ErrorMessage = "Gender is required.")]
        public int GenderID { get; set; }
    }
}
