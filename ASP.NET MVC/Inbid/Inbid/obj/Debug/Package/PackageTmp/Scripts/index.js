function callback(a, b, c, d) {
    setTimeout(function () {
        c.insertBefore(a, b);
        $('#value-' + d.RowId).text(' ' + d.Value);
        $('#row-' + d.RowId).show("fold", null, 1000, null);
        d.Do = false;
    }, 1000);
};


function runReplace(a, b, c, d) {
    //var row = document.getElementById('row-4');
    //var insertPos = document.getElementById('row-1');
    var p = d.Position;
    var c = a.parentNode;

    if (d.RowId + 1 != d.NextRowId) {
        $('#row-' + d.RowId).hide("blind", null, 1000, callback(a, b, c, d));


    }
}



function handleUpdate() {
    // Load and deserialize the returned JSON data
    var json = context.get_data();
    var data = Sys.Serialization.JavaScriptSerializer.deserialize(json);

    // Update the page elements
    if (d.RowId != d.NextRowId) {
        runReplace(row, insertPos, parent, data);
    }
}


function update() {
    $.post("/Offer/Update",
         function (data) {
             if (data.Do == true) {
                 var row = document.getElementById('row-' + data.RowId);
                 var insertPos = document.getElementById('row-' + data.NextRowId);

                 var parent = row.parentNode;
                 // var val = data.jResult.length;

                 //for (var i = 0; i < data.jResult.length; ++i) {
                 runReplace(row, insertPos, parent, data);
                 //}
             }

         });


};

function powtarzanie() {
    setInterval(update, 2800);

}


function timedRefresh() {
    setTimeout("location.reload(true);", 30000);
}


$(document).ready(function () {

    
    powtarzanie();
 //   timedRefresh();


});

