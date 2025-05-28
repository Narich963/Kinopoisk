function initializeSelect2(url, selectClass) {
    const placeholder = $(selectClass).data('placeholder') || 'Select an option';
    $(selectClass).select2({
        ajax: {
            url: url,
            type: 'POST',
            contentType: 'application/json',
            data: function (params) {
                const pageSize = 5;
                let request = {
                    Draw: 1,
                    Start: ((params.page || 1) - 1) * pageSize,
                    Length: pageSize,
                    Search: {
                        Value: params.term || "",
                        regex: false
                    },
                    Order: [],
                    Columns: []
                };
                return JSON.stringify(request);
            },
            processResults: function (data, params) {
                params.page = params.page || 1;
                return {
                    results: data.data.map(function (item) {
                        return {
                            id: item.id,
                            text: item.name
                        };
                    }),
                    pagination: {
                        more: (params.page * 5) < data.recordsTotal
                    }
                };
            }
        },
        placeholder: placeholder,
        minimumInputLength: 1,
    });
}
function updateRolesBasedOnOrder() {
    const filmId = document.getElementById("filmId").value

    var selectedActors = [];
    var selectData = $('.actor-select').select2('data'); 

    $('.select2-selection__choice').each(function () {
        var actorName = $(this).attr('title');

        var match = selectData.find(d => d.text === actorName);
        if (match) {
            selectedActors.push(match.id);
        }
    });

    var container = $('#actorRolesContainer');
    container.empty();

    selectedActors.forEach(function (actorId, index) {
        container.append(`<input type="hidden" name="Film.Actors[${index}].FilmEmployeeId" value="${actorId}" />`);
        container.append(`<input type="hidden" name="Film.Actors[${index}].Role" value="${index + 1}" />`);
        container.append(`<input type="hidden" name="Film.Actors[${index}].FilmId" value="${filmId}" />`);
        container.append(`<input type="hidden" name="Film.Actors[${index}].IsDirector" value="false" />`);
    });
}
$(document).ready(function () {
    const actorUrl = '/FilmEmployees/Index?handler=GetEmployees';
    const actorClass = '.actor-select';
    initializeSelect2(actorUrl, actorClass);

    const countryUrl = '/Countries/Index?handler=GetCountries';
    const countryClass = '.country-select';
    initializeSelect2(countryUrl, countryClass);

    const genreUrl = '/Genres/Index?handler=GetGenres';
    const genreClass = '.genre-select';
    initializeSelect2(genreUrl, genreClass);

    $('.select2-selection__rendered').sortable({
        containment: 'parent',
        update: function () {
            updateRolesBasedOnOrder();
        }
    });

    updateRolesBasedOnOrder();

    $('#actorSelect').on('change', function () {
        updateRolesBasedOnOrder();
    });
});