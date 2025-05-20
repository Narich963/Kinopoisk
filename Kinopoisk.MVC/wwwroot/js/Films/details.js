$(document).ready(function () {
    const filmId = $("#comments-container").data("film-id");

    let table = $('#comments-container').DataTable({
        serverSide: true,
        processing: true,
        ajax: {
            url: '/Films/Details?handler=GetComments',
            type: 'POST',
            contentType: 'application/json',
            data: function (d) {
                d.filmId = filmId;
                return JSON.stringify(d);
            }
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

    $('#ratingForm').submit(function (e) {
        e.preventDefault();

        const form = $(this);
        const formData = form.serialize();

        console.log(formData);

        $.ajax({
            url: '/Films/Details?handler=AddOrEditRating',
            type: 'POST',
            data: formData,
            success: function (response) {
                loadRating();
                loadUserRating();
            },
            error: function () {
                alert('Failed to rate film');
            }
        });
    });

    function loadRating() {
        $.ajax({
            url: '/Films/Details?handler=GetRating',
            type: 'GET',
            data: { filmId: filmId },
            success: function (response) {
                $('#filmRating').text(response);
            },
            error: function () {
                alert('Failed to load rating');
            }
        });
    }
    function loadUserRating() {
        $.ajax({
            url: '/Films/Details?handler=GetUserRating',
            type: 'GET',
            data: { filmId: filmId },
            success: function (response) {
                if (response > 0) {
                    $(`#star-${response}`).prop('checked', true);
                }
                else {
                    $('#userRating').text('Rate this film!');
                }
            },
            error: function () {
                alert('Failed to load user rating');
            }
        });
    }

    loadUserRating();
    loadRating();
});