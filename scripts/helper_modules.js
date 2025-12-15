export function handle_registration(response) {
    if(response != "False"){
        // localStorage.setItem('token', response);
        alert("Профіль був успішно створений!")
    }
    else
    {
        alert("Профіль вже існує")
    }
}

export function check_token_validity() {
    const token = cookieStore.get("AuthToken")
    if(token != null)
    {
      fetch("http://localhost:8080/check_auth", {
        method: "POST",
        credentials: "include",
        body: token,
      })
      .then((e) => (e.text()))
      .then((res) => alert(res))
    }
}


export function get_token() {
    fetch("http://localhost:8080/check_auth", {
        method: "GET",
        credentials: "include", // Важливо для відправки cookies
        headers: {
            "Accept": "application/json"
        }
    })
}

export function save_storage(json_save) {
    const userData = JSON.parse(json_save);
    for (let key in userData) {
        if (userData.hasOwnProperty(key)) {
            if(key != "token" && key != "password")
            {
                localStorage.setItem(key, userData[key])
            }
        }
    }
}


// const data = JSON.parse(json_save)
// // json_save.forEach(element => {
// //     localStorage.setItem(element, "")
// // });
// data.forEach(element => {
//     alert(element)
// });