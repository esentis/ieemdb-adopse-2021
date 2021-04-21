import React, { useEffect } from 'react';
import { useState } from 'react';
import {Col,Form,Button} from 'react-bootstrap';
import '../Styles/Login.css';
import { useUpdatePage } from './GlobalContext';
import token from '../App.js';


function Login(props) {
    const [userName, setUserName] = useState("");
    const [password, setPassword] = useState("");
    const [email, setEmail] = useState("");

  const handleSubmitRegister = async (evt) => {
    evt.preventDefault();
    alert(`Submitting Name ${userName} ${password} ${email}`);

    /*const response = await fetch('https://localhost:5001/api/account', {
      method: 'POST',
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify({
        userName,
        email,
        password
      })
    });*/
    const response = await fetch('https://localhost:5001/api/account', {
      method: 'POST',
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify({
        userName,
        email,
        password
      })
    })
      .then(
        function (response) {
          if (response.status !== 200) {
            console.log('Looks like there was a problem. Status Code: ' +
              response.status);
            return;
          }

          // Examine the text in the response
          response.json().then(function (data) {
            console.log(data);
          });
        }
      )
      .catch(function (err) {
        console.log('Fetch Error :-S', err);
      });
    
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
       </div>
       </Col>
     )
}

export default Login;
