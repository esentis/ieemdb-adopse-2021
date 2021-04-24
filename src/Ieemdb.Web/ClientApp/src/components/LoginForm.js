import React, { useEffect } from 'react';
import { useState } from 'react';
import axios from 'axios';

function LoginForm() {
  const [userNameLogin, setUserNameLogin] = useState("");
  const [passwordLogin, setPasswordLogin] = useState("")
  const [deviceName, setDeviceName] = useState("");

  const handleSubmitLogin = async (evt) => {
    evt.preventDefault();

    axios.post('https://localhost:5001/api/account/login', {
      userName: userNameLogin,
      password: passwordLogin,
      deviceName: deviceName
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
      <label>
        UserNameLogin:
            <input
          type="text"
          value={userNameLogin}
          onChange={e => setUserNameLogin(e.target.value)}
        />
      </label>
      <label>
        PasswordLogin:
            <input
          type="text"
          value={passwordLogin}
          onChange={e => setPasswordLogin(e.target.value)}
        />
      </label>
      <label>
        Device:
            <input
          type="text"
          value={deviceName}
          onChange={e => setDeviceName(e.target.value)}
        />
      </label>
      <input type="submit" value="Submit" />
    </form>
  );
}

export default LoginForm;
