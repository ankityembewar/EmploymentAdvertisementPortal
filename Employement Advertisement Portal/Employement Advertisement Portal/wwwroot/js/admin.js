function submitEmployeeForm(event) {
    event.preventDefault();  // Prevent form submission and page refresh

    var empId = document.getElementById('EmpId').value;
    var form = document.getElementById('createEditForm');
    var roleId = document.getElementById('RoleId').value;
    var dobErrorSpan = document.getElementById('DobValidation');
    var roleErrorSpan = document.getElementById('RoleValidation');
    roleErrorSpan.textContent = "";

    // Clear previous error messages
    dobErrorSpan.textContent = '';

    // Check if a valid role is selected
    if (roleId !== "" && roleId !== null && isAgeValid()) {
        if (empId !== "0") {
            // Submit to EditEmployee action method
            form.action = "/Admin/EditEmployee";
        } else {
            // Submit to AddEmployee action method
            form.action = "/Admin/AddEmployee";
        }
        form.method = "post";
        form.submit();
    }
    else if (roleId == "" || roleId == null && !isAgeValid()) {
        // Show error message indicating that a role must be selected
        dobErrorSpan.textContent = "Employee must be 18 years old or older.";
        document.getElementById('Dob').focus();

        roleErrorSpan.textContent = "Please select a role.";
        document.getElementById('RoleId').focus();
    }
    else if (roleId == "" || roleId == null) {
        roleErrorSpan.textContent = "Please select a role.";
        document.getElementById('RoleId').focus();
    }
    else if (!isAgeValid())
    {
        dobErrorSpan.textContent = "Employee must be 18 years old or older.";
        document.getElementById('Dob').focus();
    }
}

function isAgeValid() {
    var dob = document.getElementById('Dob').value;
    var dobDate = new Date(dob);
    var today = new Date();

    // Calculate age in milliseconds
    var ageInMilliseconds = today - dobDate;

    // Calculate age in years
    var age = ageInMilliseconds / (1000 * 60 * 60 * 24 * 365.25);

    return age >= 18;
}

function validatePassword() {
    var password = document.getElementById("Password").value;
    var confirmPassword = document.getElementById("ConfirmPassword").value;
    if (password !== confirmPassword) {
        // If passwords don't match, show error message and prevent form submission
        document.getElementById("confirmPasswordError").innerText = "Passwords do not match";
        return false;
    }
    // If passwords match, clear error message and allow form submission
    document.getElementById("confirmPasswordError").innerText = "";
    return true;
}

// Attach val+idatePassword function to form submission event
$(document).ready(function () {
    if ($('#account').length > 0) {
        $('#account').on('submit', function (event) {
            if (!validatePassword()) {
                // If passwords don't match, prevent form submission
                event.preventDefault();
            }
        });
    }
});