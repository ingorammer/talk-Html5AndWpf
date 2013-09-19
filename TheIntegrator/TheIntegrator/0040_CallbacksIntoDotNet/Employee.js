var validators = {};
var dataAccess = {};

(function () {


    function validateEmployee(employee) {
        var validationResult = {};

        ensureRule(validationResult, employee.Name && employee.Name.length > 5, "Name", "Please specify a name with more than 5 characters");
        ensureRule(validationResult, employee.Email && employee.Email.indexOf("@") >= 0, "Email", "Please enter a valid email address");
        ensureRule(validationResult, !employee.LeaveDate || (employee.JoinDate && employee.LeaveDate), "JoinDate", "Please enter a join date if a leave date is selected");

        if (employee.LeaveDate && employee.JoinDate) {
            ensureRule(validationResult, employee.LeaveDate >= employee.JoinDate, "LeaveDate", "Please enter a value after JoinDate");
            ensureRule(validationResult, employee.LeaveDate >= employee.JoinDate, "JoinDate", "Please enter a value before LeaveDate");
        }

        return validationResult;
    }


    // ==== public API below

    validators.Employee = function (employee) {
        return JSON.stringify(validateEmployee(employee));
    };


    dataAccess.Employee = {
        store: function (employee, success, failure) {
            var val = validateEmployee(employee);
            var firstError = getFirstValidationError(val);

            if (firstError) {
                failure("This object still has validation errors ('" + firstError + "')");
                return;
            }

            if (employee.Id) {
                platformAbstraction.sendPostRequest("/api/Employee/" + employee.Id, employee, success, failure);
            } else {
                platformAbstraction.sendPutRequest("/api/Employee", employee, success, failure);
            }
        },
        get: function (parameters, success, failure) {
            platformAbstraction.sendGetRequest("/api/Employee/" + parameters.Id, success, failure);
        }
    };




    // ==== helper methods below

    function ensureRule(validationResult, condition, fieldName, message) {
        if (condition) return;
        validationResult[fieldName] = message;
    }

    function getFirstValidationError(validationResult) {
        for (var key in validationResult) {
            if (validationResult.hasOwnProperty(key)) {
                return key + ": " + validationResult[key];
            }
        }
        return false;
    }

})();



