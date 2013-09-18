// WARNING: Only proof of concept! Normally, this should be a nicer library, which doesn't expose everything to the global scope!

var validators = {};

validators.Employee = function (jsonEmp) {
    var validationResult = {};
    var employee = JSON.parse(jsonEmp);

    if (employee.JoinDate) employee.JoinDate = Date.parse(employee.JoinDate);
    if (employee.LeaveDate) employee.LeaveDate =  Date.parse(employee.LeaveDate);

    ensureRule(validationResult, employee.Name && employee.Name.length > 5, "Name", "Please specify a name with more than 5 characters");
    ensureRule(validationResult, employee.Email && employee.Email.indexOf("@") >= 0, "Email", "Please enter a valid email address");
    ensureRule(validationResult, !employee.LeaveDate || (employee.JoinDate && employee.LeaveDate), "JoinDate", "Please enter a join date if a leave date is selected");

    if (employee.LeaveDate && employee.JoinDate) {
        ensureRule(validationResult, employee.LeaveDate > employee.JoinDate, "LeaveDate", "Please enter a value after JoinDate");
        ensureRule(validationResult, employee.LeaveDate > employee.JoinDate, "JoinDate", "Please enter a value before LeaveDate");
    }
    
    return JSON.stringify(validationResult);
};

function ensureRule(validationResult, condition, fieldName, message) {
    if (condition) return;
    validationResult[fieldName] = message;
}



