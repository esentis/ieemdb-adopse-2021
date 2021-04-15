import React from 'react'
import {Col,Row} from 'react-bootstrap';
import '../Styles/MovieView.css' 
import MovieViewPoster from './MovieViewPoster';
import MovieViewTrailer from './MovieViewTrailer';
import MovieViewSynopsis from './MovieViewSynopsis';

/*
<Col className='column-right-MovieView'>
       <div style={{color:'white'}}>
       <p>Title:{props.Title}</p>
       <p>Poster:{props.Poster}</p>
       <p>Overview:{props.Overview}</p>
       <p>ReleaseDate:{props.ReleaseDate}</p>
       <p>Genres:{props.Genres}</p>
       <p>Actors:{props.Actors}</p>
       <p>Writers:{props.Writers}</p>
       <p>Directors:{props.Directors}</p>
       <p>Rating:{props.Rating}</p>
       <p>Duration:{props.Duration}</p>
       <p>CountryOrigin:{props.CountryOrigin}</p>
       </div>
       </Col>
*/ 


function MovieView(props) {
    return (
        /*<Col>
            <Row>
                <MovieViewPoster/>
            </Row>
            <Row>
                <Col>
                    <MovieViewSynopsis props/>
                </Col>
                <Col>
                    <MovieViewTrailer/>
                </Col>
            </Row>
        </Col>*/
        <Col className='column-right-MovieView'>
            <div className='splitScreen'>
                <div className='topPane'><MovieViewPoster/></div>
                <div className='bottomPane'><MovieViewTrailer/></div>
            </div>
        </Col>
    )
}

export default MovieView;