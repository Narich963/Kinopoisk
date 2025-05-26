function initializeSelect2(url, selectClass, placeholder) {
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
function addAndRemoveGenre(genreIndex, genreUrl, genreClass, placeholder) {
    $('#addGenreBtn').on('click', function () {
        const newGenreHtml = `
            <div class="row genre-entry mb-2" data-index="${genreIndex}">
                <div class="col-md-10">
                    <select name="Film.Genres[${genreIndex}].GenreID" class="form-select genre-select" data-index="${genreIndex}"></select>
                </div>
                <input type="hidden" name="Film.Genres[i].Name" value="@Model.Film.Genres[i].Name"/>

                <div class="col-md-2">
                    <button type="button" class="btn btn-sm btn-danger remove-genre-btn">−</button>
                </div>
            </div>`;
        $('#genresContainer').append(newGenreHtml);
        initializeSelect2(genreUrl, genreClass, placeholder);
        genreIndex++;
    });

    $('#genresContainer').on('click', '.remove-genre-btn', function () {
        const entry = $(this).closest('.genre-entry')
        entry.find('input[name$=".IsForDeleting"]').val('true');
        entry.hide();
    });
}
function addAndRemoveActor(actorIndex, actorUrl, actorClass, placeholder) {
    $('#addActorBtn').on('click', function () {
        const newActorHtml = `
            <div class="row actor-entry mb-2" data-index="${actorIndex}">
                <div class="col-md-6">
                    <select name="Film.Actors[${actorIndex}].FilmEmployeeID" class="form-select actor-select" data-index="${actorIndex}"></select>
                </div>
                <div class="col-md-4">
                    <input type="number" name="Film.Actors[${actorIndex}].Role" class="form-control form-control-sm" placeholder="Role importance" required />
                </div>
                <input type="hidden" name="Film.Actors[${actorIndex}].IsDirector" value="false"/>
                <div class="col-md-2">
                    <button type="button" class="btn btn-sm btn-danger remove-actor-btn">−</button>
                </div>
            </div>`;

        $('#actorsContainer').append(newActorHtml);
        initializeSelect2(actorUrl, actorClass, placeholder);
        actorIndex++;
    });
    $('#actorsContainer').on('click', '.remove-actor-btn', function () {
        const entry = $(this).closest('.actor-entry');
        entry.find('input[name$=".IsForDeleting"]').val('true');
        entry.hide();
    });
}
$(document).ready(function () {
    const actorUrl = '/FilmEmployees/Index?handler=GetEmployees';
    const actorClass = '.actor-select';
    const actorPlaceholder = 'Select an actor';
    initializeSelect2(actorUrl, actorClass, actorPlaceholder);

    let actorIndex = parseInt(document.getElementById("actorsContainer").dataset.actorIndex) || 0;
    addAndRemoveActor(actorIndex, actorUrl, actorClass, actorPlaceholder);

    const countryUrl = '/Countries/Index?handler=GetCountries';
    const countryClass = '.country-select';
    const countryPlaceholder = 'Select a country';
    initializeSelect2(countryUrl, countryClass, countryPlaceholder);

    const genreUrl = '/Genres/Index?handler=GetGenres';
    const genreClass = '.genre-select';
    const genrePlaceholder = 'Select a genre';
    initializeSelect2(genreUrl, genreClass, genrePlaceholder);

    let genreIndex = parseInt(document.getElementById("genresContainer").dataset.genreIndex) || 0;
    addAndRemoveGenre(genreIndex, genreUrl, genreClass, genrePlaceholder);

});