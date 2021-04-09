import React,{useState} from "react";
import {Container,Col} from 'react-bootstrap';
import '../NavBar.css'
import SearchBar from './SearchBar';
import {Link} from 'react-router-dom';
import '../NavBar.css'
import  * as FaIcons  from "react-icons/fa";
import {usePage,useUpdatePage} from './Navigate'
import logo from '../images/imdb-logo2.png';




 


function LeftSide(){
    const [LoginState,setLoginState]=useState(false);
    
    const page=usePage();   
    const setPage=useUpdatePage();
        
    
    return(
    <Col className="column-left" xl={2}>
    <label for='check' className='checkbtn'><FaIcons.FaBars /></label>
         <input type='checkbox' Id='check'>  
        </input>

        <img src={logo} className='logo' onClick={()=>setPage("Home")}  />
        <Container fluid className="nav-center2">
            <nav>
            <SearchBar/>
            {LoginState ? <button className='advButton'>Advanced</button> : ""}
            <ul>
            {LoginState ? <Link to='#' name='Favorites' onClick={()=>setPage("Favorites")}>
            <FaIcons.FaStar className='fa-cog' />
            <span> Favorites</span>
            </Link> :" " }
            {LoginState ? <Link to='#' name='WatchList' onClick={()=>setPage("WatchList")}>
            <FaIcons.FaList className='fa-cog' />
            <span>Watch List</span>
            </Link> :" " }
            {LoginState?<div>
            <Link>
            <FaIcons.FaUserCog className='fa-cog' />
            <span>Settings</span>
            </Link>
            <Link onClick={()=>setLoginState(false)}>
            <FaIcons.FaSignOutAlt className='fa-cog'  />
            <span>Logout</span>
            </Link>
            </div>: <button className='login-Button' onClick={()=>setLoginState(true)}>Login</button> }
            </ul>
        </nav>
        </Container>
    </Col>    
    );
}

export default LeftSide;