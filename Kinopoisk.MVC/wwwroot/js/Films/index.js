$(document).ready(function () {
    $('#filmsTable').DataTable({
        ajax: {
            url: '/Films/Index?handler=GetFilms',
            dataSrc: ''
        },
        columns: [
            { data: 'id' },
            { data: 'poster', render: function (data) { return '<img src="' + data + '" alt="Poster" style="width:100px;height:auto;">'; } },
            { data: 'name' },
            { data: 'description' },
            {
                data: 'publishDate',
                render: function (data) {
                    return new Date(data).toLocaleDateString();
                }
            },
            {
                data: 'countryFlagLink',
                render: function (data) {
                    return `<img src="${data}" style="margin-right:5px;">`;
                }
            },
            { data: 'duration' },
            { data: 'imdbRating' },
            { data: 'usersRating' },
            { data: 'directorName' }
        ],
        createdRow: function (row, data) {
            $(row).css('cursor', 'pointer');
            $(row).on('click', function () {
                window.location.href = '/Films/Details?id=' + data.id;
            });
        }
    });
});