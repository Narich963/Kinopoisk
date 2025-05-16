$(document).ready(function () {
    const filmId = $("#comments-container").data("film-id");

    let table = $('#commentsTable').DataTable({
        ajax: {
            url: '/Films/Details?handler=GetComments',
            dataSrc: '',
            data: { filmId: filmId }
        },
        columns: [
            { data: 'user.userName' },
            { data: 'text' },
            {
                data: 'createdAt',
                render: function (data) {
                    return new Date(data).toLocaleString();
                }
            }
        ],
        order: [[2, 'desc']],
        paging: true,
        info: true
    });
    

    $('#addCommentForm').submit(function (e) {
        e.preventDefault();

        const form = $(this);
        const formData = form.serialize();

        $.ajax({
            url: '/Films/Details?id=' + filmId + '&handler=AddComment',
            type: 'POST',
            data: formData,
            success: function (response) {
                if (response.success) {
                    $('#addCommentModal').modal('hide');
                    form[0].reset();
                    table.ajax.reload(null, false);
                } else {
                    alert('Failed to add comment');
                }
            },
            error: function () {
                alert('Failed to add comment');
            }
        });
    });
});