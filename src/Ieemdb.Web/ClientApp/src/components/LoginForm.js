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
      // console.log(res);
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
    <div className="backForm">
      <label className="formTitle">Login to your account</label>
      <form className="divForm" onSubmit={handleSubmitLogin}>
        <label className="formText">Username</label>
        <input className="formInput" type="text" placeholder="Your username here" value={userNameLogin} onChange={e => setUserNameLogin(e.target.value)}/>
        <label className="formText">Password</label>
        <input className="formInput" type="password" placeholder="Your password here" value={passwordLogin} onChange={e => setPasswordLogin(e.target.value)}/>
        <input className="formButton" type="submit" value="Login" />
      </form>
    </div>
  );
}
export default LoginForm;