function showData(jsonData) {
    var data = JSON.parse(jsonData);
    $("#spouseInput").val(data.Spouse);
    $("#homeEmailInput").val(data.HomeEmail);
}

function getData() {
    var data = {};
    data.Spouse = $("#spouseInput").val();
    data.HomeEmail = $("#homeEmailInput").val();
    return JSON.stringify(data);
}