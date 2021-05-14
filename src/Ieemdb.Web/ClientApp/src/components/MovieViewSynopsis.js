import React, { useState } from 'react';
import {Row,Col} from 'react-bootstrap';
import '../Styles/MovieViewSynopsis.css';
import Modal from 'react-awesome-modal';
import AliceCarousel from 'react-alice-carousel';
import 'react-alice-carousel/lib/alice-carousel.css';
import MovieCard from './MovieCard';
import movies from './Movie_Dataset';
import Moment from "react-moment";
import axios from 'axios';
import LoadingSpinner from './LoadingSpinner';
const responsive = {
  0: { items: 2 },
  1024: { items: 5 },
  1199: { items: 6 },
};
function MovieViewSynopsis(props) {
    const [data, setData] = useState([]);
    const [opre, setopre] = useState(false);
    const [name, setName] = useState("");
    const [birthday, setBirthday] = useState("");
    const [bio, setBio] = useState("");
    const overview=props.overview;
    const durationHours = props.duration.hours;
    const durationMinutes = props.duration.minutes;
    const [loading, setLoading] = useState(true);
    const [watchListButtonText, setWatchListButtonText] = useState("");
    const [onLoad, setOnLoad] = useState(true);
    const [storeWatchlist, setStoreWatchList] = useState("");
    
    const directors = props.directors.map((directors) =>
      <span className="spanName" onClick={() => onDirectorClick(directors)}>{directors.fullName} </span>
    );
    const actors = props.actors.map((actors) =>
      <span className="spanName" onClick={() => onActorClick(actors)}>{actors.fullName} </span>
    );
    const writers = props.writers.map((writers) =>
      <span className="spanName" onClick={() => onWriterClick(writers)}>{writers.fullName} </span>
    );
    const countryOrigin = props.countryOrigin.map((countryOrigin) =>
      <span>{countryOrigin.name} </span>
    );
    
    if (onLoad == true) {
        setStoreWatchList(props.checkWatchList);

        if (localStorage.getItem('token') == null) {
          setWatchListButtonText("Log in to use Watchlists");
        }
        else {
          if (storeWatchlist == true) {
            setWatchListButtonText("Remove From Watchlist");
          }
          else if (storeWatchlist == false){
            setWatchListButtonText("Add To WatchList");
          }
        }
        setOnLoad(false);
    }
    
    async function onWatchlistButtonClick(){
        if (localStorage.getItem('token') == null) {
          //redirect sto login
        }
        else {
          if (storeWatchlist == true) {
            await axios({
              method: 'delete', url: `https://${window.location.host}/api/watchlist`, headers: { 'Authorization': 'Bearer ' + localStorage.getItem('token') }, params: {
                "movieId": props.id
              }
            }).then(res => console.log(res))
            setWatchListButtonText("Add To WatchList");
            setStoreWatchList(false);
          }
          else if (storeWatchlist == false) {
            await axios({
              method: 'post', url: `https://${window.location.host}/api/watchlist`, headers: { 'Authorization': 'Bearer ' + localStorage.getItem('token') }, params: {
                "movieId": props.id
              }
            }).then(res => console.log(res))
            setWatchListButtonText("Remove From Watchlist");
            setStoreWatchList(true);
          }
        }
    }
    function popupToggle() {
      setopre(current => !current);
    }
    function onActorClick(actors) {
      setName(actors.fullName);
      const releaseDate = <Moment format="DD/MM/YYYY">{actors.birthDay}</Moment>
      setBirthday(releaseDate);
      setBio(actors.bio);
      getActorCarousel(actors);
      popupToggle();
    }
    function onDirectorClick(directors) {
      setName(directors.fullName);
      const releaseDate = <Moment format="DD/MM/YYYY">{directors.birthDay}</Moment>
      setBirthday(releaseDate);
      setBio(directors.bio);
      getDirectorCarousel(directors);
      popupToggle();
    }
    function onWriterClick(writers) {
      setName(writers.fullName);
      const releaseDate = <Moment format="DD/MM/YYYY">{writers.birthDay}</Moment>
      setBirthday(releaseDate);
      setBio(writers.bio);
      getWriterCarousel(writers);
      popupToggle();
    }
    async function getDirectorCarousel(directors) {
      console.log(directors.fullName);
      await axios({
        method: 'post', url: `https://${window.location.host}/api/movie/search`, data: {
          "page": 1, "itemsPerPage": 20, "director": directors.fullName
        }
      }).then(res => setData(res.data.results))
    }
    async function getActorCarousel(actors) {
      console.log(actors.fullName);
      await axios({
        method: 'post', url: `https://${window.location.host}/api/movie/search`, data: {
          "page": 1, "itemsPerPage": 20, "actor": actors.fullName
        }
      }).then(res => setData(res.data.results))
    }
    async function getWriterCarousel(writers) {
      console.log(writers.fullName);
      await axios({
        method: 'post', url: `https://${window.location.host}/api/movie/search`, data: {
          "page": 1, "itemsPerPage": 20, "writer": writers.fullName
        }
      }).then(res => setData(res.data.results))
    }
    const items = data.map(i => <MovieCard
      id={i.id}
      Title={i.title}
      Poster={i.posterUrl ? i.posterUrl : "https://media.comicbook.com/files/img/default-movie.png"}
      height={"250vh"}
      width={'auto'}
      posterClass='poster'
      flag={false} />)
    const [activeIndex, setActiveIndex] = useState(0);
    const slidePrev = () => setActiveIndex(activeIndex - 1);
    const slideNext = () => setActiveIndex(activeIndex + 1);
    const syncActiveIndex = ({ item }) => setActiveIndex(item);
    return(
      <Col>
        <Row >
          <button className="buttonAddToWatchList" onClick={onWatchlistButtonClick}>{watchListButtonText}</button>
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
        <Modal visible={opre} width="90%" height="70%" effect="fadeInRight" onClickAway={popupToggle}>
          <div id="popADSView">
            <div id="popUpHeader">
              <button className="buttonClose" onClick={popupToggle}><i id="return" className="fa fa-close"></i></button>
              <p className="popUpName">{name}</p>
              <div className="birthDate">
                <p className="popUpDate">{birthday}</p>
              </div>
            </div>
            <div id="popUpBody">
              <p className="popUpBio">{bio===""?"We don't have a biography for "+name:bio}</p>
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
