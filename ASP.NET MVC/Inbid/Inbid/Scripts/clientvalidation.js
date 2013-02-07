/// <reference path="jquery-1.5.2-vsdoc.js" />
/// <reference path="jquery.validate-vsdoc.js" />
/// <reference path="jquery.validate.unobtrusive.js" />

//value - Property that is being validated
//param - Html Element of Property against which 'value' is being validated
/*
jQuery.validator.addMethod("greaterthan", function (value, element, param) {
    return Date.parse(value) > Date.parse($(param).val());
});


jQuery.validator.unobtrusive.adapters.add("greaterthan", ["otherfield"], function (options) {
    options.rules["greaterthan"] = "#" + options.params.otherfield;
    options.messages["greaterthan"] = options.message;
});

*/