/// <reference path="jquery-1.6.2-vsdoc.js" />
/// <reference path="jquery-ui.js" />
$(document).ready(function () {
    //autocomplete Marka-----------
    $('*[data-autocomplete-url]')
        .each(function () {
            $(this).autocomplete({
                source: $(this).data("autocomplete-url")
            });
        });

    //DropDownList scripts----------
    $("#ddlGenders").change(function () {
        var idGender = $(this).val();
        $.getJSON("/Clothes/LoadTypesByGender", { id: idGender },
                    function (genderData) {
                        var select = $("#ddlTypes");
                        select.empty();
                        select.append($('<option/>', {
                            value: 0,
                            text: "Wybierz drugą"
                        }));

                        var select1 = $("#ddlSubTypes");
                        select1.empty();
                        select1.append($('<option/>', {
                            value: 0,
                            text: "Wybierz trzecią"
                        }));

                        select.attr("disabled", false);

                        $.each(genderData, function (index, itemData) {

                            select.append($('<option/>', {
                                value: itemData.Value,
                                text: itemData.Text
                            }));
                        });
                    });

    });
    $("#ddlTypes").change(function () {
        var idType = $(this).val();
        $.getJSON("/Clothes/LoadSubTypesByType", { id: idType },
                    function (modelData) {
                        var select = $("#ddlSubTypes");
                        select.empty();
                        select.append($('<option/>', {
                            value: 0,
                            text: "Wybierz trzecią"
                        }));

                        select.attr("disabled", false);
                        $.each(modelData, function (index, itemData) {

                            select.append($('<option/>', {
                                value: itemData.Value,
                                text: itemData.Text
                            }));
                        });
                    });
        //load sizes
        $.getJSON("/Clothes/LoadSizesByType", { id: idType },
                    function (genderData) {
                        var select = $("#ddlSizes");
                        select.empty();
                        select.append($('<option/>', {
                            value: 0,
                            text: "Wybierz rozmiar"
                        }));


                        select.attr("disabled", false);

                        $.each(genderData, function (index, itemData) {

                            select.append($('<option/>', {
                                value: itemData.Value,
                                text: itemData.Text
                            }));
                        });
                    });
        //end load sizes            
    });

    //scrypty do fileupload
    $('.progressbar').progressbar({ value: 0 });

    $('#frontUpload').fileupload({
        dataType: 'json',
        url: '/Upload/UploadHandler.ashx',
        progressall: function (e, data) {
            $(this).find('.progressbar').progressbar({ value: parseInt(data.loaded / data.total * 100, 10) });
        },
        done: function (e, data) {
            $('img:last-child', '#wrap_photo_front').remove();
            $('#wrap_photo_front').append('<img src="/files/' + data.result[0].name + '" />');
            $(this).find('.progressbar').progressbar({ value: 100 });
            $('#rot-front').css({ display: 'block' });
        }
    });
    $('#backUpload').fileupload({
        dataType: 'json',
        url: '/Upload/UploadHandler.ashx',
        progressall: function (e, data) {
            $(this).find('.progressbar').progressbar({ value: parseInt(data.loaded / data.total * 100, 10) });
        },
        done: function (e, data) {
            $('img:last-child', '#wrap_photo_back').remove();
            $('#wrap_photo_back').append('<img src="/files/' + data.result[0].name + '" />');
            $(this).find('.progressbar').progressbar({ value: 100 });
            $('#rot-back').css({ display: 'block' });
        }
    });
    $('#detailUpload0').fileupload({
        dataType: 'json',
        url: '/Upload/UploadHandler.ashx',
        progressall: function (e, data) {
            $(this).find('.progressbar').progressbar({ value: parseInt(data.loaded / data.total * 100, 10) });
        },
        done: function (e, data) {
            $('img:last-child', '#wrap_photo_detail0').remove();
            $('#wrap_photo_detail0').append('<img src="/files/' + data.result[0].name + '" />');
            $(this).find('.progressbar').progressbar({ value: 100 });
            $('#rot-detail0').css({ display: 'block' });
        }
    });
    $('#detailUpload1').fileupload({
        dataType: 'json',
        url: '/Upload/UploadHandler.ashx',
        progressall: function (e, data) {
            $(this).find('.progressbar').progressbar({ value: parseInt(data.loaded / data.total * 100, 10) });
        },
        done: function (e, data) {
            $('img:last-child', '#wrap_photo_detail1').remove();
            $('#wrap_photo_detail1').append('<img src="/files/' + data.result[0].name + '" />');
            $(this).find('.progressbar').progressbar({ value: 100 });
            $('#rot-detail1').css({ display: 'block' });
        }
    });

});

function remove_photo(sender) {
    var photoN = $('img:last-child', sender).attr("src").substring(7);

    $.ajax({
        type: 'delete',
        dataType: 'json',
        url: '/Upload/UploadHandler.ashx?f=' + photoN

    }).done(function () {
        var target = null;
        if (sender == '#frontUpload')
            target = '#rot-front';
        else if (sender == '#backUpload')
            target = '#rot-back';
        else if (sender == '#detailUpload0')
            target = '#rot-detail0';
        else if (sender == '#detailUpload1')
            target = '#rot-detail1';

        $('img:last-child', sender).remove();
        $(target).css({ display: 'none' });
        // alert("zdjęcie usuniete");
    });

}

function setFieldName(field) {
    var outField;
    switch (field) {
        case "Genders":
            outField = "Sex"; //zmaina 30-12-2012 11:03
            break;
        case "Types":
            outField = "TypeId";
            break;
        case "SubTypes":
            outField = "SubTypeId";
            break;
        case "Sizes":
            outField = "SizeId";
            break;
        case "Materials":
            outField = "MaterialId";
            break;
        case "Colors":
            outField = "ColorId";
            break;
        case "Patterns":
            outField = "PatternId";
            break;
        default: null;
    }
    return outField;
}

function take_photoNames() {
    var fileName;
    var photoF = $('img:last-child', '#frontUpload'); //.attr("src").substring(7);     
    if (photoF.length) {
        fileName = $('img:last-child', '#frontUpload').attr("src").substring(7);
        jQuery("#product-detail").append('<input type="hidden" name="PhotoNameF" value="' + fileName + '" />');
    }
    var photoB = $('img:last-child', '#backUpload');
    if (photoB.length) {
        fileName = $('img:last-child', '#backUpload').attr("src").substring(7);
        jQuery("#product-detail").append('<input type="hidden" name="PhotoNameB" value="' + fileName + '" />');
    }
    var photoD0 = $('img:last-child', '#detailUpload0');
    if (photoD0.length) {
        fileName = $('img:last-child', '#detailUpload0').attr("src").substring(7);
        jQuery("#product-detail").append('<input type="hidden" name="PhotoNameD0" value="' + fileName + '" />');
    }
    var photoD1 = $('img:last-child', '#detailUpload1');
    if (photoD1.length) {
        fileName = $('img:last-child', '#detailUpload1').attr("src").substring(7);
        jQuery("#product-detail").append('<input type="hidden" name="PhotoNameD1" value="' + fileName + '" />');
    }
}


function save_draft() {
    // jQuery('#product-detail').attr('action', '/listings/save_item/true');
    //Listing.validate_and_submit();
    //copy all info into lower form for submission...
    var info_form = document.getElementById("product-classify");
    var copy = info_form.getElementsByTagName("select");
    for (var c = 0; c < copy.length; c++) {
        //var field = copy[c].getAttribute("name");
        var field = setFieldName(copy[c].getAttribute("name"))
        var val = copy[c].value;
        if (val == 0) val = "";
        if (field == "Sex") {
            if (val == 0) val = false;
            if (val == 1) val = true;
            if (val == 2) val = null;
        }
        //else {
        jQuery("#product-detail").append('<input type="hidden" name="' + field + '" value="' + val + '" />');
        //}
    }
    var copy = info_form.getElementsByTagName("input");
    jQuery('#product-classify :radio:first,:checkbox').each(function () {
        var field = jQuery(this).attr("name");
        var val = jQuery(this).val();
        if (field == 'IsVintage' && !jQuery(this).attr('checked')) {
            val = false;
        } else if (field == 'IsVintage') {
            val = true;
        }

        if (field == 'Condition') {
            if (jQuery('#isNew').is(':checked')) {
                val = false;
            }
            else if (jQuery('#isUsed').is(':checked')) {
                val = true;
            }
            else val = false;
        }
        //$('input:radio[name=sex]:checked').val()

        jQuery("#product-detail").append('<input type="hidden" name="' + field + '" value="' + val + '" />');
    });
    if (jQuery('#BrandName').val() != '' && jQuery('#BrandName').val() != '(enter brand name)');
    jQuery("#product-detail").append('<input type="hidden" name="BrandName" value="' + jQuery('#BrandName').val() + '" />');
    take_photoNames();
    document.getElementById('product-detail').submit();
} 
                    
                
                

                