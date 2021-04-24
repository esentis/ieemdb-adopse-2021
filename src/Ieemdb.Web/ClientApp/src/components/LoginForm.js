import React, { useEffect } from 'react';
import { useState } from 'react';
import axios from 'axios';
import '../Styles/Forms.css'
import { Row, Col } from 'react-bootstrap';

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
        localStorage.setItem('token', res.data.accessToken);
      }
    });
  }

  return (
    <form onSubmit={handleSubmitLogin}>
      <label className="centered">
        Username :
            <input
          type="text"
          value={userNameLogin}
          onChange={e => setUserNameLogin(e.target.value)}
        />
        </label>
      <label className="centered">
        Password :
            <input
          type="password"
          value={passwordLogin}
          onChange={e => setPasswordLogin(e.target.value)}
        />
      </label>
      <input type="submit" value="Login" />
    </form>
  );
}

export default LoginForm;
