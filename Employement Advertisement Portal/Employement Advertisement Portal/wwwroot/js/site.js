function refreshPageWithoutReload() {
    if (window.history.replaceState) {
        window.history.replaceState(null, null, window.location.href);
    }
}

function initializeEmployeeDataTable() {
    $(document).ready(function () {
        $('#employeeTable').DataTable({
            // DataTables options
        });
    });
}
