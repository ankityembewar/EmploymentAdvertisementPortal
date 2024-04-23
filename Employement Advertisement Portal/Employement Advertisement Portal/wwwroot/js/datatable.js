$(document).ready(function () {
    GetEmployeeList();
    // Listen for changes in the dropdown menu
    $('#chartType').on('change', function () {
        var selectedChartType = $(this).val(); // Get the selected chart type
        GetEmployeeList(selectedChartType); // Pass selectedChartType to GetEmployeeList
    });

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
            renderChart(response, selectedChartType);
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
            //{
            //    data: "Gender",
            //    render: function (data, type, row, meta) {
            //        return row.gender
            //    }
            //},
            //{
            //    data: "Dob",
            //    title: "Birth Date",
            //    render: function (data, type, row, meta) {
            //        var dob = new Date(row.dob);
            //        // Format the date as a string without the time
            //        var formattedDate = dob.toLocaleDateString();
            //        return formattedDate;
            //    }
            //},
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
        showNotification('Please ensure at least one column is visible.',"error"); // Display alert message
    } else {
        // Toggle the visibility of the column based on checkbox state
        column.visible($(this).is(':checked'));
    }
}
function showNotification(message, type = 'info') {
    // Create a notification element with the Bootstrap alert style based on the type
    var notification = document.createElement('div');
    notification.className = 'alert';

    // Set the alert class based on the type
    switch (type) {
        case 'success':
            notification.classList.add('alert-success');
            break;
        case 'error':
            notification.classList.add('alert-danger');
            break;
        case 'info':
            notification.classList.add('alert-info');
            break;
        default:
            notification.classList.add('alert-info');
            break;
    }

    notification.setAttribute('role', 'alert');

    // Add the message to the notification
    notification.textContent = message;

    // Position the notification at the top of the page
    notification.style.position = 'fixed';
    notification.style.top = '0';
    notification.style.left = '50%';
    notification.style.transform = 'translateX(-50%)';
    notification.style.zIndex = '1000'; // Ensure it appears above other elements

    // Append the notification to the document body
    document.body.appendChild(notification);

    // Remove the notification after a certain period
    setTimeout(function () {
        notification.remove();
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
            name: 'Number of Employees',
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
                showNotification("An error occurred while loading the edit view.", "error");
            }
        },
        error: function (xhr, status, error) {
            console.error('Error loading edit view:', error);
            showNotification("An error occurred while loading the edit view.", "error");
        }
    });
}

// Function to handle deleting an employee
function deleteEmployee(empId) {
    // Send AJAX request to the server
    $.ajax({
        url: '/admin/deleteEmployee', 
        type: 'POST', 
        data: { empId: empId },
        success: function (response) {
            if (response.success) {
                showNotification("Record delete successfully.", "success")
            }
            else {
                showNotification("Failed to delete record", "error")
            }
            GetEmployeeList();
        },
        error: function (xhr, status, error) {
            console.error('Error deleting employee:', error);
        }
    });
}






