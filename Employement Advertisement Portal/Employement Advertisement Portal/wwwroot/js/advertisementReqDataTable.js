$(document).ready(function () {
    GetAdvertisementRequest()
    $(document).ready(function () {
        $('#showChartBtn').click(function () {
            $('#chartDropdown').toggle(); // Use toggle to show/hide the dropdown
        });
    });
   
    //$('#chartType').on('change', function () {
    //    var selectedChartType = $(this).val(); // Get the selected chart type
    //    GetAdvertisementRequest(selectedChartType); // Pass selectedChartType to GetEmployeeList
    //});

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
                /*renderChart(response, selectedChartType);*/
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
                /*renderChart(response, selectedChartType);*/
            },
        })
    }
        
    }

function OnSuccess(response) {
    debugger
    if ($.fn.DataTable.isDataTable('#advertisementReqTable')) {
        $('#advertisementReqTable').DataTable().destroy();
    }
    const currentUrl = window.location.href;
    $('#advertisementReqTable').DataTable({
        bProcessing: true,
        bLengthChange: true,
        lengthMenu: [[10, 25, 50, 100], [10, 25, 50, 100]],
        bFilter: true,
        bSort: true,
        bPaginate: true,
        scrollX: false, 
        scrollY: false, 
        "columnDefs": [
            { "className": "dt-center", "targets": "_all" }, // Center-align all columns
            { "orderable": false, "targets": [6,7, 8] } // Disable sorting for columns with indexes 6, 8
        ],
        data: response,
        columns: [
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
            <div style="display: flex; align-items: center; justify-content: center;">
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

                    // Check if the URL contains "Advertisement/UserAdvertisementList"
                    if (currentUrl.includes("Advertisement/UserAdvertisementList")) {
                        actionHtml = '<a href="#" onclick="editAdvertisement(' + row.advId + ')">' +
                            '<i class="fa-solid fa-user-pen" style="color: #05a31f;"></i></a>&nbsp;&nbsp;' +
                            '<a href="#" onclick="deleteAdvertisement(' + row.advId + ')">' +
                            '<i class="fa-solid fa-trash" style="color: #f00540;"></i></a>';
                    } else {
                        // Role check for non-"UserAdvertisementList" URLs
                        if ($('#loginedRole').val() !== "Admin") {
                            actionHtml = '<a href="#" onclick="editAdvertisement(' + row.advId + ')">' +
                                '<i class="fa-solid fa-user-pen" style="color: #05a31f;"></i></a>&nbsp;&nbsp;' +
                                '<a href="#" onclick="deleteAdvertisement(' + row.advId + ')">' +
                                '<i class="fa-solid fa-trash" style="color: #f00540;"></i></a>';
                        } else {
                            actionHtml = '<div style="text-align: center;">' +
                                '<a href="#" onclick="deleteAdvertisement(' + row.advId + ')">' +
                                '<i class="fa-solid fa-trash" style="color: #f00540;"></i></a></div>';
                        }
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
    $('#chartDropdown').hide();
    if (decision == "approved") {
        var data = {
            advId: rowId, // sending rowId as advId
            decision: decision // sending the decision
        };
        ActionOnAdvertisement(data);
    }
    else {
        var title = 'Confirm Reject';
        var message = 'Are you sure you want to reject this advertisement?';
        var confirmButtonText = 'Reject';
        var confirmCallback = ActionOnAdvertisement;
        var data = {
            advId: rowId, // sending rowId as advId
            decision: decision // sending the decision
        };
        showConfirmationModal(title, message, confirmButtonText, confirmCallback, data);
    }
}

function ActionOnAdvertisement(data) {
    $.ajax({
        url: '/Admin/ActionOnAdvertisement',
        type: 'POST',
        data: {
            advId: data.advId, // sending rowId as advId
            decision: data.decision // sending the decision
        },
        dataType: 'json', // change 'bool' to 'json' for clarity
        success: function (response) {
            // Check if response is valid before proceeding
            if (response) {
                if (data.decision === "approved")
                    showNotification("Advertisement approved successfully", "success")
                else {
                    showNotification("Advertisement rejected successfully", "success")
                }
                /*GetAdvertisementRequest()*/
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
    // Create jQuery alert element with the specified message and type
    var alertMessage = $('<div class="alert alert-sm text-center" role="alert"></div>').addClass('alert-' + type).text(message);

    // Prepend the alert message to the header
    $('header').prepend(alertMessage);

    // Remove the alert after a certain period
    setTimeout(function () {
        alertMessage.remove();
    }, 3000); // Remove after 3 seconds (adjust as needed)
}


function handleCheckboxChange() {
    var columnIndex = $(this).data('column');
    var column = $('#advertisementReqTable').DataTable().column(columnIndex);

    var allUnchecked = $('.columnCheckbox').filter(':checked').length === 0;

    if (allUnchecked) {
        showNotification('Please ensure at least one column is visible.', "danger"); // Display alert message
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
            debugger
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

function renderChart(response, selectedChart) {
    var categorySet = new Set();
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

var isAlertDisplayed = false;

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

function handleChartButtonClick(selectedValue) {

    $('#chartDropdown').hide();
    // Check if an alert is already being displayed
    if (isAlertDisplayed) {
        return;
    }

    fetchAdvertisementRequestData()
        .then(function (response) {
            if (Array.isArray(response) && response.length > 0) {
                renderChart(response, selectedValue);
                if ($('#chartContainer').is(':hidden')) {
                    $('#chartContainer').show();
                }

                if ($('#chartContainer').length > 0) {
                    $('#confirmationModalLabel').text('Advertisement Category');
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
            console.error('Error fetching advertisement request data:', error);
            isAlertDisplayed = false; // Reset flag in case of error
        });
}


// Function to fetch advertisement request data
function fetchAdvertisementRequestData() {
    var url = ($('#loginedRole').val() === "Admin" && window.location.pathname !== '/Advertisement/UserAdvertisementList') ?
        '/Admin/GetAdvertisementRequest' :
        '/Advertisement/GetUserAdvertisementList';

    if (url === '/Admin/GetAdvertisementRequest') {
        return $.ajax({
            url: url,
            type: 'GET',
            data: ($('#loginedRole').val() === "Admin") ? {} : { empId: $('#userId').val() },
            dataType: 'json'
        });
    }
    else {
        return $.ajax({
            url: url,
            type: 'GET',
            data: ($('#loginedRole').val() === "Admin") ? { empId: $('#userId').val() } : { empId: $('#userId').val() },
            dataType: 'json'
        });
    }

    
}

$('#confirmationModal' + ' #closeConfirmationModalBtn').on('click', function () {
    $('#confirmationModal').modal('hide'); // Hide the modal when the close button is clicked
});

// Function to show a generic confirmation modal
function showConfirmationModal(title, message, confirmButtonText, confirmCallback, data) {
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
function deleteAdvertisement(advId) {
    $('#chartDropdown').hide();
    var title = 'Confirm Delete';
    var message = 'Are you sure you want to delete this advertisement?';
    var confirmButtonText = 'Delete';
    var confirmCallback = performAdvertisementDelete;
    var data = { advId: advId };
    // Show the confirmation modal
    showConfirmationModal(title, message, confirmButtonText, confirmCallback, data);
    fetchAdvertisementRequestData();
}

// Function to perform the advertisement delete
function performAdvertisementDelete(data) {
    $('#chartDropdown').hide();
    var advId = data.advId;
    // Send AJAX request to the server to delete the advertisement
    $.ajax({
        url: '/advertisement/delete',
        type: 'POST',
        data: { advId: advId },
        success: function (response) {
            if (response.success) {
                showNotification("Record deleted successfully.", "success");
            } else {
                showNotification("Failed to delete record", "error");
            }
            /*GetAdvertisementRequest();*/ // Refresh advertisement data
        },
        error: function (xhr, status, error) {
            console.error('Error deleting advertisement:', error);
        }
    });
}


