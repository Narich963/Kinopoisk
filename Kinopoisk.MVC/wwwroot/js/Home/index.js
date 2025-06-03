$(document).ready(function () {
    loadTopFilms();
});

function loadTopFilms() {
    $.ajax({
        url: '/Home/Index?handler=GetTopFilmsByRate',
        method: 'GET',
        success: function (data) {
            renderFilms(data.rateFilms, 'top-rated-films');
        }
    });

    $.ajax({
        url: '/Home/Index?handler=GetTopFilmsByDate',
        method: 'GET',
        success: function (data) {
            renderFilms(data.dateFilms, 'top-newest-films');
        }
    });
}

function renderFilms(films, containerId) {
    const $container = $('#' + containerId);
    $container.empty();

    films.forEach(film => {
        const card = `
                    <div class="col">
                        <div class="card h-100">
                            <img src="${film.poster || 'placeholder.jpg'}" class="card-img-top" alt="${film.name}">
                            <div class="card-body">
                                <h5 class="card-title">${film.name}</h5>
                                <p class="card-text">Rating: ${film.sitesRating}</p>
                                <p class="card-text"><small class="text-muted">Published: ${new Date(film.publishDate).toLocaleDateString()}</small></p>
                            </div>
                        </div>
                    </div>
                `;
        $container.append(card);
    });
}