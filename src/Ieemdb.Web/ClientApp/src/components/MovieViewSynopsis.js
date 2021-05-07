import React, { useState } from 'react';
import {Row,Col} from 'react-bootstrap';
import '../Styles/MovieViewSynopsis.css';
import Modal from 'react-awesome-modal';
function MovieViewSynopsis(props){
    /*const id=props.id;*/
    //test giana alla3w se vs
    const [opre, setopre] = useState(false);
    const [name, setName] = useState("");
    const overview=props.overview;
    const directors = props.directors.map((directors) =>
      <span className="spanName" onClick={() => onDirectorClick(directors)}>{directors} </span>
    );
    const actors = props.actors.map((actors) =>
      <span className="spanName" onClick={() => onActorClick(actors)}>{actors} </span>
    );
    const writers = props.writers.map((writers) =>
      <span className="spanName" onClick={() => onWriterClick(writers)}>{writers} </span>
    );
    const countryOrigin = props.countryOrigin.map((countryOrigin) =>
      <span>{countryOrigin} </span>
    );
    function onWatchlistButtonClick(){
        //Otan kanei click sto ADD TO WATCHLIST button
        console.log("Click on ADD TO WATCHLIST button");
    }
    function popupToggle() {
      setopre(current => !current);
    }
    function onActorClick(actors) {
      setName(actors);
      popupToggle();
    }
    function onDirectorClick(directors) {
      setName(directors);
      popupToggle();
    }
    function onWriterClick(writers) {
      setName(writers);
      popupToggle();
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
            <Modal visible={opre} width="60%" height="40%" effect="fadeInRight" onClickAway={popupToggle}>
              <div className="popUpHeader">
                <p className="popUpName">{name}</p>
              </div>
            </Modal>
        </Col>

    );
}
export default MovieViewSynopsis;
