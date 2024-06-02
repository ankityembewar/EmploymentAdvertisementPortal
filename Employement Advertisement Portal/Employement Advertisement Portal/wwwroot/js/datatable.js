$(document).ready(function () {
    GetEmployeeList();

    $(document).ready(function () {
        $('#showChartBtn').click(function () {
            $('#chartDropdown').toggle(); // Use toggle to show/hide the dropdown
        });
    });

    // Listen for changes in the dropdown menu
    //$('#chartType').on('change', function () {
    //    var selectedChartType = $(this).val(); // Get the selected chart type
    //    GetEmployeeList(selectedChartType); // Pass selectedChartType to GetEmployeeList
    //});

    // Function to close dropdown menu when clicking outside or clicking dropdown button again
    $(document).on('click', function (event) {
        // Check if the clicked element is inside the dropdown menu or the dropdown button
        if (!$(event.target).closest('#columnDropdownMenu').length && !$(event.target).closest('#columnDropdown').length) {
            // If the clicked element is outside the dropdown menu and the dropdown button, close the dropdown menu
            $('#columnDropdownMenu').hide();
        }
    });

    $('#columnDropdown').click(function () {
        toggleDropdownAndGenerateItems(); // Call the toggleDropdownAndGenerateItems function when the button is clicked
    });

    // Attach the handleCheckboxChange function to the change event of .columnCheckbox elements
    $(document).on('click', '.columnCheckbox', handleCheckboxChange);
});
function GetEmployeeList(selectedChartType) {
    $.ajax({
        url: '/Admin/GetEmployeeList',
        type: 'Get',
        dataType: 'json',
        success: function (response) {
            OnSuccess(response);
            /*renderChart(response, selectedChartType);*/
        }

    })
}
function OnSuccess(response) {
    if ($.fn.DataTable.isDataTable('#employeeTable')) {
        $('#employeeTable').DataTable().destroy();
    }
    $('#employeeTable').DataTable({
        bProcessing: true,
        bLengthChange: true,
        lengthMenu: [[10, 25, 50, 100], [10, 25, 50, 100]],
        bFilter: true,
        bSort: true,
        bPaginate: true,
        "columnDefs": [
            { "className": "dt-center", "targets": "_all" } // Center-align all columns
        ],
        data: response,
        columns: [
            {
                data: "EmpId",
                render: function (data, type, row, meta) {
                    return row.empId
                }
            },
            {
                data: "FirstName",
                render: function (data, type, row, meta) {
                    return row.firstName
                }
            },
            {
                data: "LastName",
                render: function (data, type, row, meta) {
                    return row.lastName
                }
            },
            {
                data: "Email",
                render: function (data, type, row, meta) {
                    return row.email
                }
            },
            {
                data: "Gender",
                render: function (data, type, row, meta) {
                    if (row.gender === 0) {
                        return "MALE";
                    }
                    else {
                        return "FEMALE";
                    }
                }
            },
            {
                data: "Dob",
                title: "Birth Date",
                render: function (data, type, row, meta) {
                    var dob = new Date(row.dob);
                    // Format the date as a string without the time
                    var formattedDate = dob.toLocaleDateString();
                    return formattedDate;
                }
            },
            {
                data: "EmployeeRole",
                title: "Role",
                render: function (data, type, row, meta) {
                    if (row.employeeRole && row.employeeRole.length > 0) {
                        return row.employeeRole[0].text;
                    } else {
                        return "Not Assign"; // Handle the case where employeeRole is empty or undefined
                    }
                }
            },
            {
                data: null,
                title: "Actions",
                render: function (data, type, row, meta) {
                    return '<a href="#" onclick="editEmployee(' + row.empId + ')"><i class="fa-solid fa-user-pen" style="color: #05a31f;"></i></a>&nbsp;&nbsp;' +
                        '<a href="#" onclick="deleteEmployee(' + row.empId + ')"><i class="fa-solid fa-trash" style="color: #f00540;"></i></a>';
                }
            }
        ],
        layout: {
            topStart: {
                buttons: [
                    {
                        extend: 'copy',
                        text: '<i class="fa-solid fa-copy"></i>',
                        exportOptions: {
                            columns: ':visible:not(.no-export)' // Export only visible columns
                        },
                        titleAttr: 'Copy'
                    },
                    {
                        extend: 'excel',
                        text: '<i class="fa-solid fa-file-excel"></i>',
                        exportOptions: {
                            columns: ':visible:not(.no-export)' // Export only visible columns
                        },
                        titleAttr: 'Excel'
                    },
                    {
                        extend: 'csv',
                        text: '<i class="fa-solid fa-file-csv"></i>',
                        exportOptions: {
                            columns: ':visible:not(.no-export)' // Export only visible columns
                        },
                        titleAttr: 'CSV'
                    },
                    {
                        extend: 'pdf',
                        text: '<i class="fa-solid fa-file-pdf"></i>',
                        exportOptions: {
                            columns: ':visible:not(.no-export)' // Export only visible columns
                        },
                        titleAttr: 'PDF'
                    },
                    {
                        extend: 'pageLength',
                        text:'<i class="fa-solid fa-pager" style="color: #e290f9;"></i>',
                        titleAttr: 'Entries per page'
                    }
                ]
            }
        }

    });
}
function handleCheckboxChange() {
    var columnIndex = $(this).data('column');
    var column = $('#employeeTable').DataTable().column(columnIndex);

    var allUnchecked = $('.columnCheckbox').filter(':checked').length === 0;
   
    if (allUnchecked) {
        showNotification('Please ensure at least one column is visible.',"danger"); // Display alert message
    } else {
        // Toggle the visibility of the column based on checkbox state
        column.visible($(this).is(':checked'));
    }
}
function showNotification(message, type = 'info') {
    // Create jQuery alert element with the specified message and type
    var alertMessage = $('<div class="alert alert-sm text-center" role="alert"></div>').addClass('alert-' + type).text(message);

    // Prepend the alert message to the header
    $('header').prepend(alertMessage);

    // Remove the alert after a certain period
    setTimeout(function () {
        alertMessage.remove();
    }, 3000); // Remove after 3 seconds (adjust as needed)
}
function toggleDropdownAndGenerateItems() {
    $('#columnDropdownMenu').toggle(); // Toggle dropdown visibility
    generateDropdownItems(); // Generate dropdown items
}
function generateDropdownItems() {
    $('#columnDropdownMenu').empty(); // Clear existing items
    // Loop through all columns in the DataTable
    $('#employeeTable').DataTable().columns().every(function (index) {
        var columnName = this.header().textContent.trim(); // Get the column name
        // Skip if the column is for actions (identified by the 'no-export' class)
        if (!$(this.header()).hasClass('no-export')) {
            // Determine if the column is currently visible
            var columnVisible = this.visible();
            // Create HTML for a dropdown item with a checkbox for each column
            var checkboxHtml = '<label class="dropdown-item"><input type="checkbox" class="columnCheckbox" data-column="' + index + '" ' + (columnVisible ? 'checked' : '') + '>' + columnName + '</label>';
            // Append the dropdown item HTML to the dropdown menu
            $('#columnDropdownMenu').append(checkboxHtml);
        }
    });
}
function renderChart(response, selectedChart) {
    var rolesSet = new Set();
    response.forEach(function (employee) {
        employee.employeeRole.forEach(function (role) {
            rolesSet.add(role.text);
        });
    });
    var roles = Array.from(rolesSet);
    var roleData = roles.map(function (role) {
        var employeesWithRole = response.filter(function (employee) {
            return employee.employeeRole.some(function (empRole) {
                return empRole.text === role;
            });
        });
        return {
            name: role,
            y: employeesWithRole.length,
            employees: employeesWithRole.map(function (emp) {
                return emp.firstName + ' ' + emp.lastName;
            })
        };
    });

    var commonOptions = {
        chart: {
            type: selectedChart || 'pie', // Change chart type to pie if selectedChart is not provided
            plotBackgroundColor: null,
            plotBorderWidth: null,
            plotShadow: false
        },
        title: {
            text: 'Employee Data'
        },
        xAxis: {
            categories: roles // Set categories for column chart
        },
        yAxis: {
            title: {
                text: 'Number of Employees'
            }
        },
        series: [{
            name: 'Role',
            colorByPoint: true,
            data: roleData
        }]
    };

    if (selectedChart === 'pie' || !selectedChart) {
        commonOptions.tooltip = {
            pointFormat: '<b>{point.name}</b>: {point.y} ({point.percentage:.1f}%)'
        };
        commonOptions.plotOptions = {
            pie: {
                allowPointSelect: true,
                cursor: 'pointer',
                dataLabels: {
                    enabled: true,
                    format: '<b>{point.name}</b>: {point.y} ({point.percentage:.1f}%)'
                },
                showInLegend: true
            }
        };
    } else if (selectedChart === 'column') {
        commonOptions.tooltip = {
            formatter: function () {
                return '<b>' + this.key + '</b><br>' +
                    'Employees: ' + this.point.employees.join(', ') + '<br>' +
                    'Percentage: ' + ((this.y / this.series.yData.reduce((a, b) => a + b, 0)) * 100).toFixed(1) + '%';
            }
        };
        commonOptions.plotOptions = {
            column: {
                colorByPoint: true // Enable color by point for column chart
            }
        };
    }
    $('#confirmationModalBody').text("");

    var chartContainer = document.getElementById("chartContainer");
    // Check if chartContainer is not present
    if (!chartContainer) {
        // Select the element with id "confirmationModalBody"
        var confirmationModalBody = document.getElementById("confirmationModalBody");

        // Create the new element
        var newChartContainer = document.createElement("div");
        newChartContainer.id = "chartContainer";

        // Insert the new element just below the "confirmationModalBody"
        confirmationModalBody.insertAdjacentElement("afterend", newChartContainer);
    }
    Highcharts.chart('chartContainer', commonOptions);
}

// Function to handle editing an employee
function editEmployee(empId) {
    $('#employeeListView').remove();
    // Send AJAX request to the server
    $.ajax({
        url: '/admin/editEmployee', 
        type: 'GET', 
        data: { empId: empId }, 
        success: function (response) {
            if (response) {
                $('#editEmployeeView').html(response).removeClass('d-none');
            }
            else {
                showNotification("An error occurred while loading the edit view.", "danger");
            }
        },
        error: function (xhr, status, error) {
            showNotification("An error occurred while loading the edit view.", "danger");
        }
    });
}

// Function to handle deleting an employee
//function deleteEmployee(empId) {
//    // Send AJAX request to the server
//    $.ajax({
//        url: '/admin/deleteEmployee', 
//        type: 'POST', 
//        data: { empId: empId },
//        success: function (response) {
//            if (response.success) {
//                showNotification("Record delete successfully.", "success")
//            }
//            else {
//                showNotification("Failed to delete record", "danger")
//            }
//            GetEmployeeList();
//        },
//        error: function (xhr, status, error) {
//            console.error('Error deleting employee:', error);
//        }
//    });
//}

var isAlertDisplayed = false;
function handleChartButtonClick(selectedValue) {
    // Check if an alert is already being displayed
    if (isAlertDisplayed) {
        return;
    }

    fetchEmployeeData()
        .then(function (response) {
            if (Array.isArray(response) && response.length > 0) {
                renderChart(response, selectedValue);
                if ($('#chartContainer').is(':hidden')) {
                    $('#chartContainer').show();
                }

                if ($('#chartContainer').length > 0) {
                    $('#confirmationModalLabel').text('Employee Data');
                    $('#confirmActionBtn').hide();
                }
                $('#confirmationModal').modal('show');
            } else {
                // Display an alert indicating no advertisement record
                var alertMessage = $('<div class="alert alert-danger alert-sm text-center" role="alert">No advertisement records found.</div>'); // Added alert-sm for small width
                $('header').prepend(alertMessage);
                isAlertDisplayed = true; // Set flag to true
                setTimeout(function () {
                    alertMessage.fadeOut('slow', function () {
                        $(this).remove();
                        isAlertDisplayed = false; // Reset flag to false
                    });
                }, 3000); // Hide after 3 seconds
            }
        })
        .catch(function (error) {
            isAlertDisplayed = false; // Reset flag in case of error
        });
}

function fetchEmployeeData() {
    var url = ($('#loginedRole').val() === "Admin") ?
        '/Admin/GetEmployeeList' :
        '/Login/UserLogin';

    return $.ajax({
        url: url,
        type: 'GET',
        dataType: 'json'
    });
}

$('#confirmationModal' + ' #closeConfirmationModalBtn').on('click', function () {
    $('#confirmationModal').modal('hide'); // Hide the modal when the close button is clicked
});

document.getElementById("chartDropdown").addEventListener("click", function (event) {
    const target = event.target;
    if (target && target.matches(".dropdown-item")) {
        const selectedValue = target.getAttribute("data-value");
        // Call handleChartButtonClick function
        handleChartButtonClick(selectedValue);
        // Hide the dropdown
        $('#chartDropdown').hide();
    }
});

function showConfirmationModal(title, message, confirmButtonText, confirmCallback, data) {
    $('#chartDropdown').hide();
    // Set modal title and message
    $('#confirmationModalLabel').text(title);
    $('#confirmationModalBody').text(message);
    // Update confirm button text
    $('#confirmActionBtn').text(confirmButtonText);
    $('#confirmActionBtn').show();
    // Store callback function and data in modal data attributes
    $('#confirmActionBtn').data('callback', confirmCallback);
    $('#confirmActionBtn').data('data', data);
    $('#chartContainer').hide();

    // Show the modal
    $('#confirmationModal').modal('show');

    // Bind click event to confirmation button
    $('#confirmActionBtn').off('click').on('click', function () {
        var callback = $(this).data('callback');
        var data = $(this).data('data');
        if (typeof callback === 'function') {
            callback(data);
        }
        $('#confirmationModal').modal('hide');
    });

    // Bind click event to close button
    $('#closeConfirmationModalBtn').off('click').on('click', function () {
        $('#confirmationModal').modal('hide');
    });
}

// Rest of your code remains the same

function deleteEmployee(empId) {
    $('#chartDropdown').hide();
    var title = 'Confirm Delete';
    var message = 'Are you sure you want to delete this advertisement?';
    var confirmButtonText = 'Delete';
    var confirmCallback = performEmployeeDelete;
    var data = { empId: empId };
    // Show the confirmation modal
    showConfirmationModal(title, message, confirmButtonText, confirmCallback, data);
}

// Function to perform the advertisement delete
function performEmployeeDelete(data) {
    $('#chartDropdown').hide();
    var empId = data.empId;
    // Send AJAX request to the server to delete the advertisement
    $.ajax({
        url: '/admin/deleteEmployee',
        type: 'POST',
        data: { empId: empId },
        success: function (response) {
            if (response.success) {
                showNotification("Record deleted successfully.", "success");
            } else {
                showNotification("Failed to delete record", "danger");
            }
            GetEmployeeList();
        },
        error: function (xhr, status, error) {
        }
    });
}




