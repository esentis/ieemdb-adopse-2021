import React from 'react';
import {Row,Col,Container} from 'react-bootstrap';
import '../Styles/MovieView.css';

function MovieViewTrailer(props){
    return(
        <Container>
            <Row>
                <Col>
                    <p className="smallTitles" style={{textAlign: "center"}}>WATCH THE TRAILER</p>
                </Col>
            </Row>
            <Row>
                <Col>
                <div className="divTrailer">
                    <iframe width="560" height="315" src="https://www.youtube.com/embed/qHAH_zO0TWY" title="YouTube video player" frameborder="0" allow="accelerometer; autoplay; clipboard-write; encrypted-media; gyroscope; picture-in-picture" allowfullscreen></iframe>
                </div>
                </Col>
            </Row>
        </Container>
    );
}

export default MovieViewTrailer;