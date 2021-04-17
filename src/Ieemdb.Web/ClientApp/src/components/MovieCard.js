import React from 'react';
import '../Styles/MovieCard.css';
import {useUpdatePage} from './Navigate'
<<<<<<< HEAD
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
=======
import {useHistory} from 'react-router-dom';



function MovieCard(props){
    const setPage=useUpdatePage();
    const history=useHistory();

    const MovieDetails={name:"MovieView",
    id:props.id,
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

>>>>>>> 43c3444f2315957844f44fd247f6465180b3cf63
    function onPosterClick(){
        setPage(MovieDetails);
        history.push('/Movie/'+props.id);
    }
    return(
        <div>
            <div className="poster">
<<<<<<< HEAD
                <img src={props.Poster} alt={props.id} height={props.height} width={props.width} onClick={onPosterClick} />
=======
                <img src={props.Poster} alt={props.key} height={props.height} width={props.width} onClick={onPosterClick} />
>>>>>>> 43c3444f2315957844f44fd247f6465180b3cf63
            </div>
            <div>
            <p className="title" onClick={onPosterClick}>{props.Title}</p>
            </div>
        </div>
    );
}export default MovieCard;