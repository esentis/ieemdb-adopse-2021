import React from "react";
import {Col,Row} from 'react-bootstrap';
import TopRight from "./TopRight";
import BottomRight from "./BottomRight";

function RightSide(){
    return(
        <Col lg={10}>
            <Row>
                <TopRight/>
            </Row>
            <Row>
                <BottomRight/>
            </Row>
        </Col>
    );
}

export default RightSide;