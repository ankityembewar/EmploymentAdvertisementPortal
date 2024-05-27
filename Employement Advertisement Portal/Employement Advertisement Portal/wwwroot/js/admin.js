function submitEmployeeForm() {
    var empId = document.getElementById('EmpId').value;
    var form = document.getElementById('createEditForm');

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

// Attach validatePassword function to form submission event
document.getElementById("account").addEventListener("submit", function (event) {
    if (!validatePassword()) {
        // If passwords don't match, prevent form submission
        event.preventDefault();
    }
});