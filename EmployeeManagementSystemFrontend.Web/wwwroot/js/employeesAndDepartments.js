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
    },
  });
}

function loadEmployees(
  departmentId,
  pageNumber,
  pageSize,
  searchTerm,
  sortBy,
  sortOrder
) {
  localStorage.setItem("pageNumber", pageNumber);
  localStorage.setItem("pageSize", pageSize);
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

$(document).on("input", "#employeeSearchInput", function () {
  var pageNumber = localStorage.getItem("pageNumber");
  var pageSize = localStorage.getItem("pageSize");
  var selectedDepartmentId = localStorage.getItem("activeDepartment");
  loadEmployees(selectedDepartmentId, pageNumber, pageSize, this.value);
});

function setActiveDepartment(deptId, deptName, deptRowId) {
  localStorage.setItem("activeDepartment", deptId);
  updateDepartmentStyle(deptName, deptRowId, true);
  resetOtherDepartments(deptId, deptRowId);
  loadEmployees(deptId, 1, 5, "");
}

function resetOtherDepartments(activeId, activeRowId) {
  document.querySelectorAll(".departmentsRows").forEach((department) => {
    const deptId = department.id.split("-")[1];
    if (deptId !== activeId) {
      const deptName = department.querySelector("span").id;
      updateDepartmentStyle(deptName, `department-${deptId}`, false);
    }
  });
}

function updateDepartmentStyle(name, rowId, isActive) {
  const row = document.getElementById(rowId);
  const text = document.getElementById(name);
  const dragIcon = document.getElementById(`imageFor ${name}`);
  const editIcon = document.getElementById(`editImageFor ${name}`);
  const deleteIcon = document.getElementById(`deleteImageFor ${name}`);

  if (row && text && dragIcon && editIcon && deleteIcon) {
    row.style.backgroundColor = isActive ? "rgb(46, 10, 125,0.6)" : "";
    row.style.transition = "0.3s";
    text.style.color = isActive ? "#fff" : "";
    text.style.transition = "0.3s";

    dragIcon.src = isActive
      ? "/images/draggabledots-svgrepo-com-active.svg"
      : "/images/draggabledots-svgrepo-com.svg";

    editIcon.src = isActive
      ? "/images/edit-button-svgrepo-com-active.svg"
      : "/images/edit-button-svgrepo-com.svg";

    deleteIcon.src = isActive
      ? "/images/edit-delete-svgrepo-com-active.svg"
      : "/images/edit-delete-svgrepo-com.svg";
  }
}

function loadAddDepartmentModal() {
  $.ajax({
    url: "/EmployeesAndDepartments/GetAddDepartmentModal",
    type: "GET",
    cache: false,
    success: function (response) {
      $("#modalLoader").html(response);
      $("#addDepartmentModal").modal("show");
    },
  });
}

function loadUpdateDepartmentModal(id) {
  console.log(id);
  $.ajax({
    url: "/EmployeesAndDepartments/GetUpdateDepartmentModal",
    type: "GET",
    data: { id: id },
    cache: false,
    success: function (response) {
      $("#modalLoader").html(response);
      $("#updateDepartmentModal").modal("show");
    },
  });
}

function loadConfirmationModal(message, work, id) {
  console.log(id);
  $.ajax({
    url: "/EmployeesAndDepartments/GetConfirmationModal",
    type: "GET",
    data: { confirmationMessage: message, confirmationWork: work, id: id },
    cache: false,
    success: function (response) {
      $("#modalLoader").html(response);
      $("#confirmationModal").modal("show");
    },
  });
}

function loadAddEmployeeModal() {
  $.ajax({
    url: "/EmployeesAndDepartments/GetAddEmployeeModal",
    type: "GET",
    cache: false,
    success: function (response) {
      $("#modalLoader").html(response);
      $("#addEmployeeModal").modal("show");
    },
  });
}

function loadCountries() {
  $.get("/authentication/GetCountries", function (data) {
    if (data && data.length > 0) {
      $("#addEmployeeCountryId").html(
        '<option value="">Select Country</option>'
      );
      $.each(data, function (i, country) {
        $("#addEmployeeCountryId").append(
          '<option value="' +
            country.id +
            '" style="color:#212529">' +
            country.name +
            "</option>"
        );
      });
    }
  }).fail(function () {
    console.log("Error loading countries.");
  });
}

function loadStates(countryId) {
  $.get(
    "/authentication/GetStatesByCountryId?countryId=" + countryId,
    function (data) {
      if (data && data.length > 0) {
        $("#addEmployeeStateId").html('<option value="">Select State</option>');
        $.each(data, function (i, state) {
          $("#addEmployeeStateId").append(
            '<option value="' +
              state.id +
              '" style="color:#212529">' +
              state.name +
              "</option>"
          );
        });
      }
    }
  ).fail(function () {
    console.log("Error loading states.");
  });
}

function loadCities(stateId) {
  $.get(
    "/authentication/GetCitiesByStateId?stateId=" + stateId,
    function (data) {
      if (data && data.length > 0) {
        $("#addEmployeeCityId").html('<option value="">Select City</option>');
        $.each(data, function (i, city) {
          $("#addEmployeeCityId").append(
            '<option value="' +
              city.id +
              '" style="color:#212529">' +
              city.name +
              "</option>"
          );
        });
      }
    }
  ).fail(function () {
    console.log("Error loading cities.");
  });
}

function loadDepartmentsJson() {
  $.get("/EmployeesAnddepartments/GetDepartmentsJson", function (data) {
    console.log(data);
    if (data && data.length > 0) {
      $("#addEmployeeDepartmentId").html(
        '<option value="">Select Department</option>'
      );
      $.each(data, function (i, department) {
        $("#addEmployeeDepartmentId").append(
          '<option value="' +
            department.id +
            '" style="color:#212529">' +
            department.name +
            "</option>"
        );
      });
    }
  }).fail(function () {
    console.log("Error loading departments.");
  });
}

function loadUpdateEmployeeModal(id) {
  $.ajax({
    url: "/EmployeesAndDepartments/GetUpdateEmployeeModal",
    type: "GET",
    data: { id: id },
    cache: false,
    success: function (response) {
      $("#modalLoader").html(response);
      $("#updateEmployeeModal").modal("show");
    },
  });
}

function loadDeleteMultipleItemsModal() {
  var selectedEmployees = [
    ...document.querySelectorAll(".employees-checkboxes:checked"),
  ].map((x) => x.value);
  if (selectedEmployees.length === 0) {
    toastr.warning("Please select items to delete");
  } else {
    loadConfirmationModal('Are you sure you want to delete these employees?','DeleteMultipleEmployees',)
  }
}
