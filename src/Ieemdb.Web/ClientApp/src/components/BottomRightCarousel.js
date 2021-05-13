import React, { useState } from 'react';
import movies from './Movie_Dataset';
import moviesTest from './test_dataset';
import AliceCarousel from 'react-alice-carousel';
import 'react-alice-carousel/lib/alice-carousel.css';
import MovieCard from './MovieCard';
import { Nav, Col } from 'react-bootstrap';
import '../Styles/BottomRightCarousel.css'
import axios from 'axios';
const responsive = {
    0: { items: 2 },
    568: { items: 3 },
    1024: { items: 6 },
};
function BottomRightCarousel() {
    const [data, setData] = useState([]);
    showNewReleases(); // me to poy fortwnei tha deixnei ta new releases
    function showNewReleases() {
      loadNewReleases();
    }

    function showRecentlyAdded() {
      loadRecentlyAdded();
    }

    function showTopRated() {
      loadTopRated();
    }
    async function loadTopRated() {
      await axios({
        method: 'get', url: `https://${window.location.host}/api/movie/top`, data: {
          "page": 1
        }
      }).then(res => setData(res.data.results))
    }
    async function loadNewReleases() {
      await axios({
        method: 'post', url: `https://${window.location.host}/api/movie/new`, data: {
          "page": 1 , "itemsPerPage": 20
        }
      }).then(res => setData(res.data.results))
    }
    async function loadRecentlyAdded() {
      await axios({
        method: 'post', url: `https://${window.location.host}/api/movie/latest`, data: {
          "page": 1, "itemsPerPage": 20
        }
      }).then(res => setData(res.data.results))
    }
    const items = data.map(i => <MovieCard
      id={i.id}
      Title={i.title}
      Poster={i.posterUrl}
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
    const [activeIndex, setActiveIndex] = useState(0);
    const slidePrev = () => setActiveIndex(activeIndex - 1);
    const slideNext = () => setActiveIndex(activeIndex + 1);
    const syncActiveIndex = ({ item }) => setActiveIndex(item);
    return (
        <div>
        <Nav className="nav-bottom-right">
            <Col>
            </Col>
            <Col>
                <Nav.Item>
                  <Nav.Link className="nav-bottom-right-link" onClick={showNewReleases}>New Releases</Nav.Link>
                </Nav.Item>
            </Col>
            <Col>
                <Nav.Item>
                  <Nav.Link className="nav-bottom-right-link" onClick={showRecentlyAdded}>Recently Added</Nav.Link>
                </Nav.Item>
            </Col>
            <Col>
                <Nav.Item>
                  <Nav.Link className="nav-bottom-right-link" onClick={showTopRated}>Top Rated</Nav.Link>
                </Nav.Item>
            </Col>
            <Col>
            </Col>
        </Nav>
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
    );
}
export default BottomRightCarousel;
