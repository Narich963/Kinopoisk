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
        items: '.select2-selection__choice',
        tolerance: 'pointer',
        update: function () {
            const $select = $(this).closest('.select2-container').prev('select');

            const newOrder = $(this).children('.select2-selection__choice').map(function () {
                const title = $(this).attr('title');
                const matchingOption = $select.find('option').filter(function () {
                    return $(this).text() === title;
                });
                return matchingOption.val();
            }).get();

            const options = $select.find('option');
            newOrder.forEach(val => {
                const option = options.filter(`[value="${val}"]`);
                if (option.length) {
                    $select.append(option);
                }
            });

            $select.trigger('change');
        }
    });
});