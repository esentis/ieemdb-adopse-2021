import React from "react";
import {Col,Row} from 'react-bootstrap';
import TopRight from "./TopRight";
import BottomRight from "./BottomRight";
import Favorites from './Favorites';
import WatchList from './WatchList';
import Featured from './Featured';
import {usePage} from './Navigate' 
import Login from './Login';









function RightSide(){

    const page=usePage();

    var topPage=<Featured />
    var bottomPage=<BottomRight/>
    if(page=="Favorites"){
        topPage=<Favorites />
    }else if(page=="WatchList"){
        topPage=<WatchList />
    }else if(page=="Home"){
        topPage=<Featured />
    }else if(page=="LoginPage"){
        topPage=<Login />
        bottomPage=""
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