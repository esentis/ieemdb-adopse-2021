import React from "react";
import {Col,Row} from 'react-bootstrap';
import BottomRight from "./BottomRight";
import Favorites from './Favorites';
import WatchList from './WatchList';
import Featured from './Featured';
import {usePage} from './GlobalContext' 
import Login from './Login';
import MovieView from "./MovieView";
import SearchView from "./SearchView";
import AdvancedSearchView from "./AdvancedSearchView";
import {Switch,Route} from 'react-router-dom';
import UserSettings from './UserSettings';



function RightSide(){
    const page=usePage();
    var bottomPage="";
    if(page==="2")
   {
        bottomPage=<BottomRight />
    }
    return(
    <Col>
        <Row>   
   <Switch>
     <Route path='/' exact children={<Featured/>}/>
     <Route path='/Favorites' children={<Favorites/>} />
     <Route path='/WatchList' children={<WatchList/>} />
     <Route path='/Login' children={<Login/>} />
     <Route path={'/Movie/:id'} children={<MovieView/>} />
     <Route path='/AdvancedSearch' children={<AdvancedSearchView/>} />
     <Route path='/UserSettings' children={<UserSettings />} />
     <Route path={'/:SearchType/:value'} children={<SearchView/>} />
        </Switch>
        </Row>
            <Row>
              {bottomPage}
            </Row>
    </Col>


  

    );
}
export default RightSide;