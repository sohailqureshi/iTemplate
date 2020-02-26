var myHub;
$(function () {

  // $('#mainDiv').center();
  $.fn.extend({
    center: function (parent) {
      if (parent) {
        parent = this.parent();
      } else {
        parent = window;
      }
      this.css({
        "position": "absolute",
        "top": ((($(parent).height() - this.outerHeight()) / 2) + $(parent).scrollTop() + "px"),
        "left": ((($(parent).width() - this.outerWidth()) / 2) + $(parent).scrollLeft() + "px")
      });
      return this;
    }
  });

  $('#bootstrap-growl').center();

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
    setTimeout(function () {
      $.bootstrapGrowl(message,
        {
          type: klass
        });
    }, 500);
  };

  // Defining a connection to the server hub.
  myHub = $.connection.serverHub;

  // Setting logging to true so that we can see whats happening in the browser console log. [OPTIONAL]
  $.connection.hub.logging = true;

  // Start the hub
  $.connection.hub.start();

  // Personal client method to broadcast the message
  myHub.client.personalMessage = function (message, name, connectionid) {
    if (connectionid == myHub.connection.id) {
      $("#message").text(message);
      successMessage(message);
    };
  };

  // System client method to broadcast the message for all users
  myHub.client.systemMessage = function (message, name, connectionid) {
    $("#message").text(message);
    successMessage(message);
  };
  
});