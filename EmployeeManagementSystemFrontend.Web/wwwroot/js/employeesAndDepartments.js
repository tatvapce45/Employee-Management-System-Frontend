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
      loadDepartments();
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
    url: '/EmployeesAndDepartments/GetAddDepartmentModal',
    type: "GET",
    cache: false,
    success: function (response) {
      $("#modalLoader").html(response);
      $("#addDepartmentModal").modal("show");
    },
  });
}
