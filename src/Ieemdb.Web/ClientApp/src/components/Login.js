import React, { useEffect } from 'react';
import { useState } from 'react';
import {Col,Form,Button} from 'react-bootstrap';
import '../Styles/Login.css';
import axios from 'axios';
import { useUpdatePage } from './GlobalContext';
import { useLoginState } from './GlobalContext';


function Login(props) {
  const [stateRegister, setStateRegister] = useState(false);
    
  const [userName, setUserName] = useState("");
  const [password, setPassword] = useState("");
  const [email, setEmail] = useState("");

  const [userNameLogin, setUserNameLogin] = useState("");
  const [passwordLogin, setPasswordLogin] = useState("")
  const [deviceName, setDeviceName] = useState("");

  function ShowFormLogin(){
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
        <p>Or register here:</p>
        <Button onClick={changeForm}>Register</Button>
      </form>
    );
  }

  function changeForm() {
    setStateRegister(!stateRegister);
  }

  function ShowFormRegister(){
    return (
      <form onSubmit={handleSubmitRegister}>
        <label>
          UserNameReg:
            <input
            type="text"
            value={userName}
            onChange={e => setUserName(e.target.value)}
          />
        </label>
        <label>
          PasswordReg:
            <input
            type="text"
            value={password}
            onChange={e => setPassword(e.target.value)}
          />
        </label>
        <label>
          Email:
          <input
            type="text"
            value={email}
            onChange={e => setEmail(e.target.value)}
          />
        </label>
        <input type="submit" value="Submit" />
        <p>Already Hace Account:</p>
        <Button onClick={changeForm}>Login</Button>
      </form>
    );
  }

  function DecideForm() {
    if (stateRegister==false) {
      return <ShowFormLogin />;
    }
    else {
      return <ShowFormRegister />;
    }
  }

  const handleSubmitRegister = async (evt) => {
    evt.preventDefault();

    axios.post('https://localhost:5001/api/account/login', {
        userName,
        email,
        password
    });
  }

  const handleSubmitLogin = async (evt) => {
    evt.preventDefault();

    axios.post('https://localhost:5001/api/account/login', {
        userName: userNameLogin,
        password: passwordLogin,
        deviceName: deviceName
    }).then(function (res) {
      console.log(res);
      console.log(res.data.accessToken);
    });
  }
   
    const setPage=useUpdatePage();
    useEffect(() => {
        setPage("1")})
    
    return (
       <Col className='column-right-Login'>
        <div style={{ color: 'white' }}>
          <DecideForm/>
        </div>
       </Col>
     )
}

export default Login;
