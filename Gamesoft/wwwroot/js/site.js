// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.
document.querySelectorAll(".addToFavoriteBtn").forEach(function(btn) {
    btn.addEventListener("click", function () {
        var productId = this.getAttribute("data-product-id");
        var btn = this;
        addToFavorite(productId, function (response) {
            if (response.success)
            {
                if (response.message === "added") {
                    var SearchValue = "Heart"
                    var ReplaceValue = "HeartFill"
                }
                else {
                    var SearchValue = "HeartFill"
                    var ReplaceValue = "Heart"
                }                
                btn.src = btn.src.replace(SearchValue, ReplaceValue);
                var score = document.getElementById("score_" + productId);
                if (response.message === "added") {
                    score.innerHTML = (parseInt(score.innerHTML) + 1).toString();
                }
                else
                {
                    score.innerHTML = (parseInt(score.innerHTML) - 1).toString();
                }
            }
            
        });
    });
});

function addToFavorite(productId, callback)
{
    var xhr = new XMLHttpRequest();
    xhr.open("POST", "/Products/AddToFavorite", true);
    xhr.setRequestHeader("Content-Type", "application/json;charset=UTF-8");
    xhr.onreadystatechange = function () {
        if (xhr.readyState === 4 && xhr.status === 200)
        {
            var response = JSON.parse(xhr.responseText);
            callback(response);
        }
    };
    xhr.send(JSON.stringify({productId: productId }));
}

$(document).ready(function () {
    $('#input-search').on('input', function () {
        var searchText = $(this).val();
        if (searchText.length >= 3) {
            ProductFilters();
        }        
    });
    $('#genrefilter').on('change', function () {
        ProductFilters();        
    });
    $('#statusfilter').on('change', function () {
        ProductFilters();
    });
});

function ProductFilters()
{
    var searchText = $('#input-search').val();
    var genrefilter = $('#genrefilter').val();
    var statusfilter = $('#statusfilter').val();

    $.ajax({
        url: '/Products/Search',
        type: 'GET',
        data: { searchedtext: searchText, genre: genrefilter, status: statusfilter },
        success: function (result) {
            $('#storeresults').html(result);
        },
        error: function (xhr, status, error) {
            console.error(xhr.responseText);
        }
    });
}

var socket = new WebSocket('ws://localhost:5000/newsHub');
socket.onmessage = function (event) {
    var newNews = JSON.parse(event.data);
    // Mettre à jour l'interface utilisateur pour afficher la nouvelle actualité
};