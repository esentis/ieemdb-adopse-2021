import React from "react";
import {Nav,Col} from 'react-bootstrap';

function BottomRightNav(){
    return(
        <Nav variant="pills" className="nav-bottom-right">
            <Col>
            </Col>
            <Col>
                <Nav.Item>
                    <Nav.Link>New Realeases</Nav.Link>
                </Nav.Item>
            </Col>
            <Col>
                <Nav.Item>
                    <Nav.Link>Popular</Nav.Link>
                </Nav.Item>
            </Col>
            <Col>
                <Nav.Item>
                    <Nav.Link>Recently Added</Nav.Link>
                </Nav.Item>
            </Col>
            <Col>
                <Nav.Item>
                    <Nav.Link>Top Rated</Nav.Link>
                </Nav.Item>
            </Col>
            <Col>
            </Col>
        </Nav>
    );
}

export default BottomRightNav;