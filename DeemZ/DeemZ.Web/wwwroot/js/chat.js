
var connection =
    new signalR.HubConnectionBuilder()
        .withUrl("/chat")
        .withAutomaticReconnect()
        .build();

connection.on("NewMessage",
    function (message) {

        var messageHtml = "";

        if (chatRoom.getCurrentUserId() == message.senderId) {
            messageHtml = `
                <div title="Send on: ${message.createdOn}" class="my-message-holder">
                    <div class="message text-white">
                        ${message.text}
                    </div>
                </div>
            `;
        }
        else {
            messageHtml = `
                <div title="Send on: ${message.createdOn}" class="message-holder flex">
                    <div>
                        <img class="small-img rounted-img" src="${message.senderImg}" />
                    </div>
                    <div class="flex flex-column">
                        <div class="username text-white">${message.senderName}</div>
                        <div class="message text-white">
                            ${message.text}
                        </div>
                    </div>
                </div>
            `;
            var link = `${window.location.origin}/media/notification_sound.mp3`;
            var audio = new Audio(link);
            audio.play();
        }

        $("#messagesList").append(messageHtml);
    });

$("form").submit(function (e) {
    e.preventDefault();
    var message = $("#messageInput").val();
    var groupId = $("#groupId").val();
    if (message !== "") {
        connection.invoke("Send", message, groupId); //Invokes controller method
        $("#messageInput").val("");

        $("#messageInput").focus();
        $("#messageInput").select();
    }
});

$("#addToAdminGroup").click(function () {
    alert("Added to Admin group");
    connection.invoke("AddToAdminGroup"); //Invokes controller method
});

$("#removeFromAdminGroup").click(function () {
    alert("Removed from Admin group");
    connection.invoke("RemoveFromAdminGroup"); //Invokes controller method
});

connection.start().then(function () {
    var groupId = $("#groupId").val();
    connection.invoke("OnConnect", groupId);
})
    .catch(function (err) {
        return console.error(err.toString());
    });

function escapeHtml(unsafe) {
    return unsafe
        .replace(/&/g, "&amp;")
        .replace(/</g, "&lt;")
        .replace(/>/g, "&gt;")
        .replace(/"/g, "&quot;")
        .replace(/'/g, "&#039;");
}