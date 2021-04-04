import React from 'react';
import AliceCarousel from 'react-alice-carousel';
import 'react-alice-carousel/lib/alice-carousel.css';
import {Col} from 'react-bootstrap';
import MovieCard from './MovieCard';
import movies from './Movie_Dataset';
const title = 'FEATURED';
const items = movies.map(i => <div> {MovieCard(i.id, i.title, i.poster, "250px", "55%")}</div>);
const div_style = {display: 'flex', justifyContent: 'center', width: '100%'};
const h1_style = {color: "#ede6c4", fontFamily: "Arial", paddingTop: "10px", fontSize: "30px", fontWeight: "bold", textShadow: "1px 10px 10px rgba(127, 113, 47), 0 0 20px rgba(127, 113, 47), 0 0 30px rgba(48, 43, 18, 1)"};
const responsive = {
    0: { items: 1 },
    568: { items: 2 },
    1024: { items: 4 },
};
function TopRight(){
    return(
        <Col className="column-right">
            <div style={div_style}>
                <h1 style={h1_style}>{title}</h1>
            </div>
            <div style={div_style}>
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
        </Col>
    );
}
export default TopRight;