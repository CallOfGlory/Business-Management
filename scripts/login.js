const form = document.getElementById('loginForm');
const email = document.getElementById('email');
const username = document.getElementById('username');
const password = document.getElementById('password');
const togglePassword = document.getElementById('togglePassword');


function validateEmail(value){
  // simple email validation
  return /^[^\s@]+@[^\s@]+\.[^\s@]+$/.test(value);
}

togglePassword?.addEventListener('click', ()=>{
  if(password.type === 'password'){
    password.type = 'text';
    togglePassword.setAttribute('aria-label', 'Приховати пароль');
    togglePassword.textContent = '🙈';
  } else {
    password.type = 'password';
    togglePassword.setAttribute('aria-label', 'Показати пароль');
    togglePassword.textContent = '👁️';
  }
});

form?.addEventListener('submit', (e)=>{
  e.preventDefault();
  if(validateEmail(email.value) == true) {
    if(username.value != "" && email.value != "" && password.value != "") {
      let final_data = {
          "username": username.value,
          "email": email.value,
          "password": password.value
      }
      try
      {
        fetch("http://localhost:8080/login", {
            method: "POST",
            body: JSON.stringify(final_data),
        })
        .then((e) => e.text())
        .then((ans) => alert(ans))
      }
      catch(e) {
        alert(e)
      }
    }
  }
});