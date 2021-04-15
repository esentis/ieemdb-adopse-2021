import React from 'react'
import {Col} from 'react-bootstrap';
import '../Styles/MovieView.css' 
import MovieViewPoster from './MovieViewPoster';
import MovieViewTrailer from './MovieViewTrailer';
import MovieViewSynopsis from './MovieViewSynopsis';

function MovieView(props) {
    const key = props.key;
    const title = props.Title;
    const poster = props.Poster;
    const overview = props.Overview;
    const releaseDate = props.ReleaseDate;
    const genres = props.Genres;
    const actors = props.Actors;
    const writers = props.Writers;
    const directors = props.Directors;
    const rating = props.Rating;
    const duration = props.Duration;
    const countryOrigin = props.CountryOrigin;
    console.log(key); //key de douleuei gia kapoio logo 
    return (
        <Col className='column-right-MovieView'>
            <div className='MovieViewPoster'><MovieViewPoster key={key} title={title} poster={poster} releaseDate={releaseDate} genres={genres} rating={rating} duration={duration}/></div>
            <div className='splitScreen'>
                <div className='MovieViewSynopsis'><MovieViewSynopsis key={key} overview={overview} actors={actors} writers={writers} directors={directors} countryOrigin={countryOrigin}/></div>
                <div className='MovieViewTrailer'><MovieViewTrailer key={key}/></div>
            </div>
        </Col>
    )
}
export default MovieView;