import React, { useState } from 'react';
import axios from 'axios';
import '../Styles/Forms.css';
/*import auth from './auth';*/
import { useHistory } from 'react-router-dom';
import {/*useLoginState,*/ useChangeLoginState } from './GlobalContext';


function LoginForm() {
  const [userNameLogin, setUserNameLogin] = useState("");
  const [passwordLogin, setPasswordLogin] = useState("")
  /*const [deviceName, setDeviceName] = useState("");*/
  const history = useHistory();
  const isLoggedIn = useChangeLoginState();

  const handleSubmitLogin = async (evt) => {
    evt.preventDefault();
    const url = 'https://' + window.location.host + '/api/account/login';

    axios.post(url, {
      userName: userNameLogin,
      password: passwordLogin,
      deviceName: 'fdsfadsfas' //na perasw deviceName
    }).then(function (res) {
      console.log(res);
      //console.log(res.data.accessToken);
      //console.log(res.status);
      if (res.status === 200) {
        localStorage.setItem('token', res.data.accessToken);
        isLoggedIn(true);
        history.push("/");
      }
    });

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
