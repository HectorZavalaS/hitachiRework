function getVirtDir() {
    var Path = location.host;
    var VirtualDirectory;
    if (Path.indexOf("localhost") >= 0 && Path.indexOf(":") >= 0) {
        VirtualDirectory = "";

    }
    else {
        var pathname = window.location.pathname;
        var VirtualDir = pathname.split('/');
        VirtualDirectory = VirtualDir[1];
        VirtualDirectory = '/' + VirtualDirectory;
    }
    return VirtualDirectory;
}
function getSerialInfo() {
    $("#results").hide();
    $.ajax({
        method: "POST",
        url: getVirtDir() + "/Controllers/getSerialInfo.ashx",
        data: { serial: $("#serial").val()},
        success: function (data) {
            r = jQuery.parseJSON(data);
            if (r.result === "true") {
                $("#tblData").html(r.html);
                $("#serial").val("");
                $("#serial").focus();
            }
            else
                $("#tblData").html(r.MessageError);
            $("#results").show();
            return false;
        },
        error: function () { }
    })
}