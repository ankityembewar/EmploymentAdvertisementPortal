$(document).ready(function () {
    GetAdvertisementRequest()
    $('#advertisechartType').on('change', function () {
        var selectedChartType = $(this).val(); // Get the selected chart type
        GetAdvertisementRequest(selectedChartType); // Pass selectedChartType to GetEmployeeList
    });

    // Function to close dropdown menu when clicking outside or clicking dropdown button again
    $(document).on('click', function (event) {
        // Check if the clicked element is inside the dropdown menu or the dropdown button
        if (!$(event.target).closest('#advertcolumnDropdownMenu').length && !$(event.target).closest('#advertColumnDropdown').length) {
            // If the clicked element is outside the dropdown menu and the dropdown button, close the dropdown menu
            $('#advertcolumnDropdownMenu').hide();
        }
    });

    $('#advertColumnDropdown').click(function () {
        toggleDropdownAndGenerateItems(); // Call the toggleDropdownAndGenerateItems function when the button is clicked
    });

    // Attach the handleCheckboxChange function to the change event of .columnCheckbox elements
    $(document).on('click', '.columnCheckbox', handleCheckboxChange);
});

function GetAdvertisementRequest(selectedChartType) {
    if ($('#loginedRole').val() === "Admin" & window.location.pathname !== '/Advertisement/UserAdvertisementList') {
        $.ajax({
            url: '/Admin/GetAdvertisementRequest',
            type: 'Get',
            dataType: 'json',
            success: function (response) {
                OnSuccess(response);
                renderChart(response, selectedChartType);
            }

        })
    }
    else {
        $.ajax({
            url: '/Advertisement/GetUserAdvertisementList',
            type: 'Get',
            data: {
                empId: $('#userId').val(),
            },
            dataType: 'json',
            success: function (response) {
                OnSuccess(response);
                renderChart(response, selectedChartType);
            },
        })
    }
        
    }

function OnSuccess(response) {
    debugger
    if ($.fn.DataTable.isDataTable('#advertisementReqTable')) {
        $('#advertisementReqTable').DataTable().destroy();
    }
    $('#advertisementReqTable').DataTable({
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
                data: "AdvId",
                render: function (data, type, row, meta) {
                    return row.advId
                }
            },
            {
                data: "Title",
                render: function (data, type, row, meta) {
                    return row.title
                }
            },
            {
                data: "Price",
                render: function (data, type, row, meta) {
                    return row.price
                }
            },
            {
                data: "CreatedBy",
                render: function (data, type, row, meta) {
                    if (row.employeeDetail && row.employeeDetail.email.length > 0) {
                        return row.employeeDetail.email;
                    } else {
                        return "Not Assign"; // Handle the case where employeeRole is empty or undefined
                    }
                }
            },
            {
                data: "CreatedDate",
                render: function (data, type, row, meta) {
                    return row.createdDate.substring(0, 10);
                }
            },
            {
                data: "Location",
                render: function (data, type, row, meta) {
                    return row.location;
                }
            },
            {
                data: "Category",
                render: function (data, type, row, meta) {
                    if (row.advertisementCategoryList && row.advertisementCategoryList[0].text.length > 0) {
                        return row.advertisementCategoryList[0].text;
                    } else {
                        return "Not Assign"; // Handle the case where employeeRole is empty or undefined
                    }
                }
            },
            {
                data: "MediaPath",
                Title:"Image",
                render: function (data, type, row, meta) {
                    return `<img src="${row.mediaPath}" alt="Advertisement Image" style="width:100px; height:auto;">`;
                }
                
            },
            {
                data: "Decision Status",
                title: "Decision Status",
                render: function (data, type, row, meta) {
                    // Check if the logged-in user's role is 'Admin' and set the disabled attribute accordingly
                    const isDisabled = $('#loginedRole').val() !== "Admin";

                    // Create radio buttons with icons for isApproved and isRejected
                    return `
            <div style="display: flex; align-items: center;">
                <input type="radio" id="isApproved_${row.advId}" name="decision_${row.advId}" value="approved"
                       ${row.isApproved ? 'checked' : ''} 
                       ${isDisabled ? 'disabled' : ''}
                       onclick="handleDecisionChange(${row.advId}, 'approved')">
                <label for="isApproved_${row.advId}" style="margin-right: 10px;">
                    <i class="fa fa-check" style="color: #05a31f;" title="Approve Employee"></i>
                </label>

                <input type="radio" id="isRejected_${row.advId}" name="decision_${row.advId}" value="rejected"
                       ${row.isRejected ? 'checked' : ''} 
                       ${isDisabled ? 'disabled' : ''}
                       onclick="handleDecisionChange(${row.advId}, 'rejected')">
                <label for="isRejected_${row.advId}">
                    <i class="fa fa-times" style="color: #f00540;" title="Reject Employee"></i>
                </label>
            </div>`
                }
            },
            {
                data: null,
                title: "Actions",
                render: function (data, type, row, meta) {
                    // Variable to hold the final HTML content
                    let actionHtml;

                    // Check if the logged-in user's role is not 'Admin'
                    if ($('#loginedRole').val() !== "Admin") {
                        // If not 'Admin', include both edit and delete icons
                        actionHtml = '<a href="#" onclick="editAdvertisement(' + row.advId + ')">' +
                            '<i class="fa-solid fa-user-pen" style="color: #05a31f;"></i></a>&nbsp;&nbsp;' +
                            '<a href="#" onclick="deleteAdvertisement(' + row.advId + ')">' +
                            '<i class="fa-solid fa-trash" style="color: #f00540;"></i></a>';
                    } else {
                        // If the role is 'Admin', center the delete icon in the column
                        actionHtml = '<div style="text-align: center;">' +
                            '<a href="#" onclick="deleteAdvertisement(' + row.advId + ')">' +
                            '<i class="fa-solid fa-trash" style="color: #f00540;"></i></a></div>';
                    }

                    // Return the final HTML content for the cell
                    return actionHtml;
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
                       text: '<i class="fa-solid fa-pager" style="color: #e290f9;"></i>',
                       titleAttr: 'Entries per page'
                   }
               ]
           }
       }

    });
}

function toggleApproval(option, createdDate) {
    // Handle the approval or rejection option
    if (option === 'approved') {
        console.log(`Employee with createdDate ${createdDate} is approved.`);
    } else if (option === 'rejected') {
        console.log(`Employee with createdDate ${createdDate} is rejected.`);
    }
}

function renderChart(response, selectedChart) {
    var categorySet = new Set();
    debugger
    response.forEach(function (employee) {
        employee.advertisementCategoryList.forEach(function (category) {
            categorySet.add(category.text);
        });
    });
    var roles = Array.from(categorySet);
    var roleData = roles.map(function (role) {
        var employeesWithRole = response.filter(function (employee) {
            return employee.advertisementCategoryList.some(function (empRole) {
                return empRole.text === role;
            });
        });
        return {
            name: role,
            y: employeesWithRole.length,
            employees: employeesWithRole.map(function (emp) {
                return emp.employeeDetail.firstName + ' ' + emp.employeeDetail.lastName;
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
            text: 'Advertisement Category Data'
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

function generateDropdownItems() {
    $('#advertcolumnDropdownMenu').empty(); // Clear existing items
    // Loop through all columns in the DataTable
    $('#advertisementReqTable').DataTable().columns().every(function (index) {
        var columnName = this.header().textContent.trim(); // Get the column name
        // Skip if the column is for actions (identified by the 'no-export' class)
        if (!$(this.header()).hasClass('no-export')) {
            // Determine if the column is currently visible
            var columnVisible = this.visible();
            // Create HTML for a dropdown item with a checkbox for each column
            var checkboxHtml = '<label class="dropdown-item"><input type="checkbox" class="columnCheckbox" data-column="' + index + '" ' + (columnVisible ? 'checked' : '') + '>' + columnName + '</label>';
            // Append the dropdown item HTML to the dropdown menu
            $('#advertcolumnDropdownMenu').append(checkboxHtml);
        }
    });
}

function toggleDropdownAndGenerateItems() {
    $('#advertcolumnDropdownMenu').toggle(); // Toggle dropdown visibility
    generateDropdownItems(); // Generate dropdown items
}

function handleDecisionChange(rowId, decision) {
    
    $.ajax({
        url: '/Admin/ActionOnAdvertisement',
        type: 'POST',
        data: {
            advId: rowId, // sending rowId as advId
            decision: decision // sending the decision
        },
        dataType: 'json', // change 'bool' to 'json' for clarity
        success: function (response) {
            // Check if response is valid before proceeding
            if (response) {
                GetAdvertisementRequest()
                showNotification("Advertisement updated successfully","success")
            } else {
                showNotification("Failed to update", "error")
            }
        },
        error: function (jqXHR, textStatus, errorThrown) {
            // Handle AJAX request errors
            console.error(`AJAX request failed: ${textStatus}, ${errorThrown}`);
        }
    });
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

function handleCheckboxChange() {
    var columnIndex = $(this).data('column');
    var column = $('#advertisementReqTable').DataTable().column(columnIndex);

    var allUnchecked = $('.columnCheckbox').filter(':checked').length === 0;

    if (allUnchecked) {
        showNotification('Please ensure at least one column is visible.', "error"); // Display alert message
    } else {
        // Toggle the visibility of the column based on checkbox state
        column.visible($(this).is(':checked'));
    }
}

// Function to handle editing an employee
function editAdvertisement(advId) {
    $('#advertisementReqListView').remove();
    // Send AJAX request to the server
    $.ajax({
        url: '/advertisement/edit',
        type: 'GET',
        data: { advId: advId },
        success: function (response) {
            if (response) {
                $('#editAdvertisement').html(response).removeClass('d-none');
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
function deleteAdvertisement(advId) {
    // Send AJAX request to the server
    $.ajax({
        url: '/advertisement/delete',
        type: 'POST',
        data: { advId: advId },
        success: function (response) {
            if (response.success) {
                showNotification("Record delete successfully.", "success")
            }
            else {
                showNotification("Failed to delete record", "error")
            }
            GetAdvertisementRequest();
        },
        error: function (xhr, status, error) {
            console.error('Error deleting employee:', error);
        }
    });
}



