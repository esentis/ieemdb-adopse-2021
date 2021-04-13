import React from "react";
import movies from './Movie_Dataset';
import AliceCarousel from 'react-alice-carousel';
import 'react-alice-carousel/lib/alice-carousel.css';
import MovieCard from './MovieCard';
const div_style = {display: 'block', justifyContent: 'center'};
const responsive = {
    0: { items: 1 },
    568: { items: 3 },
    1024: { items: 5 },
};


function BottomRightCarousel(){
    const items=movies.map(i => <MovieCard 
        key={i.id}
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
        width={'auto'} />)
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