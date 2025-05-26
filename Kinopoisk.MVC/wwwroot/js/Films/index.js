let deleteId = null;
function remove(id) {
    deleteId = id;
    const modal = new bootstrap.Modal(document.getElementById('deleteModal'));
    modal.show();
}

function showErrorModal(message) {
    $('#errorModalBody').text(message);
    var modal = new bootstrap.Modal(document.getElementById('errorModal'));
    modal.show();
}

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
            {
                data: 'poster',
                searchable: false,
                orderable: false,
                render: function (data) {
                    return '<img src="' + data + '" alt="Poster" style="width:100px;height:auto;">';
                }
            },
            { data: 'name' },
            { data: 'description' },
            {
                data: 'publishDate',
                render: function (data) {
                    return new Date(data).toLocaleDateString();
                }
            },
            {
                data: 'country',
                render: function (data) {
                    return `<img src="${data.flag}" style="margin-right:5px;">`;
                }
            },
            { data: 'duration' },
            { data: 'imdbRating' },
            { data: 'usersRating' },
            {
                data: 'director.filmEmployee.name'
            },
            {
                data: null,
                orderable: false,
                searchable: false,
                render: function (data, type, row) {
                    return `
                    <a class="btn btn-sm btn-warning" href="/Films/AddOrEdit?id=${row.id}" onclick="event.stopPropagation()">Edit</a>
                    <a class="btn btn-sm btn-danger" href="#" onclick="event.stopPropagation(); remove(${row.id})">Delete</a>`;
                }
            }
        ],
        createdRow: function (row, data) {
            $(row).css('cursor', 'pointer');
            $(row).on('click', function () {
                window.location.href = '/Films/Details?id=' + data.id;
            });
        }
    });

    $('#confirmDeleteBtn').on('click', function (e) {
        if (deleteId !== null) {
            fetch(`/Films/Index?handler=DeleteFilm&id=${deleteId}`, {
                method: 'POST',
            })
                .then(response => {
                    if (response.ok) {
                        $('#deleteModal').modal('hide');
                        dataTable.ajax.reload();
                    } else {
                        return response.text().then(function (errorText) {
                            $('#deleteModal').modal('hide');
                            showErrorModal(errorText);
                        });
                    }
                })
                .catch(error => {
                    $('#deleteModal').modal('hide');
                    showErrorModal(error.message);
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