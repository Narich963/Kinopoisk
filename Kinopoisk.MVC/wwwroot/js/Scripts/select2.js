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