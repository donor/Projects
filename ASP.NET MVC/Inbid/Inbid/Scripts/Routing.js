﻿/*!
* ASP.NET JavaScript Router
*
* Copyright 2010, Zack Owens
* http://weblogs.asp.net/zowens/
* http://wwww.eagleenvision.net/
*/

// ===============================================================================================
//
// WARNING: This file was generated by JsRouting.Output.exe and should NOT be overridden.
// When this file is compromised, the file will be overridden and changes will be lost.
//
// ===============================================================================================

// The following code is the jQuery pluggin that handles most of the routing logic. Note that jQuery is not required,
// but is used as the root namespace to prevent clutter of the global vars.

(function ($, undefined) {

    var constants = {
        beginAndEndTag: /([\{|\}])/g,
        groupParameters: /(?:{([a-zA-Z]+)})/g
    };

    // add any method to array prototype
    Array.prototype.any = function (val) {
        /// <summary>Determines if a value in the array object meets criterion</summary>
        /// <param name="val" mayBeNull="false" optional="false" type="Function" parameterArray="false" integer="false" domElement="false">Function or value indicating the criterion for acceptance of a value as being part of the array</param>
        /// <returns type="Boolean" integer="false" domElement="false" mayBeNull="false">Value indicating whether a value in the array meets criterion of function or is equal to the value passed to the function</returns>

        var pred = $.isFunction(val) ? val : function (p) { return p === val; };
        for (var i = 0; i < this.length; i++) {
            if (pred(this[i]) === true) {
                return true;
            }
        }
        return false;
    };

    var Router = function () {
        /// <summary>Object representing the route manager that handles the registration and dispatch of routes.</summary>

        this.controllers = {};

        this.routes = [];

        this.namedRoutes = {};

        this.constraintTypeDefs = {};

        this.getParameters = function (pattern) {
            /// <summary>Gets the parameters from a URL pattern.</summary>
            /// <param name="pattern" mayBeNull="false" optional="false" type="String" parameterArray="false" integer="false" domElement="false">URL pattern defined by a route.</param>
            /// <returns type="Array" integer="false" domElement="false" mayBeNull="false" elementType="String" elementInteger="false" elementDomElement="false" elementMayBeNull="false">All parameters of the route.</returns>

            var params = [];
            $.each(pattern.match(constants.groupParameters) || [], function (i, param) {
                params[i] = param.replace(constants.beginAndEndTag, '');
            });
            return params;
        };

        this.formatUrl = function (url, data, defaults) {
            /// <summary>Formats a URL based on the data</summary>
            /// <param name="url" mayBeNull="false" optional="false" type="String" parameterArray="false" integer="false" domElement="false">URL pattern defined by a route.</param>
            /// <param name="data" mayBeNull="true" optional="true" type="Object" parameterArray="false" integer="false" domElement="false">Data used to format the route by replacing parameters in the route pattern with data values</param>
            /// <param name="defaults" mayBeNull="true" optional="true" type="Object" parameterArray="false" integer="false" domElement="false">Route value defaults.</param>
            /// <returns type="String" integer="false" domElement="false" mayBeNull="false">Formatted URL</returns>

            // add prepended slash
            if (url.substr(0, 1) !== '/') {
                url = "/" + url;
            }

            // replace paramters in the URL
            $.each(this.getParameters(url).reverse(), function (i, v) {
                // check if the URL ends with the default value
                if (new RegExp('{' + v + '}$', '').test(url) && defaults && ((data[v] && data[v] === defaults[v]) || !data[v])) {
                    url = url.replace("/{" + v + "}", "");
                }
                else {
                    url = url.replace("{" + v + "}", data[v]);
                }
            });

            // remove trailing slash
            if (url.substring(url.length - 1, url.length) === '/') {
                url = url.substring(0, url.length - 1);
            }

            // add prepended slash
            if (url.substr(0, 1) !== '/') {
                url = "/" + url;
            }

            // remove extra route params... this is a catchall measure
            return url.replace(/({[a-zA-Z]*})/g, '').replace('//', '');
        };

        this.mapRoute = function (pattern, name, defaultValues) {
            /// <summary>Adds a route with specified pattern</summary>
            /// <param name="pattern" mayBeNull="false" optional="false" type="String" parameterArray="false" integer="false" domElement="false">URL pattern defined by a route.</param>
            /// <param name="name" mayBeNull="true" optional="true" type="String" parameterArray="false" integer="false" domElement="false">Route name</param>
            /// <param name="defaults" mayBeNull="true" optional="true" type="Object" parameterArray="false" integer="false" domElement="false">Route value defaults.</param>
            /// <returns type="Route" integer="false" domElement="false" mayBeNull="false">Route object</returns>

            var r = new Route(pattern);
            r.name = name || '';
            r.defaultValues = $.extend(defaultValues, {});

            // adds the route to the end of the routing list
            this.routes[this.routes.length] = r;

            // add to named routes
            if (r.name.length > 0) {
                this.namedRoutes[r.name] = r;
            }

            return r;
        };

        this.action = function (data) {
            /// <summary>Gets the route result for the given data values</summary>
            /// <param name="data" mayBeNull="false" optional="false" type="Object" parameterArray="false" integer="false" domElement="false">Data used to format the route by replacing parameters in the route pattern with data values</param>
            /// <returns type="RouteResult" integer="false" domElement="false" mayBeNull="false">Route value object with route URL</returns>

            for (var i = 0; i < this.routes.length; i++) {
                if (this.routes[i].accept(data)) {
                    return this.routes[i].toResult(data);
                }
            }
        };

        // WARNING: this method should not be used by your code!
        this.mapControllerAction = function (controller, action, fn) {
            /// <summary>Gets the route result for the given data values</summary>
            /// <param name="controller" mayBeNull="false" optional="false" type="String" parameterArray="false" integer="false" domElement="false">Controller name</param>
            /// <param name="controller" mayBeNull="false" optional="false" type="String" parameterArray="false" integer="false" domElement="false">Action name</param>
            /// <param name="fn" mayBeNull="false" optional="false" type="Function" parameterArray="false" integer="false" domElement="false">Function that returns a route result given controller action data</param>

            if (!this.controllers[controller]) {
                this.controllers[controller] = new Object();
            }

            if (this.controllers[controller][action]) {
                var startsWith = new RegExp('^' + action);
                var c = 0;
                $.each(this.controllers[controller], function (name, val) {
                    if (startsWith.test(name)) {
                        c++;
                    }
                });

                this.controllers[controller][action + c] = fn;
            }
            else {
                this.controllers[controller][action] = fn;
            }
        }

        this.namedAction = function (name, data) {
            /// <summary>Gets the route result for the given data values with a named route</summary>
            /// <param name="name" mayBeNull="false" optional="false" type="String" parameterArray="false" integer="false" domElement="false">Route name</param>
            /// <param name="data" mayBeNull="true" optional="true" type="Object" parameterArray="false" integer="false" domElement="false">Data used to format the route by replacing parameters in the route pattern with data values</param>
            /// <returns type="RouteResult" integer="false" domElement="false" mayBeNull="false">Route value object with route URL</returns>


            if (name && this.namedRoutes[name] && this.namedRoutes[name].accept(data)) {
                return this.namedRoutes[name].toResult(data);
            }
        };
    };

    var Route = function (pattern) {
        /// <summary>Route to store the pattern and values for the route defined by ASP.NET</summary>
        /// <param name="pattern" mayBeNull="false" optional="false" type="String" parameterArray="false" integer="false" domElement="false">URL pattern defined by a route.</param>

        var _this = this;

        this.constraints = [];

        this.pattern = pattern;
        this.name = '';
        this.defaultValues = {};

        // setup parameters in the route pattern
        this.params = $.routeManager.getParameters(this.pattern);

        this.accept = function (data) {
            /// <summary>Determines if the route matches the data</summary>
            /// <param name="data" mayBeNull="false" optional="false" type="Object" parameterArray="false" integer="false" domElement="false">Data used to format the route by replacing parameters in the route pattern with data values</param>
            /// <returns type="Boolean" integer="false" domElement="false" mayBeNull="false">Value indicating whether the route matches the data and can be used to form the URL</returns>

            return data.controller &&
                   data.action &&
                   (!this.params.any("area") || data.area) &&
                   (this.defaultValues.controller === data.controller || this.params.any("controller")) &&
                   (this.defaultValues.action === data.action || this.params.any("action")) &&
                   !this.constraints.any(function (c) { return !c(data); });
        };

        this.addConstraint = function (constraint) {
            /// <summary>Adds a constraint to the route.</summary>
            /// <param name="constraint" mayBeNull="false" optional="false" type="Function" parameterArray="false" integer="false" domElement="false">Function that returns true when the constraint is matched for the route</param>

            this.constraints.push(constraint);
        };

        this.toResult = function (data) {
            /// <summary>Forms the route result based on route data</summary>
            /// <param name="data" mayBeNull="true" optional="true" type="Object" parameterArray="false" integer="false" domElement="false">Data used to format the route by replacing parameters in the route pattern with data values</param>
            /// <returns type="RouteResult" integer="false" domElement="false" mayBeNull="false">Route value object with route URL</returns>

            return new Result(this, data);
        };
    };

    var Result = function (r, data) {
        /// <summary>Result of a route formation</summary>
        /// <param name="r" mayBeNull="false" optional="false" type="Rotue" parameterArray="false" integer="false" domElement="false">Route definition</param>
        /// <param name="data" mayBeNull="false" optional="false" type="Object" parameterArray="false" integer="false" domElement="false">Data used to format the route by replacing parameters in the route pattern with data values</param>

        // var url = $.routeManager.formatUrl(r.pattern, data, r.defaultValues);
        var url = $.routeManager.formatUrl(r.pattern, $.extend({}, r.defaultValues, data), r.defaultValues);
        this.toUrl = function () {
            /// <summary>Gets the URL</summary>
            /// <returns type="String" integer="false" domElement="false" mayByNull="false">URL for the request"</param>

            return url;
        };

        var getData = function (extra) {
            var ret = $.extend({}, extra);
            $.each($.extend({}, data, r.defaultValues), function (name, value) {
                if (!ret[name] && !r.params.any(name)) {
                    ret[name] = value;
                }
            });
            return ret;
        };

        this.post = function (data, success, dataType) {
            /// <summary>Performs a POST request</summary>
            /// <param name="data" mayBeNull="true" optional="true" type="Object" parameterArray="false" integer="false" domElement="false">Extra data values passed to the POST request</param>
            /// <param name="success" mayBeNull="true" optional="true" type="Function" parameterArray="false" integer="false" domElement="false">POST request success function</param>
            /// <param name="dataType" mayBeNull="true" optional="true" type="String" parameterArray="false" integer="false" domElement="false">POST request data type</param>

            $.post(this.toUrl(), getData(data), success, dataType);
        };

        this.get = function (data, success, dataType) {
            /// <summary>Performs a GET request</summary>
            /// <param name="data" mayBeNull="true" optional="true" type="Object" parameterArray="false" integer="false" domElement="false">Extra data values passed to the GET request</param>
            /// <param name="success" mayBeNull="true" optional="true" type="Function" parameterArray="false" integer="false" domElement="false">GET request success function</param>
            /// <param name="dataType" mayBeNull="true" optional="true" type="String" parameterArray="false" integer="false" domElement="false">GET request data type</param>

            $.get(this.toUrl(), getData(data), success, dataType);
        };
    };

    // expost on jQuery obj
    $.routeManager = new Router();

})(jQuery);
(function($, undefined){

$.routeManager.constraintTypeDefs.notEmpty = function(param){
                return function(data){
                    return data[param] && data[param].length > 0;
                };
            };
$.routeManager.mapRoute('{controller}/{action}/{id}', 'Default', {"controller":"Home","action":"Index","id":""});
$.routeManager.mapControllerAction('Home', 'Index', function(additionalData){
/// <summary></summary>
 return $.routeManager.action($.extend({controller:"Home", action:"Index"}, additionalData, {})); });
$.routeManager.mapControllerAction('Home', 'About', function(id, additionalData){
/// <summary></summary>
/// <param name="id" mayBeNull="false" optional="false" type="String"></param>
 return $.routeManager.action($.extend({controller:"Home", action:"About"}, additionalData, {id:id})); });
$.routeManager.mapControllerAction('Account', 'LogOn', function(additionalData){
/// <summary></summary>
 return $.routeManager.action($.extend({controller:"Account", action:"LogOn"}, additionalData, {})); });
$.routeManager.mapControllerAction('Account', 'LogOn', function(model, returnUrl, additionalData){
/// <summary></summary>
/// <param name="model" mayBeNull="false" optional="false" type="LogOnModel"></param>
/// <param name="returnUrl" mayBeNull="false" optional="false" type="String"></param>
 return $.routeManager.action($.extend({controller:"Account", action:"LogOn"}, additionalData, {model:model, returnUrl:returnUrl})); });
$.routeManager.mapControllerAction('Account', 'LogOff', function(additionalData){
/// <summary></summary>
 return $.routeManager.action($.extend({controller:"Account", action:"LogOff"}, additionalData, {})); });
$.routeManager.mapControllerAction('Account', 'Register', function(additionalData){
/// <summary></summary>
 return $.routeManager.action($.extend({controller:"Account", action:"Register"}, additionalData, {})); });
$.routeManager.mapControllerAction('Account', 'Register', function(model, additionalData){
/// <summary></summary>
/// <param name="model" mayBeNull="false" optional="false" type="RegisterModel"></param>
 return $.routeManager.action($.extend({controller:"Account", action:"Register"}, additionalData, {model:model})); });
$.routeManager.mapControllerAction('Account', 'ChangePassword', function(additionalData){
/// <summary></summary>
 return $.routeManager.action($.extend({controller:"Account", action:"ChangePassword"}, additionalData, {})); });
$.routeManager.mapControllerAction('Account', 'ChangePassword', function(model, additionalData){
/// <summary></summary>
/// <param name="model" mayBeNull="false" optional="false" type="ChangePasswordModel"></param>
 return $.routeManager.action($.extend({controller:"Account", action:"ChangePassword"}, additionalData, {model:model})); });
$.routeManager.mapControllerAction('Account', 'ChangePasswordSuccess', function(additionalData){
/// <summary></summary>
 return $.routeManager.action($.extend({controller:"Account", action:"ChangePasswordSuccess"}, additionalData, {})); });
})(jQuery);