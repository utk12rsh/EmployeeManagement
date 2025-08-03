$(document).ready(function () {

    if ($('#employeeDataTable').length) {
        initializeDataTable();
    }

    // ✅ Add / Update Employee Handler (Combined)
    $(document).on("click", '#submit, #update', function (e) {
        e.preventDefault();

        var isUpdate = $(this).attr('id') === 'update';
        var empId = $("#EmployeeID").val();
        var $btn = $(this); 

        var employeeData = {
            EmployeeID: isUpdate ? empId : 0,
            EmployeeName: $("#Name").val(),
            GenderID: $("#GenderID").val()
        };

        $(".text-danger").text("");
        $("#status-message").html("").show();
        $btn.prop("disabled", true); // ✅ Dynamic disable

        $.ajax({
            type: "POST",
            url: "/Employee/AddOrUpdateEmployee",
            data: employeeData,
            success: function (response) {
                $("#status-message").html(`
                <div class="alert alert-success">
                    <strong>Success:</strong> Employee ${isUpdate ? 'updated' : 'added'} successfully!
                </div>
            `);

                if (isUpdate) {
                    $btn.removeAttr('id');
                    $('input[value="UPDATE"]').attr('id', 'submit').val('SUBMIT');
                }

                hideStatusMessageAfterDelay();
                resetFormFields();
                GetUpdatedEmployeesAfterActions();
                $btn.prop("disabled", false); // ✅ Re-enable
            },
            error: function (xhr) {
                if (xhr.status === 400 && xhr.responseJSON) {
                    let errors = xhr.responseJSON;
                    for (let field in errors) {
                        let message = errors[field].join(", ");
                        $(`#error-${field}`).text(message);
                    }
                } else {
                    $("#status-message").html(`
                    <div class="alert alert-danger">
                        <strong>Error:</strong> Employee could not be ${isUpdate ? 'updated' : 'added'}!
                    </div>
                `);
                    hideStatusMessageAfterDelay();
                }

                $btn.prop("disabled", false); // ✅ Always re-enable
            }
        });
    });

    // ✅ Edit Employee (load data to form)
    $(document).on('click', '.edit-btn', function () {

        var empId = $(this).data('id');
        var $btn = $(this);

        $.ajax({
            type: "GET",
            url: "/Employee/GetEmployeeByID",
            data: { employeeId: empId },
            success: function (response) {
                $('#EmployeeID').val(response.employeeID);
                $('#Name').val(response.employeeName);
                $('#GenderID').val(response.genderID);

                $btn.prop("disabled", false); 
                $('#submit').removeAttr('id');
                $('input[value="SUBMIT"]').attr('id', 'update').val('UPDATE');
            },
            error: function () {
                alert("Error fetching employee data.");
            }
        });
    });

    // ✅ Delete Employee with SweetAlert Confirmation
    $(document).on('click', '.delete-btn', function (e) {
        e.preventDefault();

        var empId = $(this).data('id');

        Swal.fire({
            title: 'Are you sure?',
            text: "This action will permanently delete the employee.",
            icon: 'warning',
            showCancelButton: true,
            confirmButtonColor: '#d33',
            cancelButtonColor: '#3085d6',
            confirmButtonText: 'Yes, delete it!',
            cancelButtonText: 'Cancel'
        }).then((result) => {
            if (result.isConfirmed) {
                $.ajax({
                    type: "DELETE",
                    url: "/Employee/DeleteEmployee",
                    data: { employeeId: empId },
                    success: function () {
                        Swal.fire(
                            'Deleted!',
                            'Employee has been deleted.',
                            'success'
                        );

                        hideStatusMessageAfterDelay();
                        resetFormFields();
                        GetUpdatedEmployeesAfterActions();
                    },
                    error: function () {
                        Swal.fire(
                            'Error!',
                            'Employee could not be deleted!',
                            'error'
                        );
                    }
                });
            }
        });
    });

    // ✅ Fetch updated employee list and reinitialize DataTable
    function GetUpdatedEmployeesAfterActions() {
        $.ajax({
            url: '/Employee/GetEmployeesAfterActions',
            type: 'GET',
            success: function (partialViewHtml) {
                $("#show-employee-list").html(partialViewHtml);

                if ($.fn.DataTable.isDataTable('#employeeDataTable')) {
                    $('#employeeDataTable').DataTable().destroy();
                }

                initializeDataTable();
            },
            error: function () {
                $("#status-message").html(`
                    <div class="alert alert-danger">
                        <strong>Error:</strong> Could not refresh employee list.
                    </div>
                `);
                hideStatusMessageAfterDelay();
            }
        });
    }

    // ✅ Fade out status message after delay
    function hideStatusMessageAfterDelay(delay = 5000) {
        setTimeout(function () {
            $("#status-message").fadeOut("slow", function () {
                $(this).html("").show();
            });
        }, delay);
    }

    // ✅ Reset all form fields
    function resetFormFields() {
        $("#Name").val("");
        $("#GenderID").val("");
        $("#EmployeeID").val("");
    }

    // ✅ Initialize DataTable with settings
    function initializeDataTable() {
        $('#employeeDataTable').DataTable({
            searching: true,
            ordering: true,
            info: true,
            paging: true,
            pageLength: 5,
            lengthMenu: [5, 10, 15, 20],
            pagingType: 'full',
            language: {
                emptyTable: "No Employee Found."
            },
            order: [[0, 'desc']]
        });
    }
});
