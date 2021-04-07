import React from 'react';
import './MovieCard.css';
var svg;
export default function MovieCard(id, title, poster, height, width, flag){
    /*
    if (flag) {
        svg = <svg className="btn_play" viewBox="0 0 60 60" onClick={onPlayerClick}><polygon points="0,0 50,30 0,60"/></svg>;
    }
    */
    function onPosterHover(id) {
        //Mouse on poster visible play button
    }
    /*
    function onPlayerClick(){
        //Click on play button open youtube frame with trailer
        console.log("Click on player");
    }
    */
    function onPosterClick(){
        //Click on poster open Movie Page
        console.log("Click on poster");
    }
    return(
        <div>
            <div className="poster">
                <img src={poster} alt={id} height={height} width={width} onClick={onPosterClick} onMouseOver={onPosterHover({id})}/>
                {svg}
            </div>
            <div>
                <p className="title" onClick={onPosterClick}>{title}</p>
            </div>
        </div>
    );
}