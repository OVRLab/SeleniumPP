/**
 * SEProxy namespace.
 */
if ("undefined" == typeof(SEProxy)) {
  var SEProxy = {};
};

/**
 * Controls the browser overlay for the Hello World extension.
 */
SEProxy.BrowserOverlay = {
  /**
   * Says 'Hello' to the user.
   */
  sayHello : function(aEvent) {
    //let stringBundle = document.getElementById("seleniumideinterface-string-bundle");
    //let message = stringBundle.getString("seleniumideinterface.greeting.label");
    let message = "This is a test msgإQQQQQ!!!";
   window.alert(message);
    var xmlhttp=new XMLHttpRequest();
    xmlhttp.open("POST", 'http://localhost:7777');
    xmlhttp.onreadystatechange = function() {
        if (xmlhttp.readyState == 4) {
            if(xmlhttp.status==200){
              window.alert('ok');
            }else{
              window.alert('error')}
        }
    }
    xmlhttp.send("THis is A#$# TEESTTTT #$# WWWWAAAAAAAOOOOOOOO.");
  }
};



function loadJsonData() {
  let message = "This is a test msgإOOOOOOO!!!";
   window.alert(message);
        var postdata = JSON.stringify(
            {
                "From": "AA",
                "To": "BB",
                "Body": "CC"
            });
            try {
                $jQ.ajax({
                    type: "POST",
                    url: "MailHandler.ashx",
                    cache: false,
                    data: postdata,
                    dataType: "json",
                    success: getSuccess,
                    error: getFail
                });
            } catch (e) {
                alert(e);
            }
            function getSuccess(data, textStatus, jqXHR) {
                alert(data.Response);
        };
            function getFail(jqXHR, textStatus, errorThrown) {
                alert(jqXHR.status);
        };
    };
