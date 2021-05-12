import React, { useState } from 'react';
import {Row,Col} from 'react-bootstrap';
import '../Styles/MovieViewSynopsis.css';
import Modal from 'react-awesome-modal';
import AliceCarousel from 'react-alice-carousel';
import 'react-alice-carousel/lib/alice-carousel.css';
import MovieCard from './MovieCard';
import movies from './Movie_Dataset';
const responsive = {
  0: { items: 2 },
  1024: { items: 5 },
  1199: { items: 6 },
};
function MovieViewSynopsis(props){
    /*const id=props.id;*/
    //test giana alla3w se vs
    //test vol2
    //τεστ v
    const items = movies.map(i => <MovieCard
        id={i.id}
        Title={i.title} 
        Poster={i.poster} 
        Overview={i.overview}
        ReleaseDate={i.release_date}
        Genres={i.genres}
        Actors={i.actors}
        Writers={i.writers}
        Directors={i.directors}
        Rating={i.rating}
        Duration={i.duration}
        CountryOrigin={i.countryOrigin}
        height={"200vh"} 
        width={"140vw"} />
        );
    const [opre, setopre] = useState(false);
    const [name, setName] = useState("");
    const overview=props.overview;
    const durationHours = props.duration.hours;
    const durationMinutes = props.duration.minutes;
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
    const [activeIndex, setActiveIndex] = useState(0);
    const slidePrev = () => setActiveIndex(activeIndex - 1);
    const slideNext = () => setActiveIndex(activeIndex + 1);
    const syncActiveIndex = ({ item }) => setActiveIndex(item);
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
        <Modal visible={opre} width="90%" height="70%" effect="fadeInRight" onClickAway={popupToggle}>
          <div id="popADSView">
            <div id="popUpHeader">
              <button className="buttonClose" onClick={popupToggle}><i id="return" className="fa fa-close"></i></button>
              <p className="popUpName">{name}</p>
              <div className="birthDate">
                <p className="popUpDate">November 19, 1958</p>
              </div>
            </div>
            <div id="popUpBody">
              <p className="popUpBio">Charles Stuart Kaufman is an American screenwriter, producer, director, and novelist. He wrote the films Being John Malkovich (1999), Adaptation (2002), and Eternal Sunshine of the Spotless Mind (2004). He made his directorial debut with Synecdoche, New York (2008), which film critic Roger Ebert called "the best movie of the decade" in 2009. Further directorial work includes the stop motion animated film Anomalisa (2015) and I'm Thinking of Ending Things (2020). In 2020, Kaufman made his literary debut with the release of his first novel, Antkind.</p>
              <div className="div_style">
                <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/4.7.0/css/font-awesome.min.css"></link>
                <button className="button_arrow" onClick={slidePrev}><i id="arrow" className="fa fa-arrow-left"></i></button>
                <div className="carousel_style">
                  <AliceCarousel
                    activeIndex={activeIndex}
                    infinite
                    autoPlayStrategy="none"
                    animationType="fadeout"
                    mouseTracking
                    items={items}
                    responsive={responsive}
                    disableDotsControls
                    disableButtonsControls
                    onSlideChanged={syncActiveIndex}
                  />
                  
                </div>
                <button className="button_arrow" onClick={slideNext}><i id="arrow" className="fa fa-arrow-right"></i></button>
              </div>   
            </div>
          </div>
        </Modal>
      </Col>
    );
}
export default MovieViewSynopsis;