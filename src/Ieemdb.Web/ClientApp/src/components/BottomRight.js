import React from "react";
import {Col,Row} from 'react-bootstrap';
import BottomRightNav from './BottomRightNav';
import BottomRightCarousel from './BottomRightCarousel';

function BottomRight(){
    return(
        <Col className="column-right">
            <Row>
                <BottomRightNav/>
            </Row>
            <Row lg={14}>
                <BottomRightCarousel/>
            </Row>
        </Col>
    );
}

export default BottomRight;