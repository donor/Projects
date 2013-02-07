﻿
;if (typeof String.prototype.trim !== 'function') {
    String.prototype.trim = function() {
        return this.replace(/^\s+|\s+$/g, '');
    };
}

(function($, _, ConsolR) {
    var root = this,
        connection,
    
        EndpointConnection = function(url, options) {
            /// <summary>
            /// A SignalR connection object that closes after a set period of time.</summary>
            /// <param name="url" type="String">
            /// The URL of the endpoint to connect to.</param>
            /// <param name="options" type="Object">
            /// Options for the connection.</param>
            /// <returns type="EndpointConnection" />
            
            var timer, 
                isConnected = false,
                conn = $.connection(url), 
                opts = _.defaults(options, {
                    timeout: 30000,
                    onReceived: $.noop
                });

            function onTimeout() {
                conn.stop();
                isConnected = false;
                timer = null;

                $('#footer').removeClass('loading');
            }

            conn.sending(function() {
                if (timer != null) {
                    root.clearTimeout(timer);
                }
                timer = root.setTimeout(onTimeout, opts.timeout);
            });

            conn.received(opts.onReceived);

            return {
                send: function (data) {
                    if (isConnected === true) {
                        conn.send(data);
                    }
                    else {
                        conn.start().done(function() { conn.send(data); });
                    }
                }
            };
        };
    
    function trackEvent(category, action, label) {
        /// <summary>
        /// Tracks an event in Google Analytics if it is initialized.</summary>
        
        var gaq = root._gaq;
        if (gaq && _.isFunction(gaq.push)) {
            gaq.push(['_trackEvent', category, action, label, , false]);
        }
    }
    
    function save() {
        /// <summary>
        /// Save content.</summary>
        
        $('form').submit();
        return false;
    }

    var markedErrors = [];

    function validate(command, classes) {
        /// <summary>
        /// Sends code to the server for validation and displays the resulting
        /// errors, if any.</summary>
        var pathname = window.location.pathname;
        
        trackEvent('Code', 'Validate', pathname);

        return $.ajax('/consolr/validate', {
            type: 'POST',
            contentType: 'application/json',
            data: JSON.stringify({ 'Command': command, 'Classes': classes }),
            success: function(msg) {
                var data = msg.data;
                
                for (var i = markedErrors.length - 1; i >= 0; i--) {
                    markedErrors[i].clear();
                }

                markedErrors.length = 0;

                if (_.isArray(data)) {
                    var $list = $('#footer .status ul.messages').detach().empty();

                    if (data.length == 0) {
                        $('#footer .status').removeClass('status-error').addClass('status-success');
                        $list.html("<li>No errors!</li>");
                        
                    } else {
                        $('#footer .status').removeClass('status-success').addClass('status-error');
                        
                        for (var index in msg.data) {
                            var error = msg.data[index];
                            var loc = error.Location;

                            var file = loc.FileName;

                            var start = loc.StartLinePosition;
                            var end = loc.EndLinePosition;

                            var markStart = { line: start.Line, ch: start.Character };
                            var markEnd = { line: end.Line, ch: end.Character };

                            var mark = ConsolR[file].markText(markStart, markEnd, 'compilation-error');

                            markedErrors.push(mark);

                            var message = 'Line: ' + (start.Line + 1) +
                                          ' Column: ' + start.Character + ' - ' + error.Message;

                            $list.append('<li data-errorId="' + index + '">' + _.escape(message) + '</li>');
                        }
                    }

                    $('#footer .status').append($list);
                }
            }
        });
    }

    function execute(command, classes) {
        /// <summary>
        /// Queues code for execution on the server.</summary>
        
        if (_.isString(command) && command.length > 0) {
            trackEvent('Code', 'Execute', window.location.pathname);
            connection.send(JSON.stringify({ 'Command': command, 'Classes': classes }));
            $('#footer:not(.loading)').addClass('loading');
        }
    }
    
    function setResult(data) {
        /// <summary>
        /// Sets the content displayed in the results section.</summary>

        $('#footer .results pre').html(data);
    }

    $(function() {
        //
        // Set up the SignalR connection
        //
        connection = new EndpointConnection('/consolr/execute', {
            onReceived: function (msg) {
                if (msg && msg.status === "ok") {
                    setResult(msg.data);
                }
                $('#footer').removeClass('loading');
            }
        });
        
        //
        // Set up CodeMirror editor
        //
        
        // Get the editor and save the current content so we can tell when it 
        // changes
        var editor = $('#define .editor textarea')[0],
            prompt = $('#execute .editor textarea')[0];

        var validateEnvironment = _.throttle(function() {
            var classes = ConsolR.Editor.getValue();
            var command = ConsolR.Prompt.getValue();

            validate(command, classes);
        }, 250);
        
        root.CodeMirror.commands.save = save;

        var editorOptions = {
            indentUnit: 4,
            lineNumbers: true,
            theme: 'neat',
            mode: 'text/x-csharp',
            onChange: function(cm, changes) {
                validateEnvironment();
            }
        };
        
        if (!editor || !prompt) {
            return;
        }
        
        ConsolR.Editor = root.CodeMirror.fromTextArea(editor, editorOptions);
        ConsolR.Editor.save = save;

        ConsolR.Prompt = root.CodeMirror.fromTextArea(prompt, editorOptions);
        ConsolR.Prompt.save = save;
        
        $('.js-save').on('click', save);

        $('.js-run').on('click', function() {
            var command = ConsolR.Prompt.getValue();
            var classes = ConsolR.Editor.getValue();
            
            execute(command, classes);

            $('.results pre').html('');
            
            return false;
        });
        
        //
        // Set up key binds
        //

        shortcut.add("Ctrl+B",function() {
            $(".js-run").click();
        });
        
        shortcut.add("Ctrl+S", save);

    });
}).call(window, window.jQuery, window._, window.ConsolR || {});
