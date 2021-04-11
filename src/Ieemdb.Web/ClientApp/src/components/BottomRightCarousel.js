import React from "react";
import movies from './Movie_Dataset';
import AliceCarousel from 'react-alice-carousel';
import 'react-alice-carousel/lib/alice-carousel.css';
import MovieCard from './MovieCard';
const items = movies.map(i => <div> {MovieCard(i.id, i.title, i.poster, "250vh", "auto",false)}</div>);
const div_style = {display: 'block', justifyContent: 'center'};
const responsive = {
    0: { items: 1 },
    568: { items: 2 },
    1024: { items: 4 },
};


function BottomRightCarousel(){
    return(
            <div style={div_style}>
                    <div style={{width: "100%"}}>
                        <AliceCarousel
                            infinite
                            autoPlayStrategy="none"
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