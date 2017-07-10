function getUrlRoot() {
    var result = ``;

    result += location.protocol;
    result += "//";
    result += location.hostname;
    result += location.port.length > 0 ? `:${location.port}` : "";
    
    return result;
}