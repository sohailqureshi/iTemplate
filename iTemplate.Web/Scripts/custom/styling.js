(function ($) {

  $.fn.tableInit = function () {
    $("table").removeClass();
    $("table").addClass("table table-bordered table-striped table-condensed table-hover");
    $('table > thead > tr').addClass('success');
  };

  $.fn.populateList = function (results) {
    var $list = $(this);
    $list.empty();
    $list.append("<option value='-1' selected='true'>Please Select</option>");
    $.each(results, function (i, result) {
      $list.append("<option value='" + result.Value + "'>" + result.Text + "</option>");
    });
  }

  $.fn.resequenceElements = function () {

    var formGroupUpdateId = 0;
    $(this).each(function (i, obj) {

      $(obj).find("input, span").each(function(index, elem) {
        if ($(elem).attr('id')) {
          var id = $(elem).attr('id').replace(/[^\d]/g, '');
          var newId = $(elem).attr('id').replace(id, parseInt(formGroupUpdateId, 10));
          $(elem).attr('id', newId);
        }

        if ($(elem).attr('name')) {
          var curName = $(elem).attr('name').replace(/[^\d]/g, '');
          var newName = $(elem).attr('name').replace(curName, parseInt(formGroupUpdateId, 10));
          $(elem).attr('name', newName);
        }

        if ($(elem).attr('data-valmsg-for')) {
          var curDatavalFor = $(elem).attr('data-valmsg-for').replace(/[^\d]/g, '');
          var newDataValFor = $(elem).attr('data-valmsg-for').replace(curDatavalFor, parseInt(formGroupUpdateId, 10));
          $(elem).attr('data-valmsg-for', newDataValFor);
        }
      });
      formGroupUpdateId++;
    });
  }

})(jQuery);

$(function () {

  $('form input[type=text], form input[type=textarea], form input[type=password], form select').each(
      function (index) {
        var input = $(this);
        var found = ($(input).data('val-required'));
        if (found) {
          console.log($(this));
          $(this).addClass("required");
        }

        //alert('Type: ' + input.attr('type') + 'Name: ' + input.attr('name') + 'Value: ' + input.val());
      }
  );

  //$.each($("form").elements, function () {
  //  console.log($(this));
  //});

  //var $form = $(this).closest('form');
  //$form.each(function (i, obj) {
  //  obj.find('input, textarea').each(function (index, elem) {
  //    if (found) {
  //      $(elem).addClass("required");
  //    }
  //  })
  //});

  $(this).tableInit();
});
