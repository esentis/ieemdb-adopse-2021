import React from 'react'
import {Col} from 'react-bootstrap';
import '../Styles/MovieView.css'


function MovieView(props) {
    return (
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
    )
}

export default MovieView;