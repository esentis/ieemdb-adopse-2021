import React from 'react';
import {Row,Col,Container} from 'react-bootstrap';
import '../Styles/MovieViewTrailer.css';

function MovieViewTrailer(props){
    const url = props.trailer.split('/').pop();

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
                    <iframe className="frameTrailer" src={`https://www.youtube.com/embed/${url}`} title="YouTube video player" frameBorder="0" allow="accelerometer; autoplay; clipboard-write; encrypted-media; gyroscope; picture-in-picture" allowFullScreen></iframe>
                </div>
                </Col>
            </Row>
            <div className="kenoTab"></div>
        </Container>
    );
}

export default MovieViewTrailer;