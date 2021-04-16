import React from 'react';
import {Button,Row,Col, Container} from 'react-bootstrap';
import '../Styles/MovieView.css';
function MovieViewSynopsis(props){
    const key=props.key;
    const overview=props.overview;
    const actors=props.actors;
    const writers=props.writers;
    const directors=props.directors;
    const countryOrigin=props.countryOrigin;
    return(
        <Container>
            <Row>
                <Button variant="primary" size="lg" block>WATCH AGAIN</Button>
            </Row>
            <Col>
                <Row>
                    <p className="smallTitles">SYNOPSIS</p>
                    <p className="text">{overview}</p>
                </Row>
            </Col>
            <Row>
                <Col>
                    <p className="smallTitles">DIRECTORS</p>
                    <p className="text">{directors}</p>
                </Col>
                <Col>
                    <p className="smallTitles">WRITERS</p>
                    <p className="text">{writers}</p>
                </Col>
            </Row>
            <Row>
                <Col>
                    <p className="smallTitles">STARRING</p>
                    <p className="text">{actors}</p>
                </Col>
            </Row><Row>
                <Col>
                    <p className="smallTitles">COUNTRY OF ORIGIN</p>
                    <p className="text">{countryOrigin}</p>
                </Col>
            </Row>
        </Container>
    );
}
export default MovieViewSynopsis;