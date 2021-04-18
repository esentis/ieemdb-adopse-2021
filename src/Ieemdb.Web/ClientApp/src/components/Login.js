import React,{useEffect} from 'react'
import {Col} from 'react-bootstrap';
import '../Styles/Login.css';
import {useUpdatePage} from './GlobalContext';


function Login(props) {
    const setPage=useUpdatePage();
    useEffect(() => {
        setPage("1")})
    
    return (
       <Col className='column-right-Login'>
       <div style={{color:'white'}}>
       <p>Login Page</p>
       </div>
       </Col>
    )
}

export default Login;
