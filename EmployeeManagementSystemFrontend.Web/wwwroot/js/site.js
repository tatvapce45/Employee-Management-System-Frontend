// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
function highlightDiv(divId) {
  var divId = document.getElementById(divId);
  divId.classList.add("highlighted");
}

function removeHighlightDiv(divId) {
  var divId = document.getElementById(divId);
  divId.classList.remove("highlighted");
}

function changeEyeIcon(passwordId, eyeIconOf) {
  var input = document.getElementById(passwordId);
  if (input.type === "password") {
    document.getElementById(passwordId).type = "text";
    document.getElementById(eyeIconOf).src = "/images/eye-fill.svg";
  } else if (input.type === "text") {
    document.getElementById(passwordId).type = "password";
    document.getElementById(eyeIconOf).src = "/images/eye-slash-fill.svg";
  }
}

function showUnauthorizedToaster(permissionName, permissionType) {
  toastr.error(
    "You don't have permission to " +
      permissionType +
      " " +
      permissionName +
      "!"
  );
}

$(document).ready(function () {
  var toastMessage = "@Model.ToastMessage";
  var messageType = "@Model.MessageType";

  if (toastMessage && messageType !== "") {
    toastr.options = {
      closeButton: true,
      progressBar: true,
      positionClass: "toast-top-right",
      timeOut: 5000,
    };

    switch (messageType) {
      case "success":
        toastr.success(toastMessage);
        break;
      case "error":
        toastr.error(toastMessage);
        break;
      case "warning":
        toastr.warning(toastMessage);
        break;
      case "info":
        toastr.info(toastMessage);
        break;
    }
  }
});

function loadConfirmationModal(message, work, id) {
  $.ajax({
    url: "/EmployeesAndDepartments/GetConfirmationModal",
    type: "GET",
    data: { confirmationMessage: message, confirmationWork: work, id: id },
    cache: false,
    success: function (response) {
      $("#layoutPageModalLoader").html(response);
      $("#confirmationModal").modal("show");
    },
  });
}
