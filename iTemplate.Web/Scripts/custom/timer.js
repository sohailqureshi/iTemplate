$(function () {

  // This is the client method which is being called inside the MyHub constructor method every 3 seconds
  myHub.client.SendServerTime = function (serverTime) {
    // Set the received serverTime in the span to show in browser
    $("#newTime").text(serverTime);
  };

  //Button click jquery handler
  $("#btnClick").click(function () {
    // Call SignalR hub method
    myHub.server.clientPing();
  });
});