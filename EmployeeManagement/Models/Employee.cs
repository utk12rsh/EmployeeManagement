using System;
using System.Collections.Generic;

namespace EmployeeManagement.Models;

public partial class Employee
{
    public int EmployeeId { get; set; }

    public string EmployeeName { get; set; } = null!;

    public int GenderId { get; set; }

    public virtual Gender Gender { get; set; } = null!;
}
