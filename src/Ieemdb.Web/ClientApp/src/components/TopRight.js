import React from 'react';
import AliceCarousel from 'react-alice-carousel';
import 'react-alice-carousel/lib/alice-carousel.css';
import {Col} from 'react-bootstrap';
import MovieCard from './MovieCard';
import movies from './Movie_Dataset';
import './TopRight.css'
const title = 'FEATURED';
const items = movies.map(i => <div> {MovieCard(i.id, i.title, i.poster, "250vh", "auto", true)}</div>);
const responsive = {
    0: { items: 1 },
    568: { items: 2 },
    1024: { items: 4 },
};
function TopRight(){
    return(
        <Col className="column-right">
            <div className="carousel">
                <div className="title1">
                    <h1 className="title2">{title}</h1>
                </div>
                <div>
                    <div style={{width: "100%"}}>
                        <AliceCarousel
                            infinite
                            autoPlay
                            autoPlayStrategy="none"
                            autoPlayInterval={3000}
                            animationDuration={1000}
                            animationType="fadeout"
                            mouseTracking
                            items={items}
                            responsive={responsive}
                            disableDotsControls
                            disableButtonsControls
                        />
                    </div>
                </div>
            </div>
        </Col>
    );
}
export default TopRight;