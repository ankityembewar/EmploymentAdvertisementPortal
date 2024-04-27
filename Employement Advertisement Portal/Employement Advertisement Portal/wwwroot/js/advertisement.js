// Initialize lazy loading
document.addEventListener("DOMContentLoaded", function () {
    const container = document.getElementById("advertisements-container");
    const loadingPlaceholder = document.getElementById("loading-placeholder");

    let currentPage = 1;
    let isLoading = false;

    // Function to load more advertisements
    function loadMoreAdvertisements() {
        // Return if a request is already in progress
        if (isLoading) return;

        // Set isLoading to true and display a loading message
        isLoading = true;
        loadingPlaceholder.innerHTML = "Loading more advertisements...";

        // Fetch the next page of advertisements from the server
        fetch(`/Advertisement/GetAdvertisements?page=${currentPage}`)
            .then(response => response.text())
            .then(html => {
                // Insert the new advertisements before the loading placeholder
                if (html.trim()) {
                    loadingPlaceholder.insertAdjacentHTML("beforebegin", html);
                    currentPage += 1; // Increment the page number for the next request
                } else {
                    // No more advertisements to load
                    loadingPlaceholder.innerHTML = "No more advertisements.";
                }

                // Reset isLoading flag
                isLoading = false;
            })
            .catch(error => {
                // Handle any errors that occur during the request
                console.error("Error loading advertisements:", error);
                loadingPlaceholder.innerHTML = "Error loading advertisements.";
                isLoading = false;
            });
    }

    // Event handler for scroll event
    function handleScroll() {
        // Check if the user has scrolled near the bottom of the container
        if (window.innerHeight + window.scrollY >= container.offsetHeight - 200) {
            loadMoreAdvertisements(); // Call the function to load more advertisements
        }
    }

    // Add an event listener for the scroll event on the window
    window.addEventListener("scroll", handleScroll);
});

function submitAdvertisementForm() {
    var empId = document.getElementById('EmpId').value;
    var form = document.getElementById('createEditAdvertiseForm');

    if (empId !== "0") {
        // Submit to Edit Advertisement action method
        form.action = "/Advertisement/Edit";
    } else {
        // Submit to Add Advertisement action method
        form.action = "/Advertisement/Create";
    }
    form.method = "post";
    form.submit();
}