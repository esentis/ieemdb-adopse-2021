import React from "react";
import {Col} from 'react-bootstrap';
import BottomRightNav from './BottomRightNav';

function BottomRight(){
    return(
        <Col className="column-right">
            <BottomRightNav/>
        </Col>
    );
}

export default BottomRight;