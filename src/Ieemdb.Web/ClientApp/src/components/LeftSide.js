import React/*,{useState,useEffect}*/ from "react";
import {Container,Col} from 'react-bootstrap';
import SearchBar from './SearchBar';
import {Link} from 'react-router-dom';
import '../Styles/NavBar.css'
import  * as FaIcons  from "react-icons/fa";
import logo from '../images/imdb_logo.png';
import {useLoginState,useChangeLoginState,useRole} from './GlobalContext';


function LeftSide(){
    const isLoggedIn=useLoginState();
    const setLoginState=useChangeLoginState();
    const Role=useRole();

    function logoutClick() {
      setLoginState(false);
      localStorage.clear();
    }

    return(
    <Col className="column-left" xl={2} style={{position:'fixed'}} >
    <label htmlFor='check' className='checkbtn'><FaIcons.FaBars /></label>
         <input type='checkbox' id='check'>  
        </input>

        <Link to="/"><img src={logo} className='logo' alt=""/></Link>
        <Container fluid className="nav-center2">
            <nav>
            <SearchBar/>
            <span>{isLoggedIn ? <Link to='/AdvancedSearch'><button className='advButton'>Advanced</button></Link> : ""}</span>
            <ul>
            <Link className='linkClass Home' to='/' name='Favorites'><span>Home</span></Link>
            {isLoggedIn ? <Link className='linkClass AdvLink' to='/AdvancedSearch' name='Favorites'><span>Advanced Search</span></Link>:" "}
            {isLoggedIn ? <Link className='linkClass' to='/Favorites' name='Favorites'>
            <FaIcons.FaStar className='fa-cog' />
            <span>Favorites</span>
            </Link> :" " }
            {isLoggedIn ? <Link className='linkClass' to='/WatchList' name='WatchList'>
            <FaIcons.FaList className='fa-cog' />
            <span>Watch List</span>
            </Link> :" " }
            {isLoggedIn&&Role==="ADMINISTRATOR" ? <Link className='linkClass' to='/AdminPanel' name='Favorites'>
            <FaIcons.FaUserLock className='fa-cog' />
            <span>Admin Panel</span>
            </Link> :" " }
              {isLoggedIn ? <div>
                <Link className='linkClass' to='/UserSettings'>
                  <FaIcons.FaUserCog className='fa-cog' />
                  <span>Settings</span>
                </Link>
                <Link className='linkClass' to='#' onClick={logoutClick}>
                  <FaIcons.FaSignOutAlt className='fa-cog' />
                  <span>Logout</span>
                </Link>
              </div> :
                <Link className='linkClass' to='/Login'>
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
