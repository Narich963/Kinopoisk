$(document).ready(function () {
    $('#genresTable').DataTable({
        serverSide: true,
        processing: true,
        ajax: {
            url: '/Genres/Index?handler=GetGenres',
            type: 'POST',
            contentType: 'application/json',
            data: function (d) {
                console.log(d);
                return JSON.stringify(d);
            }
        },
        columns: [
            { data: 'id' },
            { data: 'name' },
        ],
    });
});