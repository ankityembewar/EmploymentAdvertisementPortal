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