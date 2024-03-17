//A js script for validation on dropdown with checkboxes in the Create view
$(document).ready(function () {
    $('#post-form').submit(function () {
        if ($('input[name="category.IsSelected"]:checked').length === 0) {
            alert('Please select at least one category.');
            return false;
        }
    });
});
