import { save_storage } from "./helper_modules.js";

const form = document.getElementById('registerForm');
const full_name = document.getElementById('fullName');
const email = document.getElementById('email');
const username = document.getElementById('username');
const password_1 = document.getElementById("password");
const password_2 = document.getElementById("confirmPassword");
const company = document.getElementById("company");
const phome = document.getElementById("phone");
const terms = document.getElementById("terms");


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

toggleConfirmPassword?.addEventListener('click', ()=>{
  if(confirmPassword.type === 'password'){
    confirmPassword.type = 'text';
    toggleConfirmPassword.setAttribute('aria-label', 'Приховати пароль');
    toggleConfirmPassword.textContent = '🙈';
  } else {
    confirmPassword.type = 'password';
    toggleConfirmPassword.setAttribute('aria-label', 'Показати пароль');
    toggleConfirmPassword.textContent = '👁️';
  }
});


form.addEventListener("submit", (e) => {
    e.preventDefault()
    if(terms.checked == true)
    {
        termsError.innerHTML = ""
        if(check_filling())
        {
            if(password_1.value == password_2.value){
                passwordError.innerHTML = ""
                let final_data = form_json()
                fetch("http://localhost:8080/register", {
                    method: "POST",
                    credentials: "include",
                    headers: {
                      "Content-Type": "application/json",
                    },
                    body: JSON.stringify(final_data),
                })
                .then((e) => e.text())
                .then((ans) => handle_registration(ans))
            }
            if(password_1.value != password_2.value)
            {
                passwordError.innerHTML = "Паролі не збігаютсья!"
            }
        }
    }
    else
    {
        termsError.innerHTML = "Умови використання не були прийняті!"
    }
});

function check_filling() {
    if(full_name.value != "" && email.value != "" && username.value != "" && password_1.value != "" && password_2.value != "" && company.value != "" && phome.value != "")
    {
        return true;
    }
    else
    {
        return false;
    }
}

function form_json()
{
    let final_data = {
            "username": username.value,
            "full_name": full_name.value,
            "email": email.value,
            "password": password_1.value,
            "phone": phome.value
        }

    if(company.value != "") {
        final_data["company"] = company.value
    }

    return final_data
}

 