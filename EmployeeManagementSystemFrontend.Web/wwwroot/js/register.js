function loadRoles() {
    $.get("/authentication/GetRoles", function (data) {
        if (data && data.length > 0) {
            $("#registerRoleId").html('<option value="">Select Role</option>');
            $.each(data, function (i, role) {
                $("#registerRoleId").append(
                    '<option value="' + role.id + '">' + role.name + "</option>"
                );
            });
        }
    }).fail(function () {
        console.log("Error loading countries.");
    });
}

function loadCountries() {
    $.get("/authentication/GetCountries", function (data) {
        if (data && data.length > 0) {
            $("#registerCountryId").html('<option value="">Select Country</option>');
            $.each(data, function (i, country) {
                $("#registerCountryId").append('<option value="' + country.id + '">' + country.name + '</option>');
            });
        }
    }).fail(function () {
        console.log("Error loading countries.");
    });
}

function loadStates(countryId) {
    $.get("/authentication/GetStatesByCountryId?countryId=" + countryId, function (data) {
        if (data && data.length > 0) {
            $("#registerStateId").html('<option value="">Select State</option>');
            $.each(data, function (i, state) {
                $("#registerStateId").append('<option value="' + state.id + '">' + state.name + '</option>');
            });
        }
    }).fail(function () {
        console.log("Error loading states.");
    });
}

function loadCities(stateId) {
    $.get("/authentication/GetCitiesByStateId?stateId=" + stateId, function (data) {
        if (data && data.length > 0) {
            $("#registerCityId").html('<option value="">Select City</option>');
            $.each(data, function (i, city) {
                $("#registerCityId").append('<option value="' + city.id + '">' + city.name + '</option>');
            });
        }
    }).fail(function () {
        console.log("Error loading cities.");
    });
}

