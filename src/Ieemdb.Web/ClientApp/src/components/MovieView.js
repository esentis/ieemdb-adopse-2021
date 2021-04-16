import React from 'react'
import {Col} from 'react-bootstrap';
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
    )
}
export default MovieView;