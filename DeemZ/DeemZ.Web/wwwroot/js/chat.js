
var connection =
    new signalR.HubConnectionBuilder()
        .withUrl("/chat")
        .withAutomaticReconnect()
        .build();

connection.on("NewMessage",
    function (message) {

        var link = `${window.location.origin}/media/notification_sound.mp3`;
        var audio = new Audio(link);
        audio.play();

        var chatInfo = "";

        chatInfo += "<div class='media rounded text-white text-wrap' style='margin-bottom: 5px'>";
        chatInfo += "<div class='media-body'>";
        chatInfo += `<h3 class='mt-0'>${escapeHtml(message.text)}</h3>`;
        chatInfo += `<h5 class='mt-0'>${message.user}</h5>`;
        chatInfo += `<h5 class='mt-0'>Sent On: ${message.createdOn}</h5>`;
        chatInfo += "</div></div>";
        $("#messagesList").append(chatInfo);
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