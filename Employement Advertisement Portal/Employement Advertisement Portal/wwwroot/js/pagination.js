var currentPage = 0; // Start from page 0
var pageSize = 9;
var loading = false;
var allRecordsLoaded = false;
var advertisementLocation = $('#location').val();
var page = 1; // Initial page number
var isLoading = false; // Flag to prevent multiple simultaneous requests


$(window).scroll(function () {
    if ($(window).scrollTop() + $(window).height() >= $(document).height() - 100 && !loading && !allRecordsLoaded) {
        loadAdvertisements();
    }
});

function loadAdvertisements() {
    if (loading || allRecordsLoaded) return;
    loading = true;
    $('#loading-indicator').show();
    debugger;
    $.ajax({
        url: '/advertisement/GetAdvertisements',
        type: 'GET',
        data: { page: currentPage, pageSize: pageSize},
        success: function (response) {
            if (response.trim() === '') {
                allRecordsLoaded = true;
            } else {
                $('#advertisements-container').append(response);
                currentPage++; // Increment page for the next request
            }
            loading = false;
            $('#loading-indicator').hide();
        },
        error: function (xhr, status, error) {
            console.error('Error loading advertisements:', error);
            loading = false;
            $('#loading-indicator').hide();
        }
    });
}

function fetchAdvertisements() {
    if (!isLoading) {
        isLoading = true; // Set loading flag to true
        $('#loadingIndicator').show(); // Show loading indicator

        // Make AJAX request to fetch more advertisements
        $.ajax({
            url: '@Url.Action("Search", "Advertisement")', // Update URL as per your controller and action method
            type: 'POST',
            data: {
                location: $('#location').val(),
                category: $('#category').val(),
                page: page
            },
            success: function (data) {
                // Append new advertisements to the list
                $('#advertisementList').append(data);
                page++; // Increment page number
            },
            complete: function () {
                isLoading = false; // Reset loading flag
                $('#loadingIndicator').hide(); // Hide loading indicator
            }
        });
    }
}

// Function to check if user has scrolled to the bottom of the page
$(window).scroll(function () {
    if ($(window).scrollTop() + $(window).height() >= $(document).height() - 100) {
        fetchAdvertisements(); // Fetch more advertisements when user scrolls near the bottom
    }
});

// Initial fetch of advertisements when page loads
fetchAdvertisements();

loadAdvertisements(); // Load initial advertisements
