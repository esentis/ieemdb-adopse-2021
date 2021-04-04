import React from "react";
var img_style = {display: "block", marginLeft: "auto", marginRight: "auto", boxShadow: "0 0 10px black"};
var p_style = {color: "white", fontFamily: "Arial", fontSize: "20px", textShadow: "-1px 1px 10px white", textAlign: "center", padding: "5px"};
function MovieCard(id, title, poster, height, width){
    return(
        <div>
            <img style={img_style} src={poster} alt={id} height={height} width={width}/>
            <p style={p_style}>{title}</p>
        </div>
    );
}
export default MovieCard;