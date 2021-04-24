import React, { useEffect } from 'react';
import { useState } from 'react';
import {Col,Button} from 'react-bootstrap';
import '../Styles/Login.css';
import { useUpdatePage } from './GlobalContext';
import { useLoginState } from './GlobalContext';
import LoginForm from './LoginForm';
import RegisterForm from './RegisterForm';


function Login(props) {
  const [stateRegister, setStateRegister] = useState(false);

  function DecideForm() {
    if (stateRegister == false) {
      return <LoginForm />;
    }
    else {
      return <RegisterForm />;
    }
  }

  function DecideButton() {
    if (stateRegister == false) {
      return <Button onClick={changeForm}>Login</Button>;
    }
    else {
      return <Button onClick={changeForm}>Register</Button>;
    }
  }

  function DecideP() {
    if (stateRegister == false) {
      return <p>Don't have an account? Register HERE:</p>;
    }
    else {
      return <p>Already have an account? Login HERE:</p>;
    }
  }

  function changeForm() {
    setStateRegister(!stateRegister);
  }

    const setPage=useUpdatePage();
    useEffect(() => {
        setPage("1")})
    
    return (
      <Col className='column-right-Login'>
        <DecideForm />
        <DecideP />
        <DecideButton/>
      </Col>
     )
}

export default Login;
