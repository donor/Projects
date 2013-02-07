/// <reference path="Routing.js" />

function update(t) {

        $.post('Offer/Update/' + $("#tOferts").attr("aId"),
//    var url = $.routeManager
//    .action({ controller: "Offer", action: "Update/" + $("#tOferts").attr("aId") })
//    .toUrl();

    //$.post(url,
    function (data) {

        if (data != null) {

            var changes = JSON.parse(data);


            for (var i = 0; i < changes.length; i++) {

                var c = changes[i].NextRowId;
                var stop = false;
                for (var j = 0; j < changes.length; j++) {
                    if (c == changes[j].RowId) {
                        stop = true;
                        break;
                    }
                }

                if (!stop) {
                    //dodawanie wiersza
                    if (changes[i].Add == true) {

                        var id = changes[i].RowId;
                        var status = changes[i].IsOnline;
                        var online = changes[i].IsOnline;
                        var userName = changes[i].UserName;
                        var currentPrice = changes[i].Value;
                        var startValue = changes[i].StartValue;
                        var change = changes[i].Change;
                        var labelName = changes[i].LabelName;

                        var insertHtml = '<tr id="row-' + id + '" value="' + currentPrice + '" >' +
                            '<td id="row-' + id + '" >' +
                            online +
                            '<br />' +
                            '<img src="/Inbid/Content/OnlineStatus/' + online + '.png")"  alt="' + online + '"/>' +

                                            '</td >' +
                                            '<td>' +
                                            '<table  class="innerOfferts" >' +
                                            '<tr>' +
                                            '<td class="firstColumn" nowrap="nowrap">' +
                                            'Id: </td' +
                                            '<td class="secondColumn">' + id + '</td>' +
                                            '</tr>' +
                                            '<tr>' +
                                            '<td class="firstColumn" nowrap="nowrap">' +
                                            labelName +
                                            '</td>' +
                                            '<td class="secondColumn">' + userName + '</td>' +
                                            '</tr>' +
                                            '</table>' +
                                            '</td>' +
                                            '<td id="value-' + id + '">' +
                                            currentPrice +
                                            '</td>' +
                                            '<td>' +
                                            startValue +
                                            '</td>' +
                                            '<td id="change-' + id + '">' +
                                            change +
                                            '</td>' +
                                            '<td class="editRow">' +
                                            '</td>' +
                                            '</tr>';




                        if (changes[i].NextRowId == -1) {
                            // $(insertHtml).addClass('hiding');

                            $('#oferts >tbody').append(insertHtml);
                            // $("#row-"+changes[i].RowId).delay(10000).removeClass('addRow');
                        }
                        else {
                            //var insertPos = document.getElementById('row-' + changes[i].NextRowId);
                            var insertPos = $('#row-' + changes[i].NextRowId);


                            $(insertHtml).insertBefore(insertPos);
                        }
                        changes.splice(i, 1);
                        // i = -1;
                        i = changes.length == i ? -1 : --i;
                        continue;
                    }
                    //}


                    //sekcja do podmiany--------------------------------------------------------------//
                    if ((changes[i].Add == false) && (changes[i].Remove == false)) {

                        var row = $('#row-' + changes[i].RowId);  //document.getElementById('row-' + changes[i].RowId);

                        if (changes[i].NextRowId != -1)
                            var insertPos = $('#row-' + changes[i].NextRowId);
                        else
                            var insertPos = -1

                        // var parent = row.parentNode;

                        var rowId = changes[i].RowId;

                        var value = changes[i].Value;
                        var change = changes[i].Change;


                        // $(row).fadeTo('slow', 0.33, function () {

                        //var insertPos = $('#row-' + changes[i].NextRowId);
                        $('#value-' + rowId).text(value);
                        $('#change-' + rowId).text(change);


                        if (insertPos != -1) {
                            $(row).insertBefore(insertPos);
                            // $(row).fadeTo('slow', 1);
                        }
                        else {
                            $('#oferts >tbody').append(row);
                            //$(row).fadeTo('slow', 1);
                        }
                        changes.splice(i, 1);
                        i = changes.length == i ? -1 : --i;
                    }
                }
                var counter = 0;
                for (k = 0; k < changes.length; k++) {
                    if (changes[k].Remove == true)
                        counter += 1;
                }
                if (counter == changes.length || changes.length == 0)
                    break;
                //--------------------------------------------------------------------------------------//
            }

            //usuwania wiersza
            for (i = 0; i < changes.length; i++) {
                if (changes[i].Remove == true) {
                    var rowId = changes[i].RowId;
                    //   $('#row-' + rowId).fadeOut('slow', function () {
                    $('#row-' + rowId).remove();
                    // });
                }
            }
        }


    }

    )

    
    t += 5000;
    setTimeout(function () { update(t) }, t);
};
function getStatus(d) {
    $.post('Offer/UpdateStatus/' + $("#tOferts").attr("aId"),
    function (info) {
            if (info != null) {
                for (var i = 0; i < info.length; i++) {
                    var insert = info[i].State + ' <br /><img src="/Inbid/Content/OnlineStatus/' + info[i].State + '.png"  alt=' + info[i].State + '  />'

                    $('#status-' + info[i].RowId).html(insert);
                }
            }        
        
    });
    //   return false;
    d += 6000;
    setTimeout(function () { getStatus(d) }, d);
}

///////////////////////////////////////////////////////////////////////
/*
function Switch() {
$(".EnableDisable").click(function () {
var id = $(this).attr("auctionId");

var Value = $(this).attr("value");

if (Value == "True") {
$(this).text("Play");
$(this).attr("value", "False");
}
if (Value == "False") {
$(this).text("Pause");
$(this).attr("value", "True");
}

if (id != '') {
$.post("/Inbid/Offer/EnableDisableAuction/" + id,
function (data) {




});
}
});
}

function SetStatus(t) {
currentStatus = getAuctionStatus();
var id = $("#oferts tbody").attr("id");

if (id != '') {
$.post("/Inbid/Offer/SwitchAuction/" + id, currentStatus,
function (data) {
if (data != null) {
$(".AuctionStatus").attr("state", data.status);
}

});
}
t += 15000;
setTimeout(function () { Switch(t) }, t);
};

function getAuctionStatus() {
var currentStatus = $(".AuctionStatus").attr("state");
return { CurrentStatus: currentStatus };
}
*/
///////////////////////////////////////////////////////////////////////////
$(document).ready(function () {
    var time = 0;
    //var duration = 0;

    update(time);
    getStatus(time);

    // var period = 0;
    // SetStatus(period);
    //Switch();
});



