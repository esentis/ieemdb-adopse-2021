import React, { useState } from 'react';
import AliceCarousel from 'react-alice-carousel';
import 'react-alice-carousel/lib/alice-carousel.css';
import '../Styles/TopRight.css';
import  LoadingSpinner from './LoadingSpinner';

const responsive = {
    0: { items: 1 },
    568: { items: 2 },
    1024: { items: 4 },
};
function TopRight(props){
    var itemsLength=false;
    const title = props.title;
    const items = props.items;
    const loading=props.loading;
    const [activeIndex, setActiveIndex] = useState(0);
    const slidePrev = () => setActiveIndex(activeIndex - 1);
    const slideNext = () => setActiveIndex(activeIndex + 1);
    const syncActiveIndex = ({ item }) => setActiveIndex(item);
    if(items.length>4){
        itemsLength=true;
    }
    return(
        
            <div className="carousel">
                <div className="title1">
                    <h1 className="title2">{title.toUpperCase()}</h1>
                </div>
                {loading?<LoadingSpinner color="#D3D3D3" loading={loading} size={20} />:items.length>0? 
                <div className="div_style">
                    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/4.7.0/css/font-awesome.min.css"></link>
                    {itemsLength && <button className="button_arrow" onClick={slidePrev}><i id="arrow" className="fa fa-arrow-left"></i></button>}
                    <div className="carousel_style">
                        <AliceCarousel
                            activeIndex={activeIndex}
                            infinite={itemsLength}
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
                    {itemsLength&& <button className="button_arrow" onClick={slideNext}><i id="arrow" className="fa fa-arrow-right"></i></button>}
                </div>
                :<p className="emptyMessage">{!title.toLowerCase().includes("featured")?<span>Your</span>:""} {title} {!title.toLowerCase().includes("list")?<span>list</span>:""} is Empty</p>  } 
                 
               
            </div>
       
    );
}
export default TopRight;