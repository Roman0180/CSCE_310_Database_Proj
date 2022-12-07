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
    // console.log(itemNum)

    var itemName = document.getElementById(id).textContent;
    var itemPrice = document.getElementById(price).textContent;

    // item = {itemName, itemPrice}
    localStorage.setItem(itemName, itemPrice);
    // localStorage.setItem(JSON.stringify(item));

    for (var i = 0; i < localStorage.length; i++){
        if(!(localStorage.key(i) == "id" | localStorage.key(i) == "value")){
            console.log(localStorage.key(i) + " in cart " + localStorage.getItem(localStorage.key(i)))
        }
    }
}

function showCartItems(){
    for (var i = 0; i < localStorage.length; i++){
        if(!(localStorage.key(i) == "id" | localStorage.key(i) == "value")){
            console.log(localStorage.key(i) + " " + localStorage.getItem(localStorage.key(i)))
        }
    }
}

function deleteFromCart(){
    localStorage.removeItem(itemName);
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