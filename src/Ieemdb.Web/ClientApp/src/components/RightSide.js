import React from "react";
import {Col,Row} from 'react-bootstrap';
import BottomRight from "./BottomRight";
import Favorites from './Favorites';
import WatchList from './WatchList';
import Featured from './Featured';
import {usePage} from './Navigate' 
import Login from './Login';
import MovieView from "./MovieView";
import SearchView from "./SearchView";
import AdvancedSearchView from "./AdvancedSearchView";
import {Switch,Route} from 'react-router-dom';
import UserSettings from './UserSettings';



function RightSide(){
    const page=usePage();
    var bottomPage="";
    if(page.name==="Featured"||page.name==="Favorites"||page.name==="WatchList")
   {
        bottomPage=<BottomRight />
    }
    return(
    <Col>
        <Row>   
   <Switch>
     <Route path='/' exact render={Featured}/>
     <Route path='/Favorites' render={Favorites} />
     <Route path='/WatchList' render={WatchList} />
     <Route path='/Login' render={Login} />
     <Route path={'/Movie/:id'} children={<MovieView/>} />
     <Route path='/AdvancedSearch' render={(props)=>(<AdvancedSearchView {...props} name={page.name}/>)} />
     <Route path='/UserSettings' render={UserSettings} />
     <Route path='/Search' render={(props)=>(<SearchView {...props} name={page.name} SearchValue={page.value}/>)} />
        </Switch>
        </Row>
            <Row>
              {bottomPage}
            </Row>
    </Col>


  

    );
}
export default RightSide;