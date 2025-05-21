$(document).ready(function () {
    $('#countriesTable').DataTable({
        serverSide: true,
        processing: true,
        ajax: {
            url: '/Countries/Index?handler=GetCountries',
            type: 'POST',
            contentType: 'application/json',
            data: function (d) {
                return JSON.stringify(d);
            },
        },
        columns: [
            { data: 'id', name: 'Id', autoWidth: true },
            { data: 'name', name: 'Name', autoWidth: true },
            {
                data: 'flag',
                searchable: false,
                orderable: false,
                render: function (data) {
                    return `<img src="${data}" style="margin-right:5px;">`;
                }
            }
        ]
    });
});