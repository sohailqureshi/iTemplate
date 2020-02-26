$(function () {

  var startTime = "";
  var timeCurrent = "";
  $("#Submit").click(function () {
    var startTime = timeCurrent;
  });

  $(".filesProcessed").html("");
  hub.client.updateStatus = function (message) {
    $(".serverTime").html(serverTime - startTime);
    $(".filesProcessed").html("Files Processed: " + String(message));
  };

  hub.client.serverTime =  function (serverTime) {
    timeCurrent = serverTime;
  };

});
