import React from "react";
import {Col,Container,Card, Row} from 'react-bootstrap';

function BottomRightCarousel(){
    return(
        <Container fluid className="carousel-bottom-right"> 
            <Row>
                <Col lg={1}>
                    <span>1</span>
                </Col>
                <Col lg={2}>
                    <span>1</span>
                </Col>
                <Col lg={2}>
                    <span>1</span>
                </Col>
                <Col lg={2}>
                    <span>1</span>
                </Col>
                <Col lg={2}>
                    <span>1</span>
                </Col>
                <Col lg={2}>
                    <span>1</span>
                </Col>
                <Col lg={2}>
                    <span>1</span>
                </Col>
                <Col lg={1}>
                    <span>1</span>
                </Col>
            </Row>
        </Container>
    );
}

export default BottomRightCarousel;