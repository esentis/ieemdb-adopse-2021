import React from "react";
import {Col,Row} from 'react-bootstrap';
import TopRight from "./TopRight";
import BottomRight from "./BottomRight";
import Favorites from './Favorites';
import WatchList from './WatchList';
import {usePage} from './Navigate' 







function RightSide(){

    const page=usePage();

    var topPage=<TopRight />;
    if(page=="Favorites"){
        topPage=<Favorites />
    }else if(page=="WatchList"){
        topPage=<WatchList />
    }
    
    return(
        <Col lg={10}>
            <Row>
            {topPage}
            </Row>
            <Row>
                <BottomRight/>
            </Row>
        </Col>
    );
}

export default RightSide;