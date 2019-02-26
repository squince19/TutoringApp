var login = sessionStorage.getItem('logon');


function displayButton() {
    var log = sessionStorage.getItem('logon');
    if (log == 'true') {
        document.getElementById('searchTable').style.display = "block";
        document.getElementById('headerForm').style.visibility = "hidden";
        document.getElementById('headerNavView').style.display = "block";
        //document.getElementById('headerNav').style.display = "block";
      }
    else
        document.getElementById('searchTable').style.display = "none";
        document.getElementById('headerForm').style.display = "block";
        //document.getElementById('searchButton').style.visibility = "hidden";

}


function LogOn(userName, userPassword) {
    //the url of the webservice we will be talking to
    var webMethod = "WebService.asmx/LogOn";
    var parameters = "{\"userName\":\"" + encodeURI(userName) + "\",\"userPassword\":\"" + encodeURI(userPassword) + "\"}";
    $.ajax({
        type: "POST",
        url: webMethod,
        data: parameters,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        //gets a response, it calls the function mapped to the success key here
        success: function (msg) {
            if (msg.d) {
                alert("Login Successful")
                window.location.href = 'PersonalizeProfile.html';
                sessionStorage.setItem('logon', 'true');
                displayButton();
                alert(sessionStorage.getItem('logon'));
            }
            else {
                alert("Login Failed. Wrong username or password")
            }
        },
        error: function (e) {
            alert("boo...");
        }
    });
}

var tutorArray;


function searchTutors(courseProf) {
    var webMethod = "WebService.asmx/FindTutor";
    var parameters = "{\"courseProf\":\"" + encodeURI(courseProf) + "\"}";
    $.ajax({
        type: "POST",
        url: webMethod,
        data: parameters,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (msg) {
            alert("working!")
            tutorArray = msg.d;
            alert(tutorArray[0].firstName);
            sessionStorage.setItem('fn1', tutorArray[0].firstName);
            sessionStorage.setItem('ln1', tutorArray[0].lastName);
            sessionStorage.setItem('e1', tutorArray[0].email);
            sessionStorage.setItem('c1', tutorArray[0].courseProf);

            sessionStorage.setItem('fn2', tutorArray[1].firstName);
            sessionStorage.setItem('ln2', tutorArray[1].lastName);
            sessionStorage.setItem('e2', tutorArray[1].email);
            sessionStorage.setItem('c2', tutorArray[1].courseProf);

            sessionStorage.setItem('fn3', tutorArray[2].firstName);
            sessionStorage.setItem('ln3', tutorArray[2].lastName);
            sessionStorage.setItem('e3', tutorArray[2].email);
            sessionStorage.setItem('c3', tutorArray[2].courseProf);


            window.location.href = 'searchResults.html';
        },
        error: function (e) {
            alert("sad");
        }
    });
}

function populateResults() {

    document.getElementById("firstname1").innerHTML = sessionStorage.getItem('fn1');
    document.getElementById('lastname1').innerHTML = sessionStorage.getItem('ln1');
    document.getElementById('email1').innerHTML = sessionStorage.getItem('e1');
    document.getElementById('course1').innerHTML = sessionStorage.getItem('c1');

    document.getElementById("firstname2").innerHTML = sessionStorage.getItem('fn2');
    document.getElementById("lastname2").innerHTML = sessionStorage.getItem('ln2');
    document.getElementById("email2").innerHTML = sessionStorage.getItem('e2');
    document.getElementById("course2").innerHTML = sessionStorage.getItem('c2');


    document.getElementById("firstname3").innerHTML = sessionStorage.getItem('fn3');
    document.getElementById("lastname3").innerHTML = sessionStorage.getItem('ln3');
    document.getElementById("email3").innerHTML = sessionStorage.getItem('e3');
    document.getElementById("course3").innerHTML = sessionStorage.getItem('c3');
}



function onProfileLoad() {
    var webMethod = "WebService.asmx/GetUserInfo";
    $.ajax({
        type: "POST",
        url: webMethod,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (msg) {
            //alert(msg);
            login = true;
            document.getElementById("nameOutput").innerHTML = msg.d.firstName + " "+msg.d.lastName;
            document.getElementById("emailOutput").innerHTML = msg.d.email;
            document.getElementById("phoneNumberoutput").innerHTML = msg.d.phoneNumber;
            //displayButton();
 
        },
        error: function (e) {
            alert("boo...");
        }
    });
}

//****************returning "SAD" 
function createAccount(userType, fname, lname, phoneNumber, userName, email, password) {
    var webMethod = "WebService.asmx/AddUser";
    var parameters = "{\"userType\":\"" + encodeURI(userType) + "\",\"firstName\":\"" + encodeURI(fname) + "\",\"lastName\":\"" + 
        encodeURI(lname) + "\",\"phoneNumber\":\"" + encodeURI(phoneNumber) + "\",\"userName\":\"" + encodeURI(userName) +
        "\",\"userEmail\":\"" + encodeURI(email) + "\",\"userPassword\":\"" + encodeURI(password) + "\"}";
    $.ajax({
        type: "POST",
        url: webMethod,
        data: parameters,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (msg) {
            alert("It's working, hopefully!");
        },
        error: function (e) {
            alert("sad");
        }
    });
}
