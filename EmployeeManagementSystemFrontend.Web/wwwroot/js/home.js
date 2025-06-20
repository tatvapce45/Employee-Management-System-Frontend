function getDashboardData(timeId, fromDate, toDate) {
  $.ajax({
    url: "/Home/GetDashboardData",
    type: "GET",
    data: {
      timeId: timeId,
      fromDate: fromDate,
      toDate: toDate,
    },
    cache: false,
    success: function (response) {
      console.log(response)
    },
  });
}
