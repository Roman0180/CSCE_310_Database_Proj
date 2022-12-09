function test() {
    event.preventDefault();
}
editReservation = async(id) => {
    var restaurantId = localStorage.getItem(id);
    var partySize = prompt("Enter your group size", "Party Size");
    url = `https://localhost:7091/ReservationEntity?reservation_id=${parseInt(restaurantId)}&party_size=${parseInt(partySize)}`
    const settings = {
        method: 'PUT',
        headers: {
            Accept: 'application/json',
            'Content-Type': 'application/json',
        }
    };
    try {
        const fetchResponse = await fetch(url, settings);
        location.reload()
    } catch (e) {
        return e;
    }


}

cancelReservation = async(id) =>  {
    var restaurantId = localStorage.getItem(id)
    var url = `https://localhost:7091/ReservationEntity?reservation_id=${restaurantId}`
    console.log(url)
    const settings = {
        method: 'DELETE',
        headers: {
            Accept: 'application/json',
            'Content-Type': 'application/json',
        }
    };
    try {
        const fetchResponse = await fetch(url, settings);
        location.reload()
    } catch (e) {
        return e;
    }


}
getAllReservations = async() => {
    url = `https://localhost:7091/ReservationEntity/getAllReservations?userId=${localStorage.getItem("id")}`
        $.get(url, function (data) {
            var restaurants = {1: "Fuego Tortilla Grill", 2: "Layne's", 3:"MESS Waffles, Etc.", 4:"Nam Cafe"}
            for(let i = 0; i < data.length; i++){
                var restaurant = restaurants[data[i].item3]
                var party = data[i].item1
                var date = data[i].item2
                var reservationId = parseInt(data[i].item4)
                localStorage.setItem(date, reservationId)
                $('#userResevationTable').find('tbody').append(`<tr><td><button id=${date} onclick="cancelReservation(this.id)" class="btn btn-danger">Cancel Reservation</button><button id=${date} onclick="editReservation(this.id)" class="btn btn-warning">Edit Reservation</button></td><td>${restaurant}</td><td>${date}</td><td>${party}</td></tr>`);
            }
            //<button id=${date} onclick="editReservation(this.id)" class="btn btn-warning">Edit Reservation</button>
            
        })
    
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

function userLogOut() {
    localStorage.clear()
    console.log("storage cleared")
}

