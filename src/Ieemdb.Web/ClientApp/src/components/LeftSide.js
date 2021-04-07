import React,{useState} from "react";
import {Container,Col} from 'react-bootstrap';
import '../NavBar.css'
import SearchBar from './SearchBar';
import {Link} from 'react-router-dom';
import '../NavBar.css'
import  * as FaIcons  from "react-icons/fa";
import {usePage,useUpdatePage} from './Navigate'

 


function LeftSide(){
    const isLoggedin=true;
    const page=usePage();   
    const setPage=useUpdatePage();
    const username='Username'
        
    
    return(
    <Col className="column-left" md={2}>
    <label for='check' className='checkbtn'><FaIcons.FaBars /></label>
         <input type='checkbox' Id='check'>  
        </input>
        <Container fluid className="nav-center2">
            <nav>
            <SearchBar/>
            {isLoggedin ? <button className='advButton'>Advanced</button> : ""}
            <ul>
            {isLoggedin ? <Link to='#' name='Favorites' onClick={()=>setPage("Favorites")}>
            <FaIcons.FaStar className='fa-cog' />
            <span>Favorites</span>
            </Link> :" " }
            {isLoggedin ? <Link to='#' name='WatchList' onClick={()=>setPage("WatchList")}>
            <FaIcons.FaList className='fa-cog' />
            <span>Watch List</span>
            </Link> :" " }
            {isLoggedin?<div className="dropdown">
            <button className="dropbtn">{username}</button>
            <div className="dropdown-content">
            <Link>
            <FaIcons.FaUserCog className='fa-cog' />
            <span>Settings</span>
            </Link>
            <Link>
            <FaIcons.FaSignOutAlt className='fa-cog' />
            <span>Logout</span>
            </Link>
            </div>
            </div> : <button className='login-Button'>Login</button> }
            </ul>
        </nav>
        </Container>
    </Col>    
    );
}

export default LeftSide;