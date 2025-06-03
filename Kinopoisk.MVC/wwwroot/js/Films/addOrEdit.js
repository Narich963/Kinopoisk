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
            const $select = $(this).closest('.select2-container').prev('select');

            let newOrder = [];
            $(this).children('.select2-selection__choice').each(function () {

                const title = $(this).attr('title');
                const option = $select.find('option').filter(function () {
                    return $(this).text() === title;
                }).first();

                if (option.length) {
                    newOrder.push(option.val());
                }
            });

            let selectedOptions = [];
            newOrder.forEach(id => {
                let option = $select.find(`option[value="${id}"]`);
                if (option.length) selectedOptions.push(option);
            });

            $select.find('option:selected').remove();

            selectedOptions.forEach(option => {
                option.prop('selected', true);
                $select.append(option);
            });

            $select.trigger('change');
        }
    });
});