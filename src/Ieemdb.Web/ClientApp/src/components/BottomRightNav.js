import React from "react";
import {Nav,Col} from 'react-bootstrap';

export default function BottomRightNav(){
    
    function showNewReleases(){
        console.log('aaaaa')
    }

    function showPopular(){
        console.log('bbbbb')
    }

    function showRecentlyAdded(){
        console.log('ccccc')
    }

    function showTopRated(){
        console.log('ddddd')
    }
    
    return(
        <Nav className="nav-bottom-right">
            <Col>
            </Col>
            <Col>
                <Nav.Item>
                    <Nav.Link className="nav-bottom-right-link" onClick={showNewReleases}>New Realeases</Nav.Link>
                </Nav.Item>
            </Col>
            <Col>
                <Nav.Item>
                    <Nav.Link className="nav-bottom-right-link" onClick={showPopular}>Popular</Nav.Link>
                </Nav.Item>
            </Col>
            <Col>
                <Nav.Item>
                    <Nav.Link className="nav-bottom-right-link" onClick={showRecentlyAdded}>Recently Added</Nav.Link>
                </Nav.Item>
            </Col>
            <Col>
                <Nav.Item>
                    <Nav.Link className="nav-bottom-right-link" onClick={showTopRated}>Top Rated</Nav.Link>
                </Nav.Item>
            </Col>
            <Col>
            </Col>
        </Nav>
    );
}

