function CreateWebSocket() {
    var webSocketURI = "wss:" + "//" + location.host + "/ws";

    socket = new WebSocket(webSocketURI);

    socket.onopen = function () {
        console.log("Connected.");
    };

    socket.onclose = function (event) {
        if (event.wasClean) {
            console.log('Disconnected.');
        } else {
            console.log('Connection lost.'); // for example if server processes is killed
        }
        console.log('Code: ' + event.code + '. Reason: ' + event.reason);
    };

    socket.onmessage = function (event) {
        console.log("Data received: " + event.data);
    };

    socket.onerror = function (error) {
        console.log("Error: " + error);
    };


    window.addEventListener("close", () => { socket.close() })
    window.addEventListener("beforeunload", () => { socket.close() })

}