$(document).ready(function () {
    $('#employeesTable').DataTable({
        serverSide: true,
        processing: true,
        ajax: {
            url: '/FilmEmployees/Index?handler=GetEmployees',
            type: 'POST',
            contentType: 'application/json',
            data: function (d) {
                console.log(d);
                return JSON.stringify(d);
            }
        },
        columns: [
            { data: 'id' },
            { data: 'name' }
        ],
    });
});
