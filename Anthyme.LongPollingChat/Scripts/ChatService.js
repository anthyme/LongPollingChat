(function (global, undefined) {

    //namespace Anthyme.LongPollingChat.ChatService
    var Anthyme = global.Anthyme = global.Anthyme || {};
    var LongPollingChat = Anthyme.LongPollingChat = Anthyme.LongPollingChat || {};
    var ChatService = LongPollingChat.ChatService = LongPollingChat.ChatService || {};


    var key = null;
    var runningUpdates = null;
    var ticks = 0;
    var genericFailed = function (rep) { debugger; };

    ChatService.SendMessage = function (callback, message) {
        $.ajax(
            {
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: "/ChatService.svc/SendMessage",
                data: JSON.stringify(
                    {
                        parameter: { Message: message, SessionKey: key }
                    }),
                success: callback,
                error: genericFailed
            }
        );
    };

    ChatService.CreateSession = function (callback, name) {
        $.ajax(
            {
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: "/ChatService.svc/CreateSession",
                data:
                    JSON.stringify(
                        {
                            parameter: { Name: name }
                        }
                    ),
                success: function (rep) {
                    key = rep.CreateSessionResult.Key;
                    callback(rep.CreateSessionResult);
                },
                error: genericFailed
            }
        );
    };

    ChatService.StartGetUpdates = function (callback) {
        getUpdates(callback);
    };

    var getUpdates = function (callback) {
        if (runningUpdates == null) {
            runningUpdates =
                $.ajax(
                    {
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        url: "/ChatService.svc/GetUpdates",
                        data: JSON.stringify(
                            {
                                parameter: { Ticks: ticks, SessionKey: key }
                            }
                        ),
                        success: function(rep) {
                            var newTicks = rep.GetUpdatesResult.Ticks;
                            if (newTicks != 0) ticks = newTicks;
                            runningUpdates = null;
                            callback(rep.GetUpdatesResult);
                            setTimeout(function() {
                                getUpdates(callback);
                            }, 50);
                        },
                        error: genericFailed
                    });
        }
    };
})(this);