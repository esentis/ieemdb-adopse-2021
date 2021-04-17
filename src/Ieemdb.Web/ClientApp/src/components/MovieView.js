import React from 'react'
import {Col} from 'react-bootstrap';
import '../Styles/MovieView.css';
import { useParams } from "react-router-dom";
import movies from './Movie_Dataset';




function MovieView(props) {
    const { id }=useParams();
    const item=movies.find(movie=>{
        return movie.id===id;
    })
    console.log(item);
    return (
       <Col className='column-right-MovieView'>
       <div style={{color:'white'}}>
    <div>
        <p>Title:{item.title}</p>
       <p>Poster:{item.poster}</p>
       <p>Overview:{item.overview}</p>
       <p>ReleaseDate:{item.releaseDate}</p>
       <p>Genres:{item.genres}</p>
       <p>Actors:{item.actors}</p>
       <p>Writers:{item.writers}</p>
       <p>Directors:{item.directors}</p>
       <p>Rating:{item.rating}</p>
       <p>Duration:{item.duration}</p>
       <p>CountryOrigin:{item.countryOrigin}</p>
       <p>{item.id}</p></div>
       </div>
       </Col>
    )
}

export default MovieView;