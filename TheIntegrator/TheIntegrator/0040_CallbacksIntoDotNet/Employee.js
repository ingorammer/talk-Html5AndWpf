// WARNING: Only proof of concept! Normally, this should be a nicer library, which doesn't expose everything to the global scope!


function validateEmployee(jsonEmp) {
    var validationResult = {};
    var employee = JSON.parse(jsonEmp);

    ensureRule(validationResult, employee.Name && employee.Name.length > 5, "Name", "Please specify a name with more than 5 characters");
    ensureRule(validationResult, employee.Email && employee.Email.indexOf("@") >= 0, "Email", "Please enter a valid email address");

    return JSON.stringify(validationResult);
};

function ensureRule(validationResult, condition, fieldName, message) {
    if (condition) return;
    validationResult[fieldName] = message;
}



