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

            platformAbstraction.sendPostRequest("/data/storeEmployee", employee, success, failure);
        },
        get: function (parameters, success, failure) {
            // this method simulates an async server-request
            window.setTimeout(function () {
                try {
                    var emp = {};
                    emp.Name = "Markus Mustermann";
                    emp.Salary = 12345;
                    emp.Email = "markus.mustermann@example.com";
                    success(emp);
                } catch (err) {
                    failure(err);
                }
            }, 250);
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
                return key + ": " + validationResult[key] + "--- VR: " + JSON.stringify(validationResult);
            }
        }
        return false;
    }

})();



