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
    const [LoginState,setLoginState]=useState(true);
    
    const page=usePage();   
    const setPage=useUpdatePage();

    return(
    <Col className="column-left" xl={2} style={{position:'fixed'}} >
    <label htmlFor='check' className='checkbtn'><FaIcons.FaBars /></label>
         <input type='checkbox' id='check'>  
        </input>

        <img src={logo} className='logo' onClick={()=>setPage("Home")}  />
        <Container fluid className="nav-center2">
            <nav>
            <SearchBar/>
            <span>{LoginState ? <button className='advButton'>Advanced</button> : ""}</span>
            <ul>
            {LoginState ? <Link to='#' name='Favorites' onClick={()=>setPage("Favorites")}>
            <FaIcons.FaStar className='fa-cog' />
            <span>Favorites</span>
            </Link> :" " }
            {LoginState ? <Link to='#' name='WatchList' onClick={()=>setPage("WatchList")}>
            <FaIcons.FaList className='fa-cog' />
            <span>Watch List</span>
            </Link> :" " }
            {LoginState?<div>
            <Link to='#'>
            <FaIcons.FaUserCog className='fa-cog' />
            <span>Settings</span>
            </Link>
            <Link to='#' onClick={()=>setLoginState(false)}>
            <FaIcons.FaSignOutAlt className='fa-cog'  />
            <span>Logout</span>
            </Link>
            </div>: 
            <Link to='#' onClick={()=>{setPage("LoginPage"); setLoginState(true)}}>
            <FaIcons.FaSignInAlt className='fa-cog'/>
            <span>Login</span>
            </Link>}
            </ul>
        </nav>
        </Container>
    </Col>    
    );
}

export default LeftSide;