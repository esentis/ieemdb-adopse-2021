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
      return <Button onClick={changeForm}>Register here</Button>;
    }
    else {
      return <Button onClick={changeForm}>Login here</Button>;
    }
  }

  function DecideP() {
    if (stateRegister == false) {
      return <p>Don't have an account?</p>;
    }
    else {
      return <p>Already have an account?</p>;
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
