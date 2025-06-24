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
      var timeWiseEmployees = response.timeWiseEmployees;
      var genderWiseEmployees = response.genderWiseEmployees;
      var ageWiseEmployees = response.ageWiseEmployees;
      var departmentWiseEmployees = response.departmentWiseEmployees;
      var countryWiseEmployees = response.countryWiseEmployees;
      var days = Object.keys(timeWiseEmployees);
      var employees = Object.values(timeWiseEmployees);

      var maxValue = Math.max(...employees);

      new Chart("timeWiseEmployeeChart", {
        type: "line",
        data: {
          labels: days,
          datasets: [
            {
              fill: true,
              lineTension: 0.5,
              backgroundColor: "rgb(46, 10, 125,0.2)",
              borderColor: "rgb(46, 10, 125,0.8)",
              data: employees,
            },
          ],
        },
        options: {
          legend: { display: false },
          scales: {
            yAxes: [
              {
                ticks: {
                  min: 0,
                  max: Math.ceil(maxValue / 10) * 10,
                  stepSize: (Math.ceil(maxValue / 10) * 10) / 4,
                },
              },
            ],
          },
        },
      });

      const data = {
        labels: Object.keys(genderWiseEmployees),
        datasets: [
          {
            label: "My First Dataset",
            data: Object.values(genderWiseEmployees),
            backgroundColor: ["rgb(255, 99, 132)", "rgb(54, 162, 235)"],
            hoverOffset: 4,
          },
        ],
      };

      const config = {
        type: "doughnut",
        data: data,
      };

      const myChart = new Chart(
        document.getElementById("genderWiseEmployeeChart"),
        config
      );

      new Chart("ageWiseEmployeeChart", {
        type: "doughnut",
        data: {
          labels: Object.keys(ageWiseEmployees),
          datasets: [
            {
              label: "My First Dataset",
              data: Object.values(ageWiseEmployees),
              backgroundColor: generateRandomColors(
                Object.keys(ageWiseEmployees).length
              ),
              hoverOffset: 4,
            },
          ],
        },
      });

      new Chart("departmentWiseEmployeeChart", {
        type: "doughnut",
        data: {
          labels: Object.keys(departmentWiseEmployees),
          datasets: [
            {
              label: "My First Dataset",
              data: Object.values(departmentWiseEmployees),
              backgroundColor: generateRandomColors(
                Object.keys(departmentWiseEmployees).length
              ),
              hoverOffset: 4,
            },
          ],
        },
      });

      new Chart("countryWiseEmployeeChart", {
        type: "doughnut",
        data: {
          labels: Object.keys(countryWiseEmployees),
          datasets: [
            {
              label: "My First Dataset",
              data: Object.values(countryWiseEmployees),
              backgroundColor: generateRandomColors(
                Object.keys(countryWiseEmployees).length
              ),
              hoverOffset: 4,
            },
          ],
        },
      });

      function generateRandomColors(count) {
        const colors = [];
        for (let i = 0; i < count; i++) {
          const r = Math.floor(Math.random() * 256);
          const g = Math.floor(Math.random() * 256);
          const b = Math.floor(Math.random() * 256);
          colors.push(`rgb(${r}, ${g}, ${b})`);
        }
        return colors;
      }
      $("#dashboardLoader").hide();
    },
  });
}

$("#dashboardSelectTimeField").on("change", function () {
  var value = $(this).val();
  if (value != 5) {
    getDashboardData(value);
  } else {
    $.ajax({
      type: "GET",
      url: "/home/GetCustomDateSelectorModal",
      success: function (data) {
        $("#layoutPageModalLoader").html(data);
        $("#selectDateRangeForDashboardDataModal").modal("show");
      },
    });
  }
});
