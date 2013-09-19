var platformAbstraction = {};

if (!window) {
    var window = {};
}

(function() {
    var callbackId = 0;
    
    platformAbstraction.callbacks = {};

    function addCallback(success, failure) {
        callbackId++;
        var callbackKey = "callback_" + callbackId;
        
        platformAbstraction.callbacks[callbackKey] = {
            success: function (callbackData) {
                if (callbackData && callbackData.data) callbackData = callbackData.data; // unwrap the JSON-wrapped response
                if (success) success(callbackData);
                delete platformAbstraction.callbacks[callbackKey];
            },
            error: function (callbackData) {
                if (callbackData && callbackData.data) callbackData = callbackData.data; // unwrap the JSON-wrapped response
                if (failure) failure(callbackData);
                delete platformAbstraction.callbacks[callbackKey];
            }
        };

        return "platformAbstraction.callbacks." + callbackKey;
    }

    platformAbstraction.sendPostRequest = function (url, data, success, failure) {
        var callbackKey = addCallback(success, failure);
        // call into the C#-provided native wrapper
        var jsonData = JSON.stringify(data);
        nativeFunctions.SendRequest(url, "POST", jsonData, callbackKey);
    };

    platformAbstraction.sendPutRequest = function (url, data, success, failure) {
        var callbackKey = addCallback(success, failure);
        // call into the C#-provided native wrapper
        var jsonData = JSON.stringify(data);
        nativeFunctions.SendRequest(url, "PUT", jsonData, callbackKey);
    };
    
    platformAbstraction.sendGetRequest = function (url, success, failure) {
        var callbackKey = addCallback(success, failure);
        // call into the C#-provided native wrapper
        nativeFunctions.SendRequest(url, "GET", null, callbackKey);
    };

    window.setTimeout = window.setTimeout || function(func, msec) {
        var callbackKey = addCallback(func);
        // call into the C#-provided native wrapper
        nativeFunctions.SetTimeout(msec, callbackKey);
    };
})();
