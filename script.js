items = {}
itemNum = 1
function saveNam(){
    if(localStorage.getItem("currRes") == null){
        localStorage.setItem("currRes", 4)
    }
}
function saveMess(){
    if(localStorage.getItem("currRes") == null){
        localStorage.setItem("currRes", 3)
    }
}
function saveFuego(){
    if(localStorage.getItem("currRes") == null){
        localStorage.setItem("currRes", 1)
    }
}
function saveLaynes(){
    if(localStorage.getItem("currRes") == null){
        localStorage.setItem("currRes", 2)
    }
}
function userLogIn() {
    var nameValue = document.getElementById("floatingUsername").value;
    nameValue = nameValue.replace("@", "%40")
    var passwordValue = document.getElementById("floatingPassword").value;
    url = "https://localhost:7091/UserEntity?userName=" + nameValue + "&password=" + passwordValue

    fetch(url).then(response =>
        response.json().then(data => ({
            data: data,
            status: response.status
        })
        ).then(res => {
            // console.log(res.status, res.data.title)
            console.log(res)
            if (res.data.item1 == true) {
                localStorage.id = res.data.item2 // userID
                localStorage.value = res.data.item7 // employee bool 
                localStorage.email = res.data.item5
                localStorage.password = res.data.item6
                localStorage.firstName = res.data.item3
                localStorage.lastName = res.data.item4
                console.log(localStorage.id, localStorage.value)
                firstName = res.data.item3
                lastName = res.data.item4
                var userId = parseInt(localStorage.getItem("id"))
                var url = `https://localhost:7091/EmployeeEntity/getEmployeeByUserId?userId=${userId}`
                $.get(url, function (data) {
                    var isEmployee = data.item1
                    var employeeId = data.item2
                    var restaurantId = data.item4
                    var isAdmin = data.item5
                    localStorage.setItem("isEmployee", data.item1);
                    localStorage.setItem("restaurantData", restaurantId);
                })
                window.alert("Welcome, " + firstName + " " + lastName + "!");
                window.location.replace("./index.html");
            }
            else {
                window.alert("Wrong username and/or password. Try again.");
            }
        }));
}


function toggleEnable(ids) {
    ids.forEach(id => {
        console.log(id)
        var textbox = document.getElementById(id);

        if (textbox.disabled) {
            // If disabled, do this 
            document.getElementById(id).disabled = false;
        } else {
            // Enter code here
            document.getElementById(id).disabled = true;
        }
    })

}

function userLogOut() {
    localStorage.clear()
    console.log("storage cleared")
}

function addToCart(id, price) {
    var itemName = document.getElementById(id).textContent;
    var itemPrice = document.getElementById(price).textContent;
    itemPrice = parseFloat(itemPrice.substring(1))
    if (localStorage.getItem([itemName, itemPrice, localStorage.getItem("currRes")]) === null) {
        localStorage.setItem([itemName, itemPrice,localStorage.getItem("currRes")], itemCount);
    }
    else if (!(localStorage.getItem([itemName, itemPrice, localStorage.getItem("currRes")]) === null)){
        var count = parseInt(localStorage.getItem([itemName, itemPrice, localStorage.getItem("currRes")]))
        count += 1
        localStorage.setItem([itemName, itemPrice, localStorage.getItem("currRes")], count);
    }
    // for (var i = 0; i < localStorage.length; i++) {
    //     if (!(localStorage.key(i) == "id" || localStorage.key(i) == "value")) {
    //         console.log(localStorage.key(i) + " QTY: " + localStorage.getItem(localStorage.key(i)))
    //     }
    // }
}

function showCartItems() {
    var completelist = document.getElementById("thelist");
    var ignore = ["id", "value", "firstName", "lastName", "email", "password", "order", ,"placedNum", "latestOrderNum", "itemsInOrder", "restaurantData", "isEmployee", "currRes"]

    for (var i = 0; i < localStorage.length; i++) {
        myString = localStorage.key(i)
        var invalidKey = ignore.some(item => myString.includes(item))

        if(!invalidKey){
            j++
            priceIndex = localStorage.key(i).toString().indexOf(",")
            itemSubstr = localStorage.key(i).toString().substring(0, priceIndex)
            var content = localStorage.key(i).toString().substring(priceIndex + 1).split(',')
            priceSubstr = content[0]
            restaurant = content[1]
            completelist.innerHTML += "<li class='list-group-item'> <input class='form-check-input me-1' type='checkbox' value=''>" + itemSubstr + "&emsp; $" + priceSubstr + "&emsp; x" + localStorage.getItem(localStorage.key(i)) + "</li>";
            price = parseFloat(localStorage.key(i).toString().substring(priceIndex + 1)) * localStorage.getItem(localStorage.key(i))
            totalCost += price
        }
    }
    document.getElementById("totalCost").innerHTML = parseFloat(totalCost).toFixed(2);
}

function deleteFromCart() {
    var cboxes = document.getElementsByClassName('form-check-input me-1');

    var len = cboxes.length;
    console.log(cboxes, len)
    for (var i = 0; i < len; i++) {
        if (cboxes[i].checked) {
            let priceSeperator = cboxes[i].offsetParent.innerText.lastIndexOf("$")
            let quentitySeperator = cboxes[i].offsetParent.innerText.lastIndexOf("x")
            var item = cboxes[i].offsetParent.innerText.substring(0, priceSeperator - 1)
            var price = cboxes[i].offsetParent.innerText.substring(priceSeperator + 1, quentitySeperator - 1)
            item = item.trim()
            price = price.trim()

            var itemCount = localStorage.getItem([item, parseFloat(price)])
            itemCount -= 1
            if (itemCount < 1) {
                localStorage.removeItem([item, price])
            }
            else {
                localStorage.setItem([item, price], itemCount)
            }

        }
    }
    location.reload()
}

function showAllOrders() {
    var completelist = document.getElementById("placedList");
    var orders = ["order"]
    for (var i = 0; i < localStorage.length; i++) {
        myString = localStorage.key(i)
        var validOrder = orders.some(item => myString.includes(item))

        if(validOrder){
            console.log(localStorage.getItem("latestOrderNum"))
            let num = localStorage.key(i).toString().substring(5)
            completelist.innerHTML += "<li class='list-group-item'> Order #" + num + ": " + localStorage.getItem(localStorage.key(i)) + "</li>";
        }
    }
}

getOrderNum = async () => {
    url =  `https://localhost:7091/PlacedOrderEntity/grabLatestOrder`
    $.get(url, function(data){
        localStorage.setItem("latestOrderNum", data); 
        var completelist = document.getElementById("placedList");
        var orders = ["order"]
        for (var i = 0; i < localStorage.length; i++) {
            myString = localStorage.key(i)
            var validOrder = orders.some(item => myString.includes(item))

            if(validOrder){
                console.log(localStorage.getItem("latestOrderNum"))
                let num = localStorage.key(i).toString().substring(5)
                completelist.innerHTML += "<li class='list-group-item'> Order #" + num + ": " + localStorage.getItem(localStorage.key(i)) + "</li>";
            }
        }
    })
    // console.log(url)

    // fetch(url).then(response =>
    //     response.json().then(data => ({
    //         data: data,
    //         status: response.status
    //     })
    //     ).then(res => {
    //         console.log(res)
    //         console.log(res.data)
    //         num = res.data + 1
    //         localStorage.setItem("latestOrderNum", num)
    //         console.log(localStorage.getItem("latestOrderNum"))
    //     }));

}
loadOrder = async () =>{
    url =  `https://localhost:7091/PlacedOrderEntity/grabLatestOrder`
    $.get(url, function(data){
        localStorage.setItem("latestOrderNum", data);
        let orderName = "order" + localStorage.getItem("latestOrderNum").toString(); 
        localStorage.setItem(orderName, JSON.stringify(localStorage.getItem("itemsInOrder")));
        console.log("test " + orderName)
        var completelist = document.getElementById("placedList");
        var orders = ["order"]
        for (var i = 0; i < localStorage.length; i++) {
            myString = localStorage.key(i)
            var validOrder = orders.some(item => myString.includes(item))

            if(validOrder){
                console.log(localStorage.getItem("latestOrderNum"))
                let num = localStorage.key(i).toString().substring(5)
                completelist.innerHTML += "<li class='list-group-item'> Order #" + num + ": " + localStorage.getItem(localStorage.key(i)) + "</li>";
            }
        }
    })
}

placeOrder = async () => {
    custId = parseInt(localStorage.getItem("id"))
    orderTotal = parseFloat(document.getElementById("totalCost").innerHTML)
    url = `https://localhost:7091/PlacedOrderEntity?customer_id=${custId}&total=${orderTotal}`
    console.log(url)

    var ignore = ["id", "value", "firstName", "lastName", "email", "password", "order", ,"placedNum", "latestOrderNum", "itemsInOrder", "restaurantData", "isEmployee", "currRes"]

    for (var i = 0; i < localStorage.length; i++) {
        myString = localStorage.key(i)
        var invalidKey = ignore.some(item => myString.includes(item))

        if(!invalidKey){
            priceIndex = localStorage.key(i).toString().indexOf(",")
            itemSubstr = localStorage.key(i).toString().substring(0, priceIndex)
            priceSubstr = localStorage.key(i).toString().substring(priceIndex + 1)
            itemsInOrder[orderIndex] = itemSubstr
            orderIndex++
            localStorage.removeItem([itemSubstr, priceSubstr])
        }
    }
    itemsInOrder[orderIndex] = "Order Total: " + totalCost
    localStorage.setItem("itemsInOrder", itemsInOrder)
    

    const settings = {
        method: 'POST',
        headers: {
            Accept: 'application/json',
            'Content-Type': 'application/json',
        }
    };
    try {
        const fetchResponse = await fetch(url, settings);
        window.alert("Your order has been placed!");
        window.location.replace("./vieworders.html");
    } catch (e) {
        return e;
    }

    
}
addEmployee = async () => {
    // 1.get data
    var empId = document.getElementById("empId")
    var adminLevel = document.querySelector('input[name="genderS"]:checked').value;
    restaurants = {1: "Fuego Tortilla Grill", 2: "Layne's", 3:"MESS Waffles, Etc.", 4:"Nam Cafe"}
    var restaurantId = restaurants[parseInt(localStorage.getItem("restaurantData"))]
    //2. make post request after information has been filled in
    url = `https://localhost:7091/EmployeeEntity/registerEmployee?userId=${empId}&restaurantId=${restaurantId}&adminFlag=${adminLevel}`
    const settings = {
        method: 'POST',
        headers: {
            Accept: 'application/json',
            'Content-Type': 'application/json',
        }
    };
    try {
        const fetchResponse = await fetch(url, settings);
        window.alert("Employee added!");
    } catch (e) {
        return e;
    }




}
function revealEmployeeAdd(){
    document.getElementById("empEditContainer").style = "block"

}
function getRestaurantData() {
    if (localStorage.getItem("isEmployee") == "true") {
        document.getElementById("empRestaurants").style = "block"
        var location = `https://localhost:7091/RestaurantEntity?restaurantId=${localStorage.getItem("restaurantData")}`
        $.get(location, function (data) {
            var restaurantName = data.item3
            var restaurantAddy = data.item4
            var restaurantHours = data.item5
            var restaurantDesc = data.item6
            $('#childTable').find('tbody').append(`<tr><td><button onclick="revealEmployeeAdd()" class="btn btn-warning">Add Employee</button></td><td>${restaurantName}</td><td>${restaurantAddy}</td><td>${restaurantHours}</td><td>${restaurantDesc}</td></tr>`);
        })
    }
    else {
        window.alert("You aren't an employee of any restaurants")
    }

}

function createReview() {
    var order_num = document.getElementById("orderNumCr").value;
    var rating = document.getElementById("ratingCr").value;
    var text = document.getElementById("textCr").value;
    var restaruantId = parseInt(localStorage.getItem("currRes"))
    console.log(order_num, rating, text, restaruantId)
    //https://localhost:7091/ReviewEntityUser?order_num=130&rating=10&text=awesome%20taco&restaurantId=1

    let url = `https://localhost:7091/ReviewEntityUser?order_num=${order_num}&rating=${rating}&text=${text}&restaurantId=${restaruantId}`
    fetch(url, {
        method: "POST",
        headers: {
        "Content-type": "application/json; charset=UTF-8"
        }})
    // window.location.href = "http://127.0.0.1:5501/my-review.html";

}

populateData = async () => {
    //1. get user data
    document.getElementById("firstName").value = localStorage.firstName;
    document.getElementById("lastName").value = localStorage.lastName;
    document.getElementById("email").value = localStorage.email;
    document.getElementById("password").value = localStorage.password;

    //2. get address data

    //3. Get payment
    var userId = parseInt(localStorage.getItem("id"))
    var url = `https://localhost:7091/PaymentEntity?userId=${2}`
    $.get(url, function (data) {
        //populate payment data
        console.log(data[0].item6)
        document.getElementById("number").value = data[0].item6
        document.getElementById("expdate").value = data[0].item3
        document.getElementById("passw").value = data[0].item4
        document.getElementById("method").value = data[0].item2
    })
    // //show user current payments and option to add new payment
    // document.getElementById("paymentContainer").style = "block"
}
populatePayment = async () => {
    //1. get user current payments
    //1.a. get curr user
    var userId = parseInt(localStorage.getItem("id"))
    //1.b. fetch request to get payment data
    var url = `https://localhost:7091/PaymentEntity?userId=${2}`
    $.get(url, function (data) {
        //populate payment data
        console.log(data[0].item6)
        document.getElementById("number").value = data[0].item6
        document.getElementById("expdate").value = data[0].item3
        document.getElementById("passw").value = data[0].item4
        document.getElementById("method").value = data[0].item2
    })
    //show user current payments and option to add new payment
    document.getElementById("paymentContainer").style = "block"
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
    var months = {
        jan: '01', feb: '02', mar: '03', apr: '04', may: '05', jun: '06',
        jul: '07', aug: '08', sep: '09', oct: '10', nov: '11', dec: '12'
    };
    function pad(n) { return (n < 10 ? '0' : '') + +n; }

    return t[3] + '-' + months[t[1].toLowerCase()] + '-' + (parseInt(t[2]) + 1)
}
function test() {
    event.preventDefault();
}


$(document).ready(function () {
    $("#reservationButton").click(function () {
        var restaurantNum = document.getElementById("restaurant").value;
        var f = $('#checkin_date').data().datepicker.viewDate;
        startDate = stringToTimestamp(f.toString())
        url = `https://localhost:7091/ReservationEntity/checkReservations?restaurantId=${restaurantNum}&startDate=${startDate}`
        $.get(url, function (data) {
            var T = document.getElementById("reservationTimes");
            T.style.display = "block";
            var select = document.getElementById("restaurantReservationTimes");

            for (var option of data) {
                var el = document.createElement("option");
                el.textContent = option;
                el.value = option;
                select.appendChild(el);
            }
        })
    });
});

var i = 0;
function childrenRow() {
    i++;
    $('#childTable').find('tbody').append('<tr><th scope="row">' + i + '</th><td class="col-sm-4"><input type="text" name="name" class="form-control" /></td><td><input type="text" name="school" class="form-control" /></td><td class="col-sm-2"><input type="text" name="year" class="form-control" /></td><td class="col-sm-2"><input type="text" name="age" class="form-control" /></td><td><input type="button" class="btn btn-block btn-default" id="addrow" onclick="childrenRow()" value="+" /></td></tr>');
}
function convertTime12To24(time) {
    var hours = Number(time.match(/^(\d+)/)[1]);
    var minutes = Number(time.match(/:(\d+)/)[1]);
    var AMPM = time.match(/\s(.*)$/)[1];
    console.log(AMPM)
    if (AMPM === "pm" && hours < 12) hours = hours + 12;
    if (AMPM === "am" && hours === 12) hours = hours - 12;
    var sHours = hours.toString();
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
    url = `https://localhost:7091/ReservationEntity/createReservation?reservationPartySize=${numPeople}&reservationDateTime=${formattedDateTime}&restaurantId=${restaurantNum}&reservationMaker=${localStorage.getItem("id")}`
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

async function grabReviewData(url) {
    url = "https://localhost:7091/ReviewEntityUser?restaurantId=1";
    // fetch(url).then(response => 
    //     response.json().then(data => ({
    //         data: data,
    //         status: response.status
    //     })
    // ).then(res => {
    //     // console.log(res.status, res.data.title)
    //     console.log(res)
    //         localStorage.id = res.data.item2
    //         console.log(localStorage.id)
    //         firstName = res.data.item3
    //         lastName = res.data.item4
    //         window.alert("Welcome, " + firstName + " " + lastName + "!");
    //         window.location.replace("./index.html");
        
        
    // }));
    const response = await fetch(url);
    console.log(response.json());
    return response.json();
}



function editFunction() {
    var commentId = document.getElementById("commentEditId").value;
    var rating = document.getElementById("rating").value;
    var feedback = document.getElementById("feedback").value;
    console.log(commentId+" "+rating+" "+feedback);
    let url = `https://localhost:7091/ReviewEntityUser?comment_id=${commentId}&rating=${rating}&feedback=${feedback}`
    fetch(url, {
    method: "PUT",
    headers: {
    "Content-type": "application/json; charset=UTF-8"
    }})
    window.location.reload();
    }

function delFunction() {
    var commentId = document.getElementById("commentDelId").value;
    console.log(commentId);
    //https://localhost:7091/ReviewEntityUser?comment_id=10

    let url = `https://localhost:7091/ReviewEntityUser?comment_id=${commentId}`
    fetch(url, {
    method: "DELETE",
    headers: {
    "Content-type": "application/json; charset=UTF-8"
    }})
    window.location.reload();
    }

    function delEmpFunction() {
        if(localStorage.getItem("value"))
            {
        var commentId = document.getElementById("commentDelEmpId").value;
        console.log(commentId);
        //https://localhost:7091/ReviewEntityEmployee?comment_id=1&comment=none&employee_id=1
        let url = `https://localhost:7091/ReviewEntityEmployee?comment_id=${commentId}&comment=${"none"}&employee_id=-1`
        fetch(url, {
        method: "POST",
        headers: {
        "Content-type": "application/json; charset=UTF-8"
        }})
        window.location.reload();
    }
    }

    function editEmpFunction(restaurantId) {
        if(localStorage.getItem("value"))
        {
        var commentId = document.getElementById("commentEditId").value;
        var comment = document.getElementById("commentEmp").value;
        var employee_id = -1;
        if(restaurantId == 1)
            employee_id = 2;
        else if(restaurantId ==2)
            employee_id = 4;
        else if(restaurantId == 3)
            employee_id = 6;
        else employee_id = 8;
        let url = `https://localhost:7091/ReviewEntityEmployee?comment_id=${commentId}&comment=${comment}&employee_id=${employee_id}`
        fetch(url, {
        method: "POST",
        headers: {
        "Content-type": "application/json; charset=UTF-8"
        }})
        window.location.reload();
    }
}

function goToRestaurant()
{
    var restaurantId = localStorage.getItem("restaurantData");
    console.log(restaurantId);
    if(restaurantId == 1)
    window.location.href = "http://127.0.0.1:5501/fuegos.html";

        else if(restaurantId ==2)
        window.location.href = "http://127.0.0.1:5501/laynes.html";

        else if(restaurantId == 3)
        window.location.href = "http://127.0.0.1:5501/Mess.html";

        else window.location.href = "http://127.0.0.1:5501/nams.html";

}

function deleteReservation() {
    if(localStorage.getItem("value"))
        {
    var reservation_id = document.getElementById("reservationDelId").value;
    console.log(reservation_id);
    //https://localhost:7091/ReservationEntity?reservation_id=-1
    //https://localhost:7091/ReviewEntityEmployee?comment_id=1&comment=none&employee_id=1
    let url = `https://localhost:7091/ReservationEntity?reservation_id=${reservation_id}`
    fetch(url, {
    method: "DELETE",
    headers: {
    "Content-type": "application/json; charset=UTF-8"
    }})
    window.location.reload();
}
}

function editReservation() {
    if(localStorage.getItem("value"))
    {
    var reservationId = document.getElementById("reservationEditId").value;
    var partySize = document.getElementById("partySize").value;
    var employee_id = -1;
    console.log(reservationId+" "+partySize);
    //https://localhost:7091/ReservationEntity?reservation_id=1&party_size=9
    //https://localhost:7091/ReservationEntity?reservation_id=2&party_size=499

    let url = `https://localhost:7091/ReservationEntity?reservation_id=${reservationId}&party_size=${partySize}`
    fetch(url, {
    method: "PUT",
    headers: {
    "Content-type": "application/json; charset=UTF-8"
    }})
    window.location.reload();
}
}

function editReservationCustomer() {
    if(localStorage.getItem("value"))
    {
    var reservationId = document.getElementById("reservationEditId2").value;
    var customerId = document.getElementById("customerReservationId").value;
    var employee_id = -1;
    console.log(reservationId+" "+customerId);
    //https://localhost:7091/ReservationEntity?reservation_id=1&party_size=9
    //https://localhost:7091/ReservationEntity?reservation_id=2&party_size=499
        //https://localhost:7091/ReservationEntity/changeReservationOwner?reservation_id=36&customer_id=12

    let url = `https://localhost:7091/ReservationEntity/changeReservationOwner?reservation_id=${reservationId}&customer_id=${customerId}`
    fetch(url, {
    method: "PUT",
    headers: {
    "Content-type": "application/json; charset=UTF-8"
    }})
    window.location.reload();
}
}

function editReadyTime() {
    if(localStorage.getItem("value"))
    {
    var order_num = document.getElementById("orderId").value;
    
    console.log(order_num);
    //https://localhost:7091/ReservationEntity?reservation_id=1&party_size=9
    //https://localhost:7091/ReservationEntity?reservation_id=2&party_size=499
        //https://localhost:7091/ReservationEntity/changeReservationOwner?reservation_id=36&customer_id=12

    let url = `https://localhost:7091/PlacedOrderEntity/updateReadyTime?order_num=${order_num}`
    fetch(url, {
    method: "PUT",
    headers: {
    "Content-type": "application/json; charset=UTF-8"
    }})
    window.location.reload();
}
}
function deletePlacedOrder() {
    if(localStorage.getItem("value"))
    {
    var order_num = document.getElementById("orderIdDel").value;
    
    console.log(order_num);
    //https://localhost:7091/ReservationEntity?reservation_id=1&party_size=9
    //https://localhost:7091/ReservationEntity?reservation_id=2&party_size=499
        //https://localhost:7091/ReservationEntity/changeReservationOwner?reservation_id=36&customer_id=12

    let url = `https://localhost:7091/PlacedOrderEntity/deletePlacedOrder?order_num=${order_num}`
    fetch(url, {
    method: "DELETE",
    headers: {
    "Content-type": "application/json; charset=UTF-8"
    }})
    //window.location.reload();
}
}


//https://localhost:7091/PlacedOrderEntity/updateReadyTime?order_num=3
//https://localhost:7091/PlacedOrderEntity/deletePlacedOrder?order_num=82


