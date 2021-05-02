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
import {Switch,Route, Redirect} from 'react-router-dom';
import UserSettings from './UserSettings';
import {useRole,useCheckLogin} from './GlobalContext';
import AdminPanel from './AdminPanel';



function RightSide() {
  const page = usePage();
  const CheckLoginState=useCheckLogin();
  const Role=useRole();
  var bottomPage="";
    if(page==="2")
   {
        bottomPage=<BottomRight />
    }

    return(
    <Col>
        <Row>   
          <Switch>
            <Route path='/' exact children={<Featured />} />
            <Route path='/Favorites' children={CheckLoginState() ? <Favorites />:<Redirect push to="/Login" />} />
            <Route path='/WatchList' children={CheckLoginState() ? <WatchList /> : <Redirect push to="/Login" />} />
            <Route path='/Login' children={CheckLoginState() ? <Redirect push to="/" />:<Login/>} />
            <Route path={'/Movie/:id'} children={<MovieView/>} />
            <Route path='/AdvancedSearch' children={CheckLoginState() ? <AdvancedSearchView /> : <Redirect push to="/Login" />} />
            <Route path='/UserSettings' children={CheckLoginState() ? <UserSettings /> : <Redirect push to="/Login" />} />
            <Route path={'/:SearchType/value=:value?'} children={<SearchView/>} />
            <Route path={'/AdminPanel'} children={CheckLoginState() && Role==='Admin'? <AdminPanel /> : <Redirect push to="/Login" /> } />
            <Route path={'/:SearchType/MovieTitle=:MovieTitle? ActorName=:ActorName? DirectorName=:DirectorName? WriterName=:WriterName? Duration=:Duration? Genres=:Genres? FromRating=:FromRating? ToRating=:ToRating? FromDate=:FromDate? ToDate=:ToDate?'} 
              children={ CheckLoginState() ? <SearchView /> : <Redirect push to="/Login" />} />
        </Switch>
        </Row>
            <Row>
              {bottomPage}
            </Row>
    </Col>
    );
}
export default RightSide;
