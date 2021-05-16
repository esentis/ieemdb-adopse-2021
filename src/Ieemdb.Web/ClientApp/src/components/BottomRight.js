import React from "react";
import {Col} from 'react-bootstrap';
import BottomRightCarousel from './BottomRightCarousel';

const BottomRight=React.memo(()=>{
    return(
        <Col className="column-right">
            <BottomRightCarousel/>
        </Col>
    )});

export default BottomRight;
