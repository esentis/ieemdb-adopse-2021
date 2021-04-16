import React from 'react';
import {Row,Col,Container} from 'react-bootstrap';
import '../Styles/MovieViewTrailer.css';

function MovieViewTrailer(props){
    return(
        <Container>
            <Row>
                <Col>
                    <p className="titleTrailer">WATCH THE TRAILER</p>
                </Col>
            </Row>
            <Row>
                <Col>
                <div className="divTrailer">
                    <iframe className="frameTrailer" src="https://www.youtube.com/embed/qHAH_zO0TWY" title="YouTube video player" frameborder="0" allow="accelerometer; autoplay; clipboard-write; encrypted-media; gyroscope; picture-in-picture" allowfullscreen></iframe>
                </div>
                </Col>
            </Row>
            <div className="kenoTab"></div>
        </Container>
    );
}

export default MovieViewTrailer;