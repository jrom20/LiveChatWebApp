var liveChatId;
var chatId;
var MAX_SIZEBUCKET = 50;

$(function () {

    "use strict";

    var connection = new signalR.HubConnectionBuilder().withUrl("/hubs/LiveChat").build();

    connection.on("receivedMessage", function (message) {
        insertNewMessage(message);
    });

    connection.on("loadMessages", function (messages, id) {
        messages.map(msg => {

            liveChatId = id;
            insertNewMessage(msg);

            $("#write_msg").removeAttr('disabled');
            $("#msg_send_btn").removeAttr('disabled');
        });
    });

    connection.start().then(function () {

        chatId = $("#chatId").val();
        connection.invoke("JoinPerson", chatId)
            .catch(function (err) {
            return console.error(err.toString());
        });

    }).catch(function (err) {
        return console.error(err.toString());
    });

    $("#write_msg").keydown(function (e) {

        if (e.keyCode == 13 || e.keyCode == 9) {

            this.value = $.trim(this.value);
            if (this.value) {

                let chatId = $("#chatId").val();

                let message = {
                    "ChatId": +chatId,
                    "Message": this.value,
                    "UserName": null,
                    "ReceiveTime": null
                };

                connection.invoke("SendMessage", message).catch(function (err) {
                    return console.error(err.toString());
                });

                this.value = '';
            }
            return false;

        }
    });

});

var Messages = [];

function insertNewMessage(activity) {

    Messages.push(activity);

    let fromMe = this.liveChatId == activity.messageFrom;

    let classMessageWrapper = fromMe ? "outgoing_msg" : "incoming_msg";
    let classMessage = fromMe ? "sent_msg" : "received_msg";

    let lastFiftyMessages = $("#msg_history").children();
    
    if (lastFiftyMessages.length > MAX_SIZEBUCKET) {
        lastFiftyMessages.first().remove();    
    }
    
    let messageTime = new Date(activity.receiveTime);
    let formatTime = messageTime.getMonth() + "/"
        + messageTime.getDate()
        + "/" + messageTime.getFullYear()
        + " " + messageTime.getHours() + ":" + messageTime.getMinutes();

    var html = `<div class="${classMessageWrapper}">
                    <div class="${classMessage}">
                        <p>${activity.message}</p>
                        <span class="time_date"> ${formatTime}    |    ${activity.userName}</span>
                    </div>
                </div>`;

    $("#msg_history").append(html);

    scrollToBottom('msg_history');
}


function scrollToBottom(id) {
    var div = document.getElementById(id);
    div.scrollTop = div.scrollHeight - div.clientHeight;
}
