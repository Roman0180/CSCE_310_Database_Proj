items = {}
itemNum = 1

function userLogIn() {
    var nameValue = document.getElementById("floatingUsername").value;
    nameValue = nameValue.replace("@", "%40")
    var passwordValue = document.getElementById("floatingPassword").value;
    url = "https://localhost:7091/UserEntity?userName="+nameValue+"&password="+passwordValue

    fetch(url).then(response => 
        response.json().then(data => ({
            data: data,
            status: response.status
        })
    ).then(res => {
        // console.log(res.status, res.data.title)
        console.log(res)
        if(res.data.item1 == true){
            localStorage.id = res.data.item2 // userID
            localStorage.value = res.data.item7 // employee bool 
            console.log(localStorage.id, localStorage.value)
            firstName = res.data.item3
            lastName = res.data.item4
            window.alert("Welcome, " + firstName + " " + lastName + "!");
            window.location.replace("./index.html");
        }
        else{
            window.alert("Wrong username and/or password. Try again.");
        }
    }));
}

function userLogOut(){
    localStorage.clear()
    console.log("storage cleared")
}

function addToCart(id, price){
    var itemName = document.getElementById(id).textContent;
    var itemPrice = document.getElementById(price).textContent;
    itemPrice = parseFloat(itemPrice.substring(1))
    if (localStorage.getItem([itemName, itemPrice]) === null) {
        localStorage.setItem([itemName, itemPrice], itemCount);
    }
    else{
        var count = parseInt(localStorage.getItem([itemName, itemPrice]))
        count += 1
        localStorage.setItem([itemName, itemPrice], count);
    }
    for (var i = 0; i < localStorage.length; i++){
        if(!(localStorage.key(i) == "id" || localStorage.key(i) == "value")){
            console.log(localStorage.key(i) + " QTY: " + localStorage.getItem(localStorage.key(i)))
        }
    }
}

function showCartItems(){
    var completelist = document.getElementById("thelist");

    for (var i = 0; i < localStorage.length; i++){
        if(!(localStorage.key(i) == "id" || localStorage.key(i) == "value")){
            j++
            priceIndex = localStorage.key(i).toString().indexOf(",")
            itemSubstr = localStorage.key(i).toString().substring(0, priceIndex)
            priceSubstr = localStorage.key(i).toString().substring(priceIndex+1)
            completelist.innerHTML += "<li class='list-group-item'> <input class='form-check-input me-1' type='checkbox' value=''>" + itemSubstr + "&emsp; $" + priceSubstr +  "&emsp; x" + localStorage.getItem(localStorage.key(i)) + "</li>";
            price = parseFloat(localStorage.key(i).toString().substring(priceIndex+1)) * localStorage.getItem(localStorage.key(i))
            totalCost += price
        }
    }
    document.getElementById("totalCost").innerHTML = totalCost;
}

function deleteFromCart(){
    var cboxes = document.getElementsByClassName('form-check-input me-1');
    var len = cboxes.length;
    console.log(cboxes, len)
    for (var i=0; i<len; i++) {
        if(cboxes[i].checked){
            let priceSeperator = cboxes[i].offsetParent.innerText.indexOf("$")
            let quentitySeperator = cboxes[i].offsetParent.innerText.indexOf("x")
            var item = cboxes[i].offsetParent.innerText.substring(0, priceSeperator-1)
            var price = cboxes[i].offsetParent.innerText.substring(priceSeperator+1, quentitySeperator-1)
            var itemCount = localStorage.getItem([item, price])

            console.log(item)
            console.log(localStorage.getItem(item))

            // if(itemCount > 1){
            //     // console.log("has more than one")
            //     // console.log(localStorage.getItem([item, price], 1))
            //     // console.log(localStorage.getItem([item, price], count-1))
            //     // // localStorage.removeItem([item, price])
            // }
            // else{
            //     localStorage.removeItem([item, price])
            // }
        }
    }
    location.reload()
}

signUpUser = async () => {
    var firstName = document.getElementById("firstName").value;
    var lastName = document.getElementById("lastName").value;
    var email = document.getElementById("email").value;
    var password = document.getElementById("password").value;
    email = email.replace("@", "%40")
    const location = `https://localhost:7091/UserEntity?firstName=${firstName}&lastName=${lastName}&email=${email}&password=${password}`;
    const settings = {
        method: 'POST',
        headers: {
            Accept: 'application/json',
            'Content-Type': 'application/json',
        }
    };
    try {
        const fetchResponse = await fetch(location, settings);
    } catch (e) {
        return e;
    }  
}
function stringToTimestamp(s) {
    var t = s.match(/[\d\w]+/g);
    console.log(t)
    var months = {jan:'01',feb:'02',mar:'03',apr:'04',may:'05',jun:'06',
                  jul:'07',aug:'08',sep:'09',oct:'10',nov:'11',dec:'12'};
    function pad(n){return (n<10?'0':'') + +n;}
  
    return t[3] + '-' + months[t[1].toLowerCase()] + '-' + (parseInt(t[2]) + 1)
  }
function test(){
    event.preventDefault();
}


$(document).ready(function () {
    $("#reservationButton").click(function () {
        var restaurantNum = document.getElementById("restaurant").value;
        var f = $('#checkin_date').data().datepicker.viewDate;
        startDate = stringToTimestamp(f.toString())
        url = `https://localhost:7091/ReservationEntity?restaurantId=${restaurantNum}&startDate=${startDate}`
        $.get(url, function (data) {
            var T = document.getElementById("reservationTimes");
            T.style.display = "block";
            var select = document.getElementById("restaurantReservationTimes");

            for(var option of data) {
                var el = document.createElement("option");
                el.textContent = option;
                el.value = option;
                select.appendChild(el);
            }
        })
    });
});
function convertTime12To24(time) {
    var hours   = Number(time.match(/^(\d+)/)[1]);
    var minutes = Number(time.match(/:(\d+)/)[1]);
    var AMPM    = time.match(/\s(.*)$/)[1];
    console.log(AMPM)
    if (AMPM === "pm" && hours < 12) hours = hours + 12;
    if (AMPM === "am" && hours === 12) hours = hours - 12;
    var sHours   = hours.toString();
    var sMinutes = minutes.toString();
    if (hours < 10) sHours = "0" + sHours;
    if (minutes < 10) sMinutes = "0" + sMinutes;
    return (sHours + ":" + sMinutes);
}
makeReservation = async () => {
    var restaurantNum = document.getElementById("restaurant").value;
    var numPeople = document.getElementById("numPeople").value; 
    var f = $('#checkin_date').data().datepicker.viewDate;
    startDate = stringToTimestamp(f.toString())
    var startTime = document.getElementById("restaurantReservationTimes").value; 
    hours = convertTime12To24(startTime)
    formattedDateTime = startDate + 'T' + hours + ':00Z'
    // FIXME: update reservation maker to pull from local storage
    var reservationMaker = 1
    url = `https://localhost:7091/ReservationEntity?reservationPartySize=${numPeople}&reservationDateTime=${formattedDateTime}&restaurantId=${restaurantNum}&reservationMaker=${reservationMaker}`
    console.log(url)
    const settings = {
        method: 'POST',
        headers: {
            Accept: 'application/json',
            'Content-Type': 'application/json',
        }
    };
    try {
        const fetchResponse = await fetch(url, settings);
    } catch (e) {
        return e;
    }  
}
