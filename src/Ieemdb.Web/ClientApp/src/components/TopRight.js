import React, { useState } from 'react';
import AliceCarousel from 'react-alice-carousel';
import 'react-alice-carousel/lib/alice-carousel.css';
import {Col} from 'react-bootstrap';
import '../Styles/TopRight.css'
const responsive = {
    0: { items: 1 },
    568: { items: 2 },
    1024: { items: 4 },
};
function TopRight(props){
    const title = props.title;
    const items = props.items;
    const [activeIndex, setActiveIndex] = useState(0);
    const slidePrev = () => setActiveIndex(activeIndex - 1);
    const slideNext = () => setActiveIndex(activeIndex + 1);
    const syncActiveIndex = ({ item }) => setActiveIndex(item);
    return(
        <Col className="column-right">
            <div className="carousel">
                <div className="title1">
                    <h1 className="title2">{title}</h1>
                </div>
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
        </Col>
    );
}
export default TopRight;