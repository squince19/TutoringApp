﻿function LogOn(userName, userPassword) {
    //the url of the webservice we will be talking to
    var webMethod = "WebService.asmx/LogOn";
    //the parameters we will pass the service (in json format because curly braces)
    //note we encode the values for transmission over the web.  All the \'s are just
    //because we want to wrap our keynames and values in double quotes so we have to
    //escape the double quotes (because the overall string we're creating is in double quotes!)
    var parameters = "{\"userName\":\"" + encodeURI(userName) + "\",\"userPassword\":\"" + encodeURI(userPassword) + "\"}";
    //alert('here');
    //jQuery ajax method
    $.ajax({
        //post is more secure than get, and allows
        //us to send big data if we want.  really just
        //depends on the way the service you're talking to is set up, though
        type: "POST",
        //the url is set to the string we created above
        url: webMethod,
        //same with the data
        data: parameters,
        //these next two key/value pairs say we intend to talk in JSON format
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        //jQuery sends the data and asynchronously waits for a response.  when it
        //gets a response, it calls the function mapped to the success key here

        success: function (msg) {
            if (msg.d) {
                alert("Login Successful")
                //alert(userName);
                window.location = "PersonalizeProfile.html";

            }
            else {
                alert("Login Failed. Wrong username or password")
            }
        },
        error: function (e) {
           // alert("boo...");
        }
    });
}

function onProfileLoad() {
    var webMethod = "WebService.asmx/GetUserInfo";
    
    //var parameters = Session["id"];
    $.ajax({
        type: "POST",
        
        url: webMethod,
        data: parameters,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (msg) {
            var userID = msg.userID;
            document.getElementById("nameoutput").innerHTML = formatter.format(userName);
       
        },
        error: function (e) {
            alert("boo...");
        }
    });
}

