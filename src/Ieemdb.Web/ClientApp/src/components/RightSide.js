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
<<<<<<< HEAD
=======
import {Switch,Route} from 'react-router-dom';
import UserSettings from './UserSettings';



>>>>>>> 43c3444f2315957844f44fd247f6465180b3cf63
function RightSide(){
    const page=usePage();
<<<<<<< HEAD
    var topPage=<Featured />
    var bottomPage=<BottomRight/>
    switch(page.name){
        case "Home":
            topPage=<Featured />
            break;
        case "Favorites":
            topPage=<Favorites />
            break;
        case "WatchList":
            topPage=<WatchList />
            break;
        case "LoginPage":
            topPage=<Login name={page.name}/>
            bottomPage=""
            break;
        case "MovieView":
            topPage=<MovieView
                key={page.key}
                Title={page.Title}
                Poster={page.Poster}
                Overview={page.Overview}
                ReleaseDate={page.ReleaseDate}
                Genres={page.Genres}
                Actors={page.Actors}
                Writers={page.Writers}
                Directors={page.Directors}
                Rating={page.Rating}
                Duration={page.Duration}
                CountryOrigin={page.CountryOrigin}/>
                bottomPage="";
                console.log(page.key);
            break;
        case "SearchView":
            topPage=<SearchView name={page.name} SearchValue={page.value} />
            bottomPage="";
            break;  
        case "AdvancedSearchView":
            topPage=<AdvancedSearchView name={page.name} />   
            bottomPage=""
            break;
        default:
            topPage=<Featured />
            break;    
=======
    var bottomPage="";
    if(page.name==="Featured"||page.name==="Favorites"||page.name==="WatchList")
   {
        bottomPage=<BottomRight />
>>>>>>> 43c3444f2315957844f44fd247f6465180b3cf63
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