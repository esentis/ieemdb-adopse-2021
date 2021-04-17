import React from 'react'
import {Col} from 'react-bootstrap';
<<<<<<< HEAD
import '../Styles/MovieView.css' 
import MovieViewPoster from './MovieViewPoster';
import MovieViewTrailer from './MovieViewTrailer';
import MovieViewSynopsis from './MovieViewSynopsis';
function MovieView(props) {
    console.log(props.id); //key de douleuei gia kapoio logo 
    return (
        <Col className='column-right-MovieView'>
            <div className='MovieViewPoster'><MovieViewPoster key={props.id} title={props.Title} poster={props.Poster} releaseDate={props.ReleaseDate} genres={props.Genres} rating={props.Rating} duration={props.Duration}/></div>
            <div className='splitScreen'>
                <div className='MovieViewSynopsis'><MovieViewSynopsis key={props.id} overview={props.Overview} actors={props.Actors} writers={props.Writers} directors={props.Directors} countryOrigin={props.CountryOrigin}/></div>
                <div className='MovieViewTrailer'><MovieViewTrailer key={props.id}/></div>
            </div>
        </Col>
        
=======
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
>>>>>>> 43c3444f2315957844f44fd247f6465180b3cf63
    )
}
export default MovieView;