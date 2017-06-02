var selectedList = "";
var sort = "x";
var sortAsc = false;
var page = 1;
var pageSquares = 1;
var perPage = 10;
var perPageSquares = 10;
var pages = 1;
var squaresTriggered = false;

$('#alert').hide();

var showAlert = function (title, message, isError) {

    if (isError) {
        var message = jQuery.parseJSON(message.responseText).error;
        $('#alert').removeClass('alert-success fade in').addClass('alert-danger fade in')
    } else {
        $('#alert').removeClass('alert-danger fade in').addClass('alert-success fade in')
    }

    $('#alert-title').text(title);
    $('#alert-message').text(message);
    $('#alert').show();

}

var loadLists = function (IsPostCreate) {
    $.getJSON("/point/getlists", null, function (data) {
        $("#point-lists option").remove();
        $.each(data, function (index, item) {
            $("#point-lists").append(
                $("<option></option>")
                    .text(item)
                    .val(item)
            );
        });
        if (IsPostCreate) {
            $('#point-lists').val(selectedList);
        }
        else {
            selectedList = $("#point-lists option:selected").text();
        }

        loadPoints();
    });
};

var loadPoints = function () {
    if (selectedList === "") {
        $("#point-table tbody tr").remove();
    }
    else {
        $.getJSON("/point/getlistpoints", { listName: selectedList, perPage: perPage, page: page, sort: sort, sortDir: (sortAsc ? "ASC" : "DESC") }, function (data) {
            $("#point-table tbody tr").remove();
            $("#total-points").text(data.totalEntries);
            $.each(data.entries, function (index, item) {
                $("#point-table tbody").append(
                    $("<tr class=\"coordinate-row\"><td>" + item.x + "</td><td>" + item.y + "</td><td id=\"" + item.id + "\">X</td></tr>")
                );
            });
            applyClickEvent();
            preparePaging(data);
            applyPagingEvents();
        });
    }

};

var preparePaging = function (data) {
    page = data.page;
    pages = Math.ceil(data.totalEntries / perPage);
    $('.page-number.point').remove();
    if (page == 1) {
        $('#backward').parent().addClass('disabled');
        $('#backward-full').parent().addClass('disabled');
    } else {
        $('#backward').parent().removeClass('disabled');
        $('#backward-full').parent().removeClass('disabled');
    }

    if (page == pages) {
        $('#forward').parent().addClass('disabled');
        $('#forward-full').parent().addClass('disabled');
    } else {
        $('#forward').parent().removeClass('disabled');
        $('#forward-full').parent().removeClass('disabled');
    }
    //       ,~~.     \MEOW/
    //      (  6 )-_, --\/
    // (\___ )=='-'
    //  \ .   ) )
    //   \ `-' /    
    //~'`~'`~'`~'`~
    for (var i = page + 2; i >= page - 2; i--) {
        if (i < 1) {
            continue;
        }
        if (i > pages)
            continue;

        $('#backward').parent().after('<li class="page-number point' + (i == page ? ' active' : '') + '" ><a class="page-number-inner">' + i + '</a></li>')
    }
};

var preparePagingSquares = function (data) {
    pageSquares = data.page;
    pagesSquares = Math.ceil(data.totalEntries / perPageSquares);
    $('.page-number.square').remove();
    if (pageSquares == 1) {
        $('#backward-square').parent().addClass('disabled');
        $('#backward-full-square').parent().addClass('disabled');
    } else {
        $('#backward-square').parent().removeClass('disabled');
        $('#backward-full-square').parent().removeClass('disabled');
    }

    if (pageSquares == pageSquares) {
        $('#forward-square').parent().addClass('disabled');
        $('#forward-full'.square).parent().addClass('disabled');
    } else {
        $('#forward-square').parent().removeClass('disabled');
        $('#forward-full-square').parent().removeClass('disabled');
    }
    //       ,~~.     \MEOW/
    //      (  6 )-_, --\/
    // (\___ )=='-'
    //  \ .   ) )
    //   \ `-' /    
    //~'`~'`~'`~'`~
    for (var i = pageSquares + 2; i >= pageSquares - 2; i--) {
        if (i < 1) {
            continue;
        }
        if (i > pagesSquares)
            continue;

        $('#backward-square').parent().after('<li class="page-number square' + (i == pageSquares ? ' active' : '') + '" ><a class="page-number-inner square">' + i + '</a></li>')
    }
};

var deletePoint = function (id) {
    $.post("/point/delete", { listName: selectedList, id: id }, function () {
    }).done(function () {
        showAlert('Success!', 'Point removed', false);
        loadPoints();
    }).fail(function (error) {
        showAlert('Error!', error, true);
    });
}

var addPoint = function (point) {
    if ($('#new-point-form').valid()) {
        $.post("/point/add", { listName: selectedList, point: point }, function () {
        }).done(function () {
            showAlert('Success!', 'Point added', false);
            loadPoints();
            $('.point-validation-summary .validation-summary-errors ul li').remove();
        }).fail(function (error) {
            showAlert('Error!', error, true);
        });
    }

}

var clearList = function () {
    $.post("/point/clearlist", { listName: selectedList }, function () {
    }).done(function () {
        showAlert('Success!', 'List cleared!', false);
        loadPoints();
    }).fail(function (error) {
        showAlert('Error!', error, true);
    });
}

var deleteList = function () {
    $.post("/point/deleteList", { listName: selectedList }, function () {
    }).done(function () {
        showAlert('Success!', 'List deleted!', false);
        loadLists();
    }).fail(function (error) {
        showAlert('Error!', error, true);
    });
}

var createList = function () {
    if ($('#new-list-form').valid()) {
        $.post("/point/createlist", { listName: $('#new-list-name').val() }, function () {
        }).done(function () {
            showAlert('Success!', 'List created!', false);
            loadLists(true);
            selectedList = $('#new-list-name').val();
            $('#new-list-name').val("");
            $('.list-validation-summary .validation-summary-errors ul li').remove();
        }).fail(function (error) {
            showAlert('Error!', error, true);
        });
    }
}

var findSquares = function () {
    $.getJSON("/point/findsquares", { listName: selectedList, perPage: perPageSquares, page: pageSquares }, function (data) {
    }).done(function (data) {
        $("#squares-table tbody tr").remove();
        $("#total-squares").text(data.totalEntries);
        $.each(data.result, function (index, item) {
            var toAppend = "<tr><td>" + item.key + "</td>";
            $.each(item.value, function (index, item) {
                toAppend += "<td>" + item.x + ", " + item.y + "</td>"
            })
            toAppend += "</tr>";
            $("#squares-table tbody").append(toAppend);
            squaresTriggered = true;
            preparePagingSquares(data);
            applyPagingSquaresEvents();
        });
    }).fail(function (error) {
        showAlert('Error!', error, true);
    })
}

$('#squares-button').click(function () {
    findSquares();
});

$("#point-lists-button").click(function () {
    loadLists();
});

$("#delete-list-button").click(function () {
    deleteList();
});

$("#download-list-button").click(function () {
    var url = "/point/download?listName=" + selectedList;
    $.get("/point/download", { listName: selectedList }, function () {
    }).done(function () {
        window.location.href = url;
    }).fail(function (error) {
        showAlert('Error!', error, true);
    });

});

$("#clear-list-button").click(function () {
    clearList();
});

$("#new-list-button").click(function () {
    createList();
});

var applyClickEvent = function () {
    $('.coordinate-row td:last-child').click(function (event) {
        deletePoint(event.target.id);
    });
}

var applyPagingEvents = function () {
    $('.page-number-inner').click(function () {
        page = $(this).text();
        loadPoints();
    });
}

var applyPagingSquaresEvents = function () {
    $('.page-number-inner.square').click(function () {
        pageSquares = $(this).text();
        findSquares();
    });
}

$("#load-points-button").click(function () {
    loadPoints();
});

$("#point-lists").change(function () {
    selectedList = $("#point-lists option:selected").text();
    squaresTriggered = false;
    $("#squares-table tbody tr").remove();
    loadPoints();
});

$("#paging-points").change(function () {
    perPage = $("#paging-points option:selected").text().trim();
    loadPoints();
});

$("#paging-squares").change(function () {
    perPageSquares = $("#paging-squares option:selected").text().trim();
    if (squaresTriggered)
        findSquares();
});

$('.alert .close').on('click', function (e) {
    $(this).parent().hide();
});

$('#new-entry-button').click(function () {
    var point = {
        X: $('#X').val(),
        Y: $('#Y').val(),
    };
    addPoint(point);
});

$("form#import").submit(function () {

    var formData = new FormData($(this)[0]);

    if (formData.getAll('list')[0] == "current") {
        formData.set('listName', selectedList);
    }
    $.ajax({
        url: '/point/import',
        type: 'POST',
        data: formData,
        async: false,
        success: function (data) {
            showAlert("Success!", "Data uploaded! Total lines in file: " + data.totalLinesCount + ", Inserted: " + data.insertedCount + ", Duplicates: " + data.duplicatesCount + ", Lines with errors: " + data.badLinesCount + ", Was point limit reached: " + (data.isLimitReached ? "Yes" : "No"), false);
            selectedList = formData.getAll('listName')[0];
            loadLists(true);
        },
        error: function (data) {
            showAlert("Error!", data, true);
        },
        cache: false,
        contentType: false,
        processData: false
    });

    $('#import-modal').modal('toggle');

    return false;
});

$('#filter-x').click(function () {
    changeFilter('x')
});

$('#filter-y').click(function () {
    changeFilter('y')
});

$('#forward').click(function () {
    if (!(page + 1 > pages))
        page++;
    loadPoints();
});

$('#backward').click(function () {
    if ((page - 1 >= 1))
        page--;
    loadPoints();
});

$('#forward-full').click(function () {
    page = pages;
    loadPoints();
});

$('#backward-full').click(function () {
    page = 1;
    loadPoints();
});

$('#forward-square').click(function () {
    console.log('a');
    if (!(pageSquares + 1 > pagesSquares))
        pageSquares++;
    if (squaresTriggered)
        findSquares();
});

$('#backward-square').click(function () {
    if ((pageSquares - 1 >= 1))
        pageSquares--;
    if (squaresTriggered)
        findSquares();
});

$('#forward-full-square').click(function () {
    pageSquares = pagesSquares;
    if (squaresTriggered)
        findSquares();
});

$('#backward-full-square').click(function () {
    pageSquares = 1;
    if (squaresTriggered)
        findSquares();
});

var changeFilter = function (filter) {
    if (filter === sort) {
        sortAsc = !sortAsc;
        $('#filter-' + filter + '-first').toggleClass('dropup');
    }
    else {
        $('#filter-' + sort + '-first').removeClass('dropup');
        $('#filter-' + sort + '-second').removeClass('caret');
        sort = filter;
        sortAsc = false;
        $('#filter-' + filter + '-second').addClass('caret');
    }
    loadPoints();
}

loadLists();
