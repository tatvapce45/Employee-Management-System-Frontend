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


