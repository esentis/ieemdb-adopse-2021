import React from 'react';
import '../Styles/MovieCard.css';
import {useHistory} from 'react-router-dom';



function MovieCard(props){
    const history=useHistory();
    function onPosterClick(){
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