function sendNotification(email, message) {
  $.ajax({
    type: "POST",
    url: "/EmployeesAndDepartments/SendNotification",
    data: { email: email, message: message },
    success: function (data) {
      console.log("Notification triggered.");
      console.log(data);
    },
    error: function () {
      alert("Failed to send notification.");
    },
  });
}

var userEmail = $('#layoutEmailField').val();
const connection = new signalR.HubConnectionBuilder()
  .withUrl(
    `http://localhost:5287/notificationHub?email=${encodeURIComponent(
      userEmail
    )}`
  )
  .configureLogging(signalR.LogLevel.Information)
  .build();

connection
  .start()
  .then(function () {
    console.log("✅ SignalR connected as:", userEmail);
  })
  .catch(function (err) {
    console.error("❌ SignalR connection error:", err.toString());
  });

connection.on("ReceiveNotification", function (message) {
  document.getElementById("notificationDot").classList.remove("d-none");

  startShakingBell();
  addNotification(message);
  document
    .getElementById("notificationBellWrapper")
    .addEventListener("click", () => {
      stopShakingBell();
      document.getElementById("notificationDot").classList.add("d-none");
    });
});
