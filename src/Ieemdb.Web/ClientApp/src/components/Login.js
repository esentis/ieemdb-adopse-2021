import React, { useEffect } from 'react';
import { useState } from 'react';
import {Col,Form,Button} from 'react-bootstrap';
import '../Styles/Login.css';
import {useUpdatePage} from './GlobalContext';


function Login(props) {
    const [userName, setUserName] = useState("");
    const [password, setPassword] = useState("");
    
    const [userNameReg, setUserNameReg] = useState("");
    const [passwordReg, setPasswordReg] = useState("");

    const handleSubmit = (evt) => {
      evt.preventDefault();
      alert(`Submitting Name ${userName} ${password}`);
    }

    const handleSubmitRegister = (evt) => {
      evt.preventDefault();
      alert(`Submitting Name ${userNameReg} ${passwordReg}`);
    }

    const setPage=useUpdatePage();
    useEffect(() => {
        setPage("1")})

    return (
       <Col className='column-right-Login'>
       <div style={{color:'white'}}>
          <form onSubmit={handleSubmit}>
            <label>
              UserName:
            <input
                type="text"
                value={userName}
                onChange={e => setUserName(e.target.value)}
              />
            </label>
            <label>
              Password:
            <input
                type="text"
                value={password}
                onChange={e => setPassword(e.target.value)}
              />
            </label>
            <input type="submit" value="Submit" />
          </form>
          <br/>

            <form onSubmit={handleSubmitRegister}>
              <label>
                UserNameReg:
              <input
                  type="text"
                  value={userNameReg}
                  onChange={e => setUserNameReg(e.target.value)}
              />
              </label>
              <label>
                PasswordReg:
                <input
                    type="text"
                    value={passwordReg}
                    onChange={e => setPasswordReg(e.target.value)}
                />
              </label>
            <input type="submit" value="Submit" />
           </form>
       </div>
       </Col>
    )
}

export default Login;
