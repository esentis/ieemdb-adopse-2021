import React, { useEffect } from 'react';
import { useState } from 'react';
import {Col,Form,Button} from 'react-bootstrap';
import '../Styles/Login.css';
import { useUpdatePage } from './GlobalContext';


function Login(props) {
    const [userName, setUserName] = useState("");
    const [password, setPassword] = useState("");
    const [email, setEmail] = useState("");

    const [userNameLogin, setUserNameLogin] = useState("");
    const [passwordLogin, setPasswordLogin] = useState("");
    const [deviceName, setDeviceName] = useState("");

    const [firstName, setFirstName] = useState("");
    const [lastName, setLastName] = useState("");
    const [birthDate, setBirthDate] = useState("");
    const [bio, setBio] = useState("");


  const handleSubmitRegister = async (evt) => {
    evt.preventDefault();
    alert(`Submitting Name ${userName} ${password} ${email}`);

    const response = await fetch('https://localhost:5001/api/account', {
      method: 'POST',
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify({
        userName,
        email,
        password
      })
    });
    const content = await response.json();
    console.log(content);
  }
  const handleSubmitActor = async (evt) => {
    evt.preventDefault();
    const response = await fetch('https://localhost:5001/api/actor', {
      method: 'POST',
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify({
        firstName,
        lastName,
        birthDate,
        bio
      })
    });
  }

  const handleSubmitLogin = async (evt) => {
    evt.preventDefault();
    const response = await fetch('https://localhost:5001/api/account/login', {
      method: 'POST',
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify({
        userName: userNameLogin,
        password: passwordLogin,
        deviceName: deviceName
      })
    });
    const content = await response.json();
    localStorage.setItem('toTokenRE', content.accessToken);
    console.log(content);
    console.log(localStorage.getItem('toTokenRE'));
  }

  async function getActors() {
  const actors = await fetch('https://localhost:5001/api/actor', { method: 'GET' })
    .then(res => res.json())
    .then(console.log);
  }
   
    const setPage=useUpdatePage();
    useEffect(() => {
        setPage("1")})

    return (
       <Col className='column-right-Login'>
       <div style={{color:'white'}}>
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
          </form>
          <br/>
          <form onSubmit={handleSubmitActor}>
            <label>
              ActorFirstname:
              <input
                type="text"
                value={firstName}
                onChange={e => setFirstName(e.target.value)}
              />
            </label>
            <label>
              actorlastname:
                <input
                type="text"
                value={lastName}
                onChange={e => setLastName(e.target.value)}
              />
            </label>
            <label>
              birthdate:
                  <input
                type="text"
                value={birthDate}
                onChange={e => setBirthDate(e.target.value)}
              />
            </label>
            <label>
              bio:
                  <input
                type="text"
                value={bio}
                onChange={e => setBio(e.target.value)}
              />
            </label>
            <input type="submit" value="InputActor" />
          </form> 
          <br/>
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
          <Button onClick={getActors}>getActors</Button>
       </div>
       </Col>
     )
}

export default Login;
