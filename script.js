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
            localStorage.id = res.data.item2
            console.log(localStorage.id)
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

function delFunction() {
    var commentId = document.getElementById("commentDelId").value;
    console.log(commentId);
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