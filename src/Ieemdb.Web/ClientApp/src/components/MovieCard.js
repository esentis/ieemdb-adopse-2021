import React from 'react';
import '../Styles/MovieCard.css';
import {useUpdatePage} from './Navigate'
function MovieCard(props){
    const setPage=useUpdatePage();
    const MovieDetails={name:"MovieView",
        key:props.id,
        Title:props.Title,
        Poster:props.Poster,
        Overview:props.Overview,
        ReleaseDate:props.ReleaseDate,
        Genres:props.Genres,
        Actors:props.Actors,
        Writers:props.Writers,
        Directors:props.Directors,
        Rating:props.Rating,
        Duration:props.Duration,
        CountryOrigin:props.CountryOrigin
    }
    function onPosterClick(){
        setPage(MovieDetails);
    }
    return(
        <div>
            <div className="poster">
                <img src={props.Poster} alt={props.id} height={props.height} width={props.width} onClick={onPosterClick} />
            </div>
            <div>
                <p className="title" onClick={onPosterClick}>{props.Title}</p>
            </div>
        </div>
    );
}export default MovieCard;