import React from "react";
import {Container,Col} from 'react-bootstrap';

import SearchText from './SearchText';
import LeftNav from './LeftNav';

function LeftSide(){
    return(
    <Col className="column-left" lg={4}>
        <Container fluid className="nav-center">
            <SearchText/>
            <br />
            <LeftNav/>
        </Container>
    </Col>
    );
}

export default LeftSide;