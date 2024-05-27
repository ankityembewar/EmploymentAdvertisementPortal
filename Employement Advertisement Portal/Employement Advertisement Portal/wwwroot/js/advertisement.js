function submitAdvertisementForm() {
    var empId = document.getElementById('EmpId').value;
    var form = document.getElementById('createEditAdvertiseForm');

    // Set form action based on empId
    if (empId !== "0") {
        // Submit to Edit Advertisement action method
        form.action = "/Advertisement/Edit";
    } else {
        // Submit to Add Advertisement action method
        form.action = "/Advertisement/Create";
    }

    form.method = "POST"; // Set the form method to POST

    // Submit the form
    form.submit();
}

