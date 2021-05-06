import React, { useState } from 'react';
import movies from './Movie_Dataset';
import AliceCarousel from 'react-alice-carousel';
import 'react-alice-carousel/lib/alice-carousel.css';
import MovieCard from './MovieCard';
import { Nav, Col } from 'react-bootstrap';
import '../Styles/BottomRightCarousel.css'
const responsive = {
    0: { items: 2 },
    568: { items: 3 },
    1024: { items: 6 },
};
function BottomRightCarousel() {
    const [choose, setChoose] = useState("");
    
    function showNewReleases(number) {
      setChoose(number);
      console.log(choose);
    }

    function showPopular(number) {
      setChoose(number);
    }

    function showRecentlyAdded(number) {
      setChoose(number);
    }

    function showTopRated(number) {
      setChoose(number);
    }

    function DecideCarousel() {
        const [activeIndex, setActiveIndex] = useState(0);
        const syncActiveIndex = ({ item }) => setActiveIndex(item);
        if(choose === "1") {

        }
        else if (choose === "2") {

        }
        else if (choose === "3") {

        }
        else if (choose === "4") {

        }
        else { //to default me to poy fortwnei h selida
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
          return <AliceCarousel
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
          />;
        }
      }
    
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
                  <Nav.Link className="nav-bottom-right-link" onClick={() => showNewReleases("1")}>New Releases</Nav.Link>
                </Nav.Item>
            </Col>
            <Col>
                <Nav.Item>
                  <Nav.Link className="nav-bottom-right-link" onClick={() => showPopular("2")}>Popular</Nav.Link>
                </Nav.Item>
            </Col>
            <Col>
                <Nav.Item>
                  <Nav.Link className="nav-bottom-right-link" onClick={() => showRecentlyAdded("3")}>Recently Added</Nav.Link>
                </Nav.Item>
            </Col>
            <Col>
                <Nav.Item>
                  <Nav.Link className="nav-bottom-right-link" onClick={() => showTopRated("4")}>Top Rated</Nav.Link>
                </Nav.Item>
            </Col>
            <Col>
            </Col>
        </Nav>
        <div className="div_style">
            <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/4.7.0/css/font-awesome.min.css"></link>
            <button className="button_arrow" onClick={slidePrev}><i id="arrow" className="fa fa-arrow-left"></i></button>
            <div className="carousel_style">
              <DecideCarousel />
            </div>
            <button className="button_arrow" onClick={slideNext}><i id="arrow" className="fa fa-arrow-right"></i></button>
        </div>
        </div>
    );
}
export default BottomRightCarousel;
