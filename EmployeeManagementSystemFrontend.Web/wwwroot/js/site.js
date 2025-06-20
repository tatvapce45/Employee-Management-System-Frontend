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

let shakeInterval;
const bell = document.getElementById("notificationBell");
const bellWrapper = document.getElementById("notificationBellWrapper");
const notificationDot = document.getElementById("notificationDot");
const notificationPanel = document.getElementById("notificationPanel");
const notificationList = document.getElementById("notificationList");
const markAllReadButton = document.getElementById("markAllRead");
const noNotifications = document.getElementById("noNotifications");

function getNotifications() {
  return JSON.parse(localStorage.getItem("notifications") || "[]");
}

function saveNotifications(data) {
  localStorage.setItem("notifications", JSON.stringify(data));
}

function addNotification(message) {
  const data = getNotifications();
  const newNotif = {
    id: Date.now(),
    message,
    read: false,
  };
  data.unshift(newNotif);
  saveNotifications(data);
  loadNotifications();
}

function loadNotifications() {
  const data = getNotifications(); 
  notificationList.innerHTML = "";

  if (data.length === 0) {
    noNotifications.classList.remove("d-none");
    markAllReadButton.disabled = true; 
    return;
  }

  noNotifications.classList.add("d-none");

  let hasUnread = false;

  data.forEach((notif) => {
    const li = document.createElement("li");
    li.className = `list-group-item notification-item ${
      notif.read ? "" : "unread"
    }`;
    li.textContent = notif.message;
    li.dataset.id = notif.id;
    notificationList.appendChild(li);

    if (!notif.read) {
      hasUnread = true;
    }
  });

  if (hasUnread) {
    notificationDot.classList.remove("d-none");
    startShakingBell();
    markAllReadButton.disabled = false; 
  } else {
    notificationDot.classList.add("d-none");
    stopShakingBell();
    markAllReadButton.disabled = true;
  }
}

function startShakingBell() {
  stopShakingBell();
  shakeInterval = setInterval(() => {
    bell.classList.add("shake");
    setTimeout(() => bell.classList.remove("shake"), 400);
  }, 1000);
}

function stopShakingBell() {
  clearInterval(shakeInterval);
  bell.classList.remove("shake");
}

markAllReadButton.addEventListener("click", () => {
  const data = getNotifications();
  const updated = data.map((n) => ({ ...n, read: true }));
  saveNotifications(updated);
  localStorage.removeItem("notifications");
  loadNotifications();
});

bellWrapper.addEventListener("click", () => {
  notificationPanel.style.display =
    notificationPanel.style.display === "none" ||
    notificationPanel.style.display === ""
      ? "block"
      : "none";
});

window.addEventListener("load", () => {
  loadNotifications();
});

function openNavbar() {
  document.getElementById("mySideBar").style.width = "250px";
  document.getElementById("main").style.marginLeft = "250px";
  document.getElementById("menuToggleIcon").style.display = "none";
}

function closeNavbar() {
  document.getElementById("mySideBar").style.width = "0";
  document.getElementById("main").style.marginLeft = "0";
  document.getElementById("menuToggleIcon").style.display = "block";
}
toastr.options = {
  closeButton: true,
  debug: false,
  newestOnTop: true,
  progressBar: true,
  positionClass: "toast-top-right",
  preventDuplicates: true,
  onclick: null,
  showDuration: "300",
  hideDuration: "1000",
  timeOut: "5000",
  extendedTimeOut: "1000",
  showEasing: "swing",
  hideEasing: "linear",
  showMethod: "fadeIn",
  hideMethod: "fadeOut",
};
