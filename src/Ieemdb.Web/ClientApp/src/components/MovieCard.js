import React from 'react';
import '../Styles/MovieCard.css';
import {useUpdatePage} from './GlobalContext'
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

    function onPosterClick(){
        setPage(MovieDetails);
        history.push('/Movie/'+props.id);
    }
    return(
        <div>
            <div className={props.posterClass}>
                <img className="img_poster" src={props.Poster} alt={props.key} height={props.height} width={props.width} onClick={onPosterClick} />
                {props.flag?<div onClick={()=>props.onClick(props.id)} className="overlay">Remove from Featured</div>:""}
            </div>
            <div>
            <p className="title" onClick={onPosterClick}>{props.Title}</p>
            </div>
        </div>
    );
}export default MovieCard;