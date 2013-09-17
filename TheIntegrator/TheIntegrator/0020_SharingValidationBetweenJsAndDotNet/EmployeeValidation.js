function validateEmployee(jsonEmp) {
    var validationResult = {};
    var employee = JSON.parse(jsonEmp);

    ensureRule(validationResult, employee.Name && employee.Name.length > 5, "name", "Please specify a name with more than 5 characters");
    ensureRule(validationResult, employee.Email && employee.Email.indexOf("@") >= 0, "email", "Please enter a valid email address");

    return JSON.stringify(validationResult);
}


function ensureRule(validationResult, condition, fieldName, message) {
    if (condition) return;
    validationResult[fieldName] = message;
}

