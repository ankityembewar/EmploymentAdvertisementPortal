// Function to remove the error message after the fade-out animation completes
//function removeErrorMessage() {
//    var errorMessage = document.getElementById("errorMessage");
//    if (errorMessage) {
//        errorMessage.remove(); // Remove the error message div
//    }
//}
function refreshPageWithoutReload() {
    if (window.history.replaceState) {
        window.history.replaceState(null, null, window.location.href);
    }
}

//// Function to initiate the fade-out animation
//function fadeOutErrorMessage() {
//    var errorMessage = document.getElementById("errorMessage");
//    if (errorMessage) {
//        errorMessage.classList.add("fade-out"); // Apply fade-out animation class
//        errorMessage.addEventListener("animationend", removeErrorMessage); // Call remove function after animation ends
//    }
//}

//// Function to hide the error message after 5 seconds
//setTimeout(fadeOutErrorMessage, 5000); // Delay for 5 seconds before hiding
