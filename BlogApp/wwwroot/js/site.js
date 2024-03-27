//A js script for validation on dropdown with checkboxes in the Create view
//i will think of a better way later
$(document).ready(() => {
    $('#post-form').submit(() => {
        if ($('input[name="category.IsSelected"]:checked').length === 0) {
            alert('Please select at least one category.');
            return false;
        }
    });
});

//loading the comments with partial view by passing the post id and hiding the button using ajax
//this way the page isn't refreshed
$(document).ready(() => {
    $('#loadComments').click(() => {
        let postId = $('#postId').val();

        $.ajax({
            url: '/Comment/LoadComments?postId=' + postId,
            type: 'GET',
            success: (data) => {
                $('#commentsContainer').html(data);
            }
        });

        $('#loadComments').hide();
    });

    $('#usersTable').DataTable();
});
