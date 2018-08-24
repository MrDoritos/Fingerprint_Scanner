function loadXML(){
var request = new XMLHttpRequest();
request.open("GET", "/DATABASE.XML", false);
request.send();
var xml = request.responseXML;
var fingers = xml.getElementsByName("finger");
for (var i = 0; i < fingers.length; i++){
    var finger = fingers[i];
    var name = (finger.getElementsByName("Name")[0]).innerText;
    var logstate = (finger.getElementsByName("LogState")[0]).innerText;
    var currentTime = (finger.getElementsByName("CurrentTime")[0]).innerText;
    var totalTime = (finger.getElementsByName("TotalTime")[0]).innerText;
    (document.getElementById(i.toString()).getElementsByClassName("name")[0]).innerText = name;
    alert(name);
    (document.getElementById(i.toString()).getElementsByClassName("signedin")[0]).innerText = logstate;
    (document.getElementById(i.toString()).getElementsByClassName("currenttime")[0]).innerText = currentTime;
    (document.getElementById(i.toString()).getElementsByClassName("totaltime")[0]).innerText = totalTime;
}    
}