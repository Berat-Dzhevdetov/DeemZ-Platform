
var connection =
    new signalR.HubConnectionBuilder()
        .withUrl("/chat")
        .withAutomaticReconnect()
        .build();

connection.on("NewMessage",
    function (message) {
        var chatInfo = "";

        if (message.isDoctor) {
            chatInfo += "<div class='media bg-success rounded text-wrap' style='margin-bottom: 5px'>";
        }
        if (message.isAdmin && !message.isDoctor) {
            chatInfo += "<div class='media bg-warning rounded text-wrap' style='margin-bottom: 5px'>";
        }
        if (!message.isDoctor && !message.isAdmin) {
            chatInfo += "<div class='media bg-gray-600 rounded text-wrap' style='margin-bottom: 5px'>";
        }

        chatInfo += "<div class='media-body'>";
        chatInfo += `<h3 class='mt-0'>${escapeHtml(message.text)}</h3>`;
        chatInfo += `<h5 class='mt-0'>${message.user}</h5>`;
        chatInfo += `<h5 class='mt-0'>Изпратено на: ${message.createdOn}</h5>`;
        chatInfo += "</div></div>";
        $("#messagesList").append(chatInfo);
    });

$("#sendButton").click(function () {
    var message = $("#messageInput").val();
    if (message !== "") {
        connection.invoke("Send", message); //Invokes controller method
        $("#messageInput").val("");
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
        console.log("JS TEST ONCONNECT")
        connection.invoke("OnConnect");
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