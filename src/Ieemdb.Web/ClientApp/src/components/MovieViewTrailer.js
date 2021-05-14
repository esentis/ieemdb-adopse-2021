import React from 'react';
import {Row,Col,Container} from 'react-bootstrap';
import '../Styles/MovieViewTrailer.css';

function MovieViewTrailer(props){
   var Site="";
   var key="";

   if(props.trailerKey!==undefined){
       Site=props.trailerKey.site;
       key=props.trailerKey.key;
   }
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
                {Site==="Vimeo"? <iframe className="frameTrailer" src={`https://player.vimeo.com/video/${key}`} title="YouTube video player" frameBorder="0" allow="accelerometer; autoplay; clipboard-write; encrypted-media; gyroscope; picture-in-picture" allowFullScreen></iframe>:
                    <iframe className="frameTrailer" src={`https://www.youtube.com/embed/${key}`} title="YouTube video player" frameBorder="0" allow="accelerometer; autoplay; clipboard-write; encrypted-media; gyroscope; picture-in-picture" allowFullScreen></iframe>}
                </div>
                </Col>
            </Row>
            <div className="kenoTab"></div>
        </Container>
    );
}

export default MovieViewTrailer;