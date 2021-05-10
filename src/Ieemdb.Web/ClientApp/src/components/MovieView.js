import React,{useEffect} from 'react'
import {Col} from 'react-bootstrap';
import { useParams } from "react-router-dom";
import '../Styles/MovieView.css' 
import MovieViewPoster from './MovieViewPoster';
import MovieViewTrailer from './MovieViewTrailer';
import MovieViewSynopsis from './MovieViewSynopsis';
import movies from './Movie_Dataset';
import {useUpdatePage} from './GlobalContext'
function MovieView() {
    const setPage=useUpdatePage();
    useEffect(() => {
        setPage("1")})
    const { id }=useParams();
    const item=movies.find(movie=>{
        return movie.id===id;
    })
    return (
        <Col className='column-right-MovieView'>
            <div className='MovieViewPoster'><MovieViewPoster key={item.id} id={item.id} title={item.title} poster={item.poster} releaseDate={item.release_date} genres={item.genres} rating={item.rating}/></div>
            <div className='splitScreen'>
                <div className='MovieViewSynopsis'><MovieViewSynopsis key={item.id} id={item.id} overview={item.overview} actors={item.actors} writers={item.writers} directors={item.directors} countryOrigin={item.countryOrigin} duration={item.duration}/></div>
                <div className='MovieViewTrailer'><MovieViewTrailer id={item.id}/></div>
            </div>
        </Col>
        
    )
}
export default MovieView;