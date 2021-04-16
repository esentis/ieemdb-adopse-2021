import React from "react";
import {Col} from 'react-bootstrap';
import BottomRightNav from './BottomRightNav';
import BottomRightCarousel from './BottomRightCarousel';
function BottomRight(){
    return(
        <Col className="column-right">
            <BottomRightNav/>
            <BottomRightCarousel/>
        </Col>
    );
}
export default BottomRight;