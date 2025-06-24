$(document).ready(function () {
    const connection = new signalR.HubConnectionBuilder()
        .withUrl("/comments")
        .build();

    connection.on("Receive", function (userName, message) {
        table.ajax.reload(null, false);
    });

    connection.start()
        .catch(err => console.error(err.toString()));

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
        const formData = form.serializeArray();
        let comment = formData.find(x => x.name == "Comment.Text")?.value;

        connection.invoke("Send", comment, filmId)
            .then(() => {
                $('#addCommentModal').modal('hide');
                form[0].reset();
            })
            .catch(err => console.error(err.toString()));
    });

    $('#ratingForm').submit(function (e) {
        e.preventDefault();

        const form = $(this);
        const formData = form.serialize();

        $.ajax({
            url: '/Films/Details?handler=AddOrEditRating',
            type: 'POST',
            data: formData,
            success: function (response) {
                $('#filmRating').text(response.newRating.toFixed(1));
                loadUserRating();
            },
            error: function () {
                alert('Failed to rate film');
            }
        });
    });

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
});