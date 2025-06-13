function loadDepartmentsContainer() {
  $.ajax({
    url:"/EmployeesAndDepartments/GetDepartmentsContainer",
    type: "GET",
    cache: false,
    success: function (data) {
      $("#departmentsAndEmployeesDepartmentsContainer").html(data);
      loadDepartments();
    },
  });
}

function loadDepartments(){
  $.ajax({
    url: "/EmployeesAndDepartments/GetDepartments",
    type: "GET",
    cache: false,
    success: function (data) {
      $("#departmentsContainer").html(data);
    },
  });
}
