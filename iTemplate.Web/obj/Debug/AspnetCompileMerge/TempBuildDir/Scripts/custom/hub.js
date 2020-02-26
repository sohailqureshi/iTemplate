var myHub;
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
  myHub = $.connection.serverHub;

  // Setting logging to true so that we can see whats happening in the browser console log. [OPTIONAL]
  $.connection.hub.logging = true;

  // Start the hub
  $.connection.hub.start();
});