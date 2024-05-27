var currentPage = 0; // Start from page 1
var pageSize = 9;
var loading = false;
var allRecordsLoaded = false;

$('#search-form').submit(function (e) {
    e.preventDefault(); // Prevent the default form submission behavior

    var locationValue = $('#location').val().trim();
    var categoryValue = $('#category').val().trim();

    // Check if either location or category is empty
    if (locationValue === '' && categoryValue === '') {
        $('#search-error').text('Please select location or category');
        $('#search-error').show();
        return; // Exit the function if either is empty
    }

    // Clear previous error message if any
    $('#search-error').text('');

    var formData = $(this).serialize(); // Serialize form data
    var url = $(this).attr('action'); // Get the form action URL

    // Make AJAX request to submit the form data
    $.ajax({
        url: url,
        type: 'POST',
        data: formData,
        success: function (data) {
            $('#advertisements-container').html(data); // Update search results
        },
        error: function (xhr, status, error) {
            console.error('Error searching advertisements:', error);
        }
    });
});


// Function to load more advertisements when scrolling
$(window).scroll(function () {
    if ($(window).scrollTop() + $(window).height() >= $(document).height() - 100 && !loading && !allRecordsLoaded) {
        loadAdvertisements();
    }
});

function loadAdvertisements() {
    if (loading || allRecordsLoaded) return;
    loading = true;
    $('#loading-indicator').show();

    $.ajax({
        url: '/advertisement/GetAdvertisements',
        type: 'GET',
        data: { page: currentPage, pageSize: pageSize },
        success: function (response) {
            if (response.trim() === '') {
                allRecordsLoaded = true;
            } else {
                $('#advertisements-container').append(response);
                currentPage++; // Increment page for the next request
            }
        },
        error: function (xhr, status, error) {
            console.error('Error loading advertisements:', error);
        },
        complete: function () {
            loading = false;
            $('#loading-indicator').hide();
        }
    });
}

// Initial fetch of advertisements when page loads
loadAdvertisements();
