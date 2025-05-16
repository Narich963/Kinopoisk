$(document).ready(function () {
    const filmId = $("#comments-container").data("film-id");
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
                    loadComments();
                } else {
                    alert('Failed to add comment');
                }
            },
            error: function () {
                alert('Failed to add comment');
            }
        });
    });
    function loadComments() {
        $.ajax({
            url: '/Films/Details?handler=GetComments',
            type: 'GET',
            data: { filmId: filmId },
            success: function (comments) {
                const container = $('#comments-container');
                container.empty();

                if (comments.length === 0) {
                    container.replaceWith('<p class="text-muted">There are no comments yet!</p>');
                    return;
                }

                comments.forEach(comment => {
                    const item = `
                                <div class="list-group-item list-group-item-action mb-2 rounded shadow-sm">
                                    <div class="d-flex w-100 justify-content-between">
                                        <h6 class="mb-1 text-primary fw-bold">${comment.user.userName}</h6>
                                        <small class="text-muted">${new Date(comment.createdAt).toLocaleString()}</small>
                                    </div>
                                    <p class="mb-1">${comment.text}</p>
                                </div>`;
                    container.append(item);
                });
            },
            error: function () {
                console.error('Failed to load comments');
            }
        });
    }
    loadComments();
});