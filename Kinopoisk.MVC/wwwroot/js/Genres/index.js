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
    let dataTable = $('#genresTable').DataTable({
        serverSide: true,
        processing: true,
        ajax: {
            url: `/Genres/Index?handler=GetGenres&culture=${currentCulture}`,
            type: 'POST',
            contentType: 'application/json',
            data: function (d) {
                return JSON.stringify(d);
            }
        },
        columns: [
            { data: 'id' },
            { data: 'name' },
            {
                data: null,
                orderable: false, 
                searchable: false, 
                render: function (data, type, row) {
                    return `
                    <a class="btn btn-sm btn-warning" href="/Genres/AddOrEdit?id=${row.id}">Edit</a>
                    <a class="btn btn-sm btn-danger" href="#" onclick="remove(${row.id})">Delete</a>`;
                }
            }
        ],
    });

    $('#confirmDeleteBtn').on('click', function () {
        if (deleteId !== null) {
            fetch(`/Genres/Index?handler=DeleteGenre&id=${deleteId}`, {
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
    
});