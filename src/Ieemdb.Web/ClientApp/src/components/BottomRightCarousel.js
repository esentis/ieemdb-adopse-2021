import React from "react";
import movies from './Movie_Dataset';
import AliceCarousel from 'react-alice-carousel';
import 'react-alice-carousel/lib/alice-carousel.css';
import MovieCard from './MovieCard';
const items = movies.map(i => <div> {MovieCard(i.id, i.title, i.poster, "200vh", "45%",false)}</div>);
const div_style = {display: 'block', justifyContent: 'center', width: '100%'};
const responsive = {
    0: { items: 1 },
    568: { items: 3 },
    1024: { items: 5 },
};


function BottomRightCarousel(){
    return(
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
    );
}

export default BottomRightCarousel;