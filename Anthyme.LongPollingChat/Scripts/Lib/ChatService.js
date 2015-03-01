(function (global, undefined) {

    // Anthyme.LongPollingChat.ChatService

    //    var Anthyme = global.Anthyme = global.Anthyme || {};
    //    var LongPollingChat = Anthyme.LongPollingChat = Anthyme.LongPollingChat || {};
    //    var ChatService = LongPollingChat.ChatService = LongPollingChat.ChatService || {};

    //    global.Anthyme = global.Anthyme || {};
    //    global.Anthyme.LongPollingChat = global.Anthyme.LongPollingChat || {};
    //    global.Anthyme.LongPollingChat.ChatService = global.Anthyme.LongPollingChat.ChatService || {};

    //    var ChatService = global.Anthyme.LongPollingChat.ChatService;

    var ChatService = namespace("Anthyme.LongPollingChat.ChatService");

    ChatService.key = null;
    ChatService.runningUpdates = null;
    ChatService.ticks = 0;
    ChatService.GenericFailed = function (rep) { debugger; };

    ChatService.SendMessage = function (callback, message) {
        $.ajax(
            {
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: "/ChatService.svc/SendMessage",
                data: JSON.stringify(
                    {
                        parameter: { Message: message, SessionKey: ChatService.key }
                    }),
                success: callback,
                error: ChatService.GenericFailed
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
                    ChatService.key = rep.CreateSessionResult.Key;
                    callback(rep.CreateSessionResult);
                },
                error: ChatService.GenericFailed
            }
        );
    };

    ChatService.StartGetUpdates = function (callback) {
        ChatService._getUpdates(callback);
    };

    ChatService._getUpdates = function (callback) {
        if (ChatService.runningUpdates == null) {
            ChatService.runningUpdates =
                $.ajax(
                    {
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        url: "/ChatService.svc/GetUpdates",
                        data: JSON.stringify(
                            {
                                parameter: { Ticks: ChatService.ticks, SessionKey: ChatService.key }
                            }
                        ),
                        success: function (rep) {
                            var newTicks = rep.GetUpdatesResult.Ticks;
                            if (newTicks != 0) ChatService.ticks = newTicks;
                            ChatService.runningUpdates = null;
                            callback(rep.GetUpdatesResult);
                            setTimeout(function () {
                                ChatService._getUpdates(callback);
                            }, 50);
                        },
                        error: ChatService.GenericFailed
                    });
        }
    };
})(this);