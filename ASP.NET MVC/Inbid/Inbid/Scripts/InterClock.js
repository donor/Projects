/// <reference path="jquery-1.4.4.min.js" />


$(document).ready(function () {
    
        // Display date for each culture
        var today = new Date();
        var results = "";
        $.each($.cultures, function (i, culture) {
            results += "<li>"
            + culture.englishName + " = "
            + $.format(today, "D", culture.name)
            + "</li>";
        });
        $("#results").html(results);


    
});
//<ul id="results">  </ul>