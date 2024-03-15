//A js script for validation a dropdown with checkboxes
$(document).ready(function () {
    $('#post-form').submit(function () {
        if ($('input[name="category.IsSelected"]:checked').length === 0) {
            alert('Please select at least one category.');
            return false;
        }
    });
});
