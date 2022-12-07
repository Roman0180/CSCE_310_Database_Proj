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
    itemPrice = itemPrice.substring(1)
    localStorage.setItem(itemName, itemPrice);

    // for (var i = 0; i < localStorage.length; i++){
    //     if(!(localStorage.key(i) == "id" | localStorage.key(i) == "value")){
    //         console.log(localStorage.key(i) + " in cart " + localStorage.getItem(localStorage.key(i)))
    //     }
    // }
}

function showCartItems(){
    var completelist = document.getElementById("thelist");

    for (var i = 0; i < localStorage.length; i++){
        if(!(localStorage.key(i) == "id" || localStorage.key(i) == "value")){
            j++
            completelist.innerHTML += "<li class='list-group-item'> <input class='form-check-input me-1' type='checkbox' value=''>" + localStorage.key(i) + " $" + localStorage.getItem(localStorage.key(i)) + "</li>";
            price = parseFloat(localStorage.getItem(localStorage.key(i)));
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
            let seperator = cboxes[i].offsetParent.innerText.indexOf("$")
            var item = cboxes[i].offsetParent.innerText.substring(0, seperator-1)
            var price = cboxes[i].offsetParent.innerText.substring(seperator)
            localStorage.removeItem(item)
            console.log("removing " + item + " from cart")
        }
    }
    location.reload()
}