import React from 'react'

function Genre(props) {
    return (
        <p className="movieDescGenre" onClick={()=>props.onClick(props.id,props.name)}>{props.name}</p>
    )
}
export default Genre;
