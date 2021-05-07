import React from 'react';
import {Row,Col} from 'react-bootstrap';
import '../Styles/MovieViewSynopsis.css';
function MovieViewSynopsis(props){
    /*const id=props.id;*/
    const overview=props.overview;
    const directors = props.directors.map((directors) =>
        <span className="span">{directors} </span>
    );
    const actors = props.actors.map((actors) =>
        <span className="span">{actors} </span>
    );
    const writers = props.writers.map((writers) =>
        <span className="span">{writers} </span>
    );
    const countryOrigin = props.countryOrigin.map((countryOrigin) =>
        <span className="span">{countryOrigin} </span>
    );
    function onWatchlistButtonClick(){
        //Otan kanei click sto ADD TO WATCHLIST button
        console.log("Click on ADD TO WATCHLIST button");
    }
    return(
        <Col>
            <Row >
                <button className="buttonAddToWatchList" onClick={onWatchlistButtonClick}>ADD TO WATCHLIST</button>
            </Row>
            <Row className="rowTab">
                <p className="smallTitles">SYNOPSIS</p>
                <p className="text">{overview}</p>
            </Row>
            <Row className="rowTab2">
                <Col>
                    <p className="smallTitles">DIRECTORS</p>
                    <p className="text">{directors}</p>
                </Col>
                <Col>
                    <p className="smallTitles">WRITERS</p>
                    <p className="text">{writers}</p>
                </Col>
            </Row>
            <Row className="rowTab2">
                <Col>
                    <p className="smallTitles">STARRING</p>
                    <p className="text">{actors}</p>
                </Col>
                <Col>
                    <p className="smallTitles">COUNTRY OF ORIGIN</p>
                    <p className="text">{countryOrigin}</p>
                </Col>
            </Row>
        </Col>
    );
}
export default MovieViewSynopsis;