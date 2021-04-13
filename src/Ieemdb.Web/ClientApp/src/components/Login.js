import React from 'react'
import {Col} from 'react-bootstrap';
import '../Styles/Login.css'


function Login(props) {
    return (
       <Col className='column-right-Login'>
       <div style={{color:'white'}}>
       <p>{props.name}</p>
       </div>
       </Col>
    )
}

export default Login
