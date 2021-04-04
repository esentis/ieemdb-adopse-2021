import React from "react";
import movies from './Movie_Dataset';
import AliceCarousel from 'react-alice-carousel';
import 'react-alice-carousel/lib/alice-carousel.css';



const items = movies.map(i => <div> {MovieCard(i.id, i.title, i.poster, "200px", "55%")}</div>);
const div_style = {display: 'flex', justifyContent: 'center', width: '100%'};
const responsive = {
    0: { items: 1 },
    568: { items: 3 },
    1024: { items: 6 },
};


function BottomRightCarousel(){
    return(
            <div style={div_style}>
                <div style={{width: "100%"}}>
                    <AliceCarousel
                        infinite
                        autoPlayStrategy="none"
                        animationDuration={1000}
                        animationType="fadeout"
                        mouseTracking
                        items={items}
                        responsive={responsive}
                        disableButtonsControls
                    />
                </div>
            </div>
    );
}

export default BottomRightCarousel;