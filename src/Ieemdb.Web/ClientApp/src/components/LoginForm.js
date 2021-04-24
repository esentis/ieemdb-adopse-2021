import React, { useEffect } from 'react';
import { useState } from 'react';
import axios from 'axios';
import '../Styles/Forms.css';
import auth from './auth';

function LoginForm() {
  const [userNameLogin, setUserNameLogin] = useState("");
  const [passwordLogin, setPasswordLogin] = useState("")
  const [deviceName, setDeviceName] = useState("");

  const handleSubmitLogin = async (evt) => {
    evt.preventDefault();

    axios.post('https://localhost:5001/api/account/login', {
      userName: userNameLogin,
      password: passwordLogin,
      deviceName: 'fdsfadsfas' //na perasw deviceName
    }).then(function (res) {
      console.log(res);
      console.log(res.data.accessToken);
      console.log(res.status);
      if (res.status == 200) {
        //local storage to token
        auth.login(); // o xrhsths einai authenticated
        localStorage.setItem('token', res.data.accessToken);
      }
    });

    if (auth.isAuthenticated()) {
      //show menu buttons kai pane sto main page
    }
  }

  return (
    <form className="centeredFields" onSubmit={handleSubmitLogin}>
      <label className="centeredText">
        Username
       </label>
      <input
          type="text"
          value={userNameLogin}
          onChange={e => setUserNameLogin(e.target.value)}
      />
      <label className="centeredText">
        Password
      </label>
      <input
          type="password"
          value={passwordLogin}
          onChange={e => setPasswordLogin(e.target.value)}
      />
      <p className="buttonAlign"><input type="submit" value="Login" /></p>
    </form>
  );
}

export default LoginForm;
