var hub;
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

  hub = $.connection.serverHub;
  //$.connection.hub.logging = true;
  $.connection.hub.start().done(init);

  function init() {

    $.connection.hub.reconnecting(function () {
      infoMessage('Connection issues... reconnecting');
    });

    $.connection.hub.error(function () {
      console.log('An error occurred...');
    });

    $.connection.hub.reconnected(function () {
      successMessage('Connection repaired');
    });

    $.connection.hub.disconnected(function () {
      errorMessage('Connection lost');
    });

  }

});
