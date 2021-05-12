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
        <span className="span">{directors.firstName} {directors.lastName} </span>
    );
    const actors = props.actors.map((actors) =>
        <span className="span">{actors.firstName} {actors.lastName}</span>
    );
    const writers = props.writers.map((writers) =>
        <span className="span">{writers.firstName} {writers.lastName} </span>
    );
    const countryOrigin = props.countryOrigin.map((countryOrigin) =>
        <span className="span">{countryOrigin.name}</span>
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
                {durationMinutes > 0
                    ? <p className="text" style={{cursor:'auto'}}>Duration: {durationHours} hours and {durationMinutes} minutes</p>
                    : <p className="text" style={{cursor:'auto'}}>Duration: {durationHours} hours</p>
                }
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