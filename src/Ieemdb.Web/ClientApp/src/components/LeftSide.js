import React,{useState} from "react";
import {Container,Col} from 'react-bootstrap';
import SearchBar from './SearchBar';
import {Link} from 'react-router-dom';
import '../Styles/NavBar.css'
import  * as FaIcons  from "react-icons/fa";
import logo from '../images/imdb-logo2.png';


function LeftSide(){
    const [LoginState,setLoginState]=useState(true);


    return(
    <Col className="column-left" xl={2} style={{position:'fixed'}} >
    <label htmlFor='check' className='checkbtn'><FaIcons.FaBars /></label>
         <input type='checkbox' id='check'>  
        </input>

        <Link to="/"><img src={logo} className='logo' alt=""/></Link>
        <Container fluid className="nav-center2">
            <nav>
            <SearchBar/>
            <span>{LoginState ? <Link to='/AdvancedSearch'><button className='advButton'>Advanced</button></Link> : ""}</span>
            <ul>
            {LoginState ? <Link className='linkClass AdvLink' to='/AdvancedSearch' name='Favorites'><span>Advanced Search</span></Link>:" "}
            {LoginState ? <Link className='linkClass' to='/Favorites' name='Favorites'>
            <FaIcons.FaStar className='fa-cog' />
            <span>Favorites</span>
            </Link> :" " }
            {LoginState ? <Link className='linkClass' to='/WatchList' name='WatchList'>
            <FaIcons.FaList className='fa-cog' />
            <span>Watch List</span>
            </Link> :" " }
            {LoginState?<div>
            <Link className='linkClass' to='/UserSettings'>
            <FaIcons.FaUserCog className='fa-cog' />
            <span>Settings</span>
            </Link>
            <Link className='linkClass' to='#' onClick={()=>setLoginState(false)}>
            <FaIcons.FaSignOutAlt className='fa-cog'  />
            <span>Logout</span>
            </Link>
            </div>: 
            <Link className='linkClass' to='/Login' onClick={()=>setLoginState(true)}>
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