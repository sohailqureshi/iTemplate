$(function () {

  warningMessage = function (message) {
    infoMessage(message, 'warning');
  };
  errorMessage = function (message) {
    infoMessage(message, 'error');
  };
  successMessage = function (message) {
    infoMessage(message, 'success');
  };
  infoMessage = function (message, klass) {
    klass = (klass == undefined) ? 'success' : klass;
    setTimeout(function () { $.bootstrapGrowl(message, { type: klass }); }, 500);
  };

  // Defining a connection to the server hub.
  var myHub = $.connection.serverHub;

  // Setting logging to true so that we can see whats happening in the browser console log. [OPTIONAL]
  //$.connection.hub.logging = true;

  // Start the hub
  $.connection.hub.start();

  // This is the client method which is being called inside the MyHub constructor method every 3 seconds
  myHub.client.SendServerTime = function (serverTime) {
    // Set the received serverTime in the span to show in browser
    $("#newTime").text(serverTime);
  };

  // Client method to broadcast the message
  myHub.client.broadcastMessage = function (message) {
    $("#message").text(message);
    successMessage(message);
  };

  //Button click jquery handler
  $("#btnClick").click(function () {
    // Call SignalR hub method
    myHub.server.clientPing();
  });

});