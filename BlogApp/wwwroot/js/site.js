$(document).ready(() => {

    //A js script for validation on dropdown with checkboxes in the Create view
    //i will think of a better way later
    $('#post-form').submit(() => {
        if ($('input[name="category.IsSelected"]:checked').length === 0) {
            alert('Please select at least one category.');
            return false;
        }
    });

    $('#changeData').click(function () {
        $('#change').remove();

        let saveBtnHtml = '\
        <div id="save" class="text-center mt-5">\
            <input value="Save Data" type="submit" id="saveData" class="btn btn-primary" />\
        </div>\
        ';

        $('#changeDataForm').append(saveBtnHtml);

        $('#FirstName').prop('disabled', false);
        $('#LastName').prop('disabled', false);
        $('#Email').prop('disabled', false);
        $('#UserName').prop('disabled', false);
        $('#PhoneNumber').prop('disabled', false);
    });

    $('#saveData').click(function () {
        $('#FirstName').prop('disabled', true);
        $('#LastName').prop('disabled', true);
        $('#Email').prop('disabled', true);
        $('#UserName').prop('disabled', true);
        $('#PhoneNumber').prop('disabled', true);

        $('save').remove();
    });

    //loading the comments with partial view by passing the post id and hiding the button using ajax
    //this way the page isn't refreshed
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

    //the tables for admin dashboars
    //look up datatable
    $('#usersTable').DataTable();
    $('#reportsTable').DataTable();
});