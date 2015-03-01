(function (global, undefined) {

    var ChatService = Anthyme.LongPollingChat.ChatService;

    var messageCount = 0;

    var initSession = function (name) {
        ChatService.CreateSession(
            function (r) {
                ChatService.StartGetUpdates(manageUpdates);
                $("#ChatBox").show(0.5);
                $("#MessageInput").focus();
            },
            name);
        $("#JoinGameDialog").hide(0.5);
    };

    var sendMessage = function () {
        var input = $("#MessageInput");
        var message = input.val();
        input.val("");
        ChatService.SendMessage(function (rep) {}, message);
        input.focus();
    };

    var manageUpdates = function (data) {
        if (data.Messages != null) {
            data.Messages.forEach(function (message) {
                $("#Messages").append(message.User.Name + " : " + message.Text + "<br/>");
                messageCount++;
                $("#RoomChat").scrollTop(20 * messageCount);
            });
        }
    };

    $(document).ready(function () {
        $("#ChatBox").hide();
        $("#LoginButton").click(function (e) {
            var userName = $('#LoginInput').val();
            $('#UserWelcome').append(userName);
            initSession(userName);
        });
        $("#SendButton").click(function (e) {
            sendMessage();
        });
    });

})(this)