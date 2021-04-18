import React,{useEffect} from 'react'
import {Col} from 'react-bootstrap';
import '../Styles/UserSettings.css'
import {useUpdatePage} from './GlobalContext'


function UserSettings() {

    const setPage=useUpdatePage();
    useEffect(() => {
        setPage("1")})
    return (
       <Col className='column-right-User'>
       <div style={{color:'white'}}>
       <p>User Settings</p>
       </div>
       </Col>
    )
}

export default UserSettings;