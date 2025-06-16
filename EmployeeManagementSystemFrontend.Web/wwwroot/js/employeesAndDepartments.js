function loadDepartmentsContainer() {
  $.ajax({
    url: "/EmployeesAndDepartments/GetDepartmentsContainer",
    type: "GET",
    cache: false,
    success: function (data) {
      $("#departmentsAndEmployeesDepartmentsContainer").html(data);
      loadDepartments();
    },
  });
}

function loadDepartments() {
  $.ajax({
    url: "/EmployeesAndDepartments/GetDepartments",
    type: "GET",
    cache: false,
    success: function (data) {
      $("#departmentsLoader").html(data);
      loadEmployees(1, 1, 5, "");
    },
  });
}

function loadEmployeesContainer() {
  $.ajax({
    url: "/EmployeesAndDepartments/GetEmployeesContainer",
    type: "GET",
    cache: false,
    success: function (data) {
      $("#departmentsAndEmployeesEmployeesListContainer").html(data);
      loadDepartments();
    },
  });
}

function loadEmployees(departmentId, pageNumber, pageSize, searchTerm,sortBy,sortOrder) {
  $.ajax({
    url: "/EmployeesAndDepartments/GetEmployees",
    type: "POST",
    contentType: "application/json", 
    data: JSON.stringify({
      departmentId: departmentId,
      pageNumber: pageNumber,
      pageSize: pageSize,
      searchTerm: searchTerm || "",
      sortBy: sortBy,
      sortOrder: sortOrder,
    }),
    success: function (response) {
      $("#employeesAndDepartmentsEmployeeLoaderDiv").html(response);
    },
    error: function (xhr, status, error) {
      console.error("Failed to load employees:", error);
    },
  });
}
