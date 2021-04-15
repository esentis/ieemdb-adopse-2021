import React from "react";
import {Col,Row} from 'react-bootstrap';
import TopRight from "./TopRight";
import BottomRight from "./BottomRight";
import Favorites from './Favorites';
import WatchList from './WatchList';
import Featured from './Featured';
import {usePage} from './Navigate' 
import Login from './Login';
import MovieView from "./MovieView";
import { propTypes } from "react-bootstrap/esm/Image";
import SearchView from "./SearchView";
import { FaBreadSlice } from "react-icons/fa";
import AdvancedSearchView from "./AdvancedSearchView";



function RightSide(){

    const page=usePage();

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
            break;
        case "SearchView":
            topPage=<SearchView name={page.name} SearchValue={page.value} />
            bottomPage="";
            break;  
        case "AdvancedSearchView":
            topPage=<AdvancedSearchView name={page.name} />   
            bottomPage=""
            break;
    }
    return(
        <Col>
            <Row>
            {topPage}
            </Row>
            <Row>
              {bottomPage}
            </Row>
        </Col>
    );
}

export default RightSide;