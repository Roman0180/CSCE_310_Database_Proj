firstName = "firstName"
lastName = "LastName"

function userInfo(firstName, lastName){
    return firstName, lastName
}

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
            firstName = res.data.item3
            lastName = res.data.item4
            userInfo(firstName, lastName)
            window.location.replace("./loggedInIndex.html");
        }
        else{
            window.alert("Wrong username and/or password. Try again.");
        }
    }));
}