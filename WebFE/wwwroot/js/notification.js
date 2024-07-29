"use strict";

var connection = new signalR.HubConnectionBuilder()
    .withUrl("https://localhost:7052/notificationHub", {
        skipNegotiation: true,
        transport: signalR.HttpTransportType.WebSockets
    })
    .build();

connection.start().then(function () {
    console.log('connected to hub');
}).catch(function (err) {
    return console.error(err.toString());
});

connection.on("OnConnected", function () {
    console.log('OnConnected');
    OnConnected();
});

function OnConnected() {
    var userId = $('#hfUserId').val();
    console.log(userId);
    connection.invoke("SaveUserConnection", userId).catch(function (err) {
        return console.error(err.toString());
    })
}

connection.on("ReceivedNotification", function (message) {
    DisplayGeneralNotification(message, 'General Message');
});

connection.on("ReceivedPersonalNotification", function (message, userId) {
    DisplayPersonalNotification(message, 'From ' + userId);
});

$(document).ready(function () {
    var userId = $('#hfUserId').val();

    if (!connection && userId != null) {
        connection.start().then(function () {
            console.log('Reconnected to hub');
        }).catch(function (err) {
            return console.error(err.toString());
        });
    }

    // Add an event listener for logout
    $("#logoutButton").click(function () {
        console.log("Đã click vô");
        connection.stop().then(function () {
            console.log('disconnected from hub');
            // Perform logout action here
            $.post("/Account/Logout", function () {
                window.location.href = "/Account/Login";
            });
        }).catch(function (err) {
            return console.error(err.toString());
        });
    });
});
