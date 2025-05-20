$(document).ready(function () {
    let dataTable = $('#filmsTable').DataTable({
        serverSide: true,
        processing: true,
        paging: true,
        ordering: true,
        ajax: {
            url: '/Films/Index?handler=GetFilms',
            type: 'POST',
            contentType: 'application/json',
            data: function (d) {
                d.name = $('#nameFilter').val();
                d.year = $('#yearFilter').val();
                d.country = $('#countryFilter').val();
                d.actor = $('#actorFilter').val();
                d.director = $('#directorFilter').val();

                return JSON.stringify(d);
            }
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

    let timeout;

    $('#nameFilter, #yearFilter, #countryFilter, #actorFilter, #directorFilter').on('change keyup', function (e) {
        clearTimeout(timeout);
        timeout = setTimeout(function () {
            dataTable.ajax.reload();
        }, 300);
    });

    $('#resetFilter').on('click', function () {
        $('#nameFilter, #yearFilter, #countryFilter, #actorFilter, #directorFilter').val('');
        dataTable.ajax.reload();
    });
});