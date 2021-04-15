import React from 'react';
import {Col,Row} from 'react-bootstrap';
import '../Styles/MovieViewPoster.css'
function RatingStars(rating){
    if (rating.stars/2 < 1){
        return (<div id="divRate">
                    <p className="rating">{rating.stars}/10</p>
                    <i id="empty" class="fa fa-star"></i>
                    <i id="star_whi" class="fa fa-star"></i>
                    <i id="empty" class="fa fa-star"></i>
                    <i id="star_whi" class="fa fa-star"></i>
                    <i id="empty" class="fa fa-star"></i>
                    <i id="star_whi" class="fa fa-star"></i>
                    <i id="empty" class="fa fa-star"></i>
                    <i id="star_whi" class="fa fa-star"></i>
                    <i id="empty" class="fa fa-star"></i>
                    <i id="star_whi" class="fa fa-star"></i>
                </div>
        );
    }
    else if (rating.stars/2 < 2){
        return (<div id="divRate">
                    <p className="rating">{rating.stars}/10</p>
                    <i id="empty" class="fa fa-star"></i>
                    <i id="star_yel" class="fa fa-star"></i>
                    <i id="empty" class="fa fa-star"></i>
                    <i id="star_whi" class="fa fa-star"></i>
                    <i id="empty" class="fa fa-star"></i>
                    <i id="star_whi" class="fa fa-star"></i>
                    <i id="empty" class="fa fa-star"></i>
                    <i id="star_whi" class="fa fa-star"></i>
                    <i id="empty" class="fa fa-star"></i>
                    <i id="star_whi" class="fa fa-star"></i>
                </div>
        );
    }
    else if (rating.stars/2 < 3){
        return (<div id="divRate">
                    <p className="rating">{rating.stars}/10</p>
                    <i id="empty" class="fa fa-star"></i>
                    <i id="star_yel" class="fa fa-star"></i>
                    <i id="empty" class="fa fa-star"></i>
                    <i id="star_yel" class="fa fa-star"></i>
                    <i id="empty" class="fa fa-star"></i>
                    <i id="star_whi" class="fa fa-star"></i>
                    <i id="empty" class="fa fa-star"></i>
                    <i id="star_whi" class="fa fa-star"></i>
                    <i id="empty" class="fa fa-star"></i>
                    <i id="star_whi" class="fa fa-star"></i>
                </div>
        );
    }
    else if (rating.stars/2 < 4){
        return (<div id="divRate">
                    <p className="rating">{rating.stars}/10</p>
                    <i id="empty" class="fa fa-star"></i>
                    <i id="star_yel" class="fa fa-star"></i>
                    <i id="empty" class="fa fa-star"></i>
                    <i id="star_yel" class="fa fa-star"></i>
                    <i id="empty" class="fa fa-star"></i>
                    <i id="star_yel" class="fa fa-star"></i>
                    <i id="empty" class="fa fa-star"></i>
                    <i id="star_whi" class="fa fa-star"></i>
                    <i id="empty" class="fa fa-star"></i>
                    <i id="star_whi" class="fa fa-star"></i>
                </div>
        );
    }
    else if (rating.stars/2 < 5){
        return (<div id="divRate">
                    <p className="rating">{rating.stars}/10</p>
                    <i id="empty" class="fa fa-star"></i>
                    <i id="star_yel" class="fa fa-star"></i>
                    <i id="empty" class="fa fa-star"></i>
                    <i id="star_yel" class="fa fa-star"></i>
                    <i id="empty" class="fa fa-star"></i>
                    <i id="star_yel" class="fa fa-star"></i>
                    <i id="empty" class="fa fa-star"></i>
                    <i id="star_yel" class="fa fa-star"></i>
                    <i id="empty" class="fa fa-star"></i>
                    <i id="star_whi" class="fa fa-star"></i>
                </div>
        );
    }
    else{
        return (<div id="divRate">
                    <p className="rating">{rating.stars}/10</p>
                    <i id="empty" class="fa fa-star"></i>
                    <i id="star_yel" class="fa fa-star"></i>
                    <i id="empty" class="fa fa-star"></i>
                    <i id="star_yel" class="fa fa-star"></i>
                    <i id="empty" class="fa fa-star"></i>
                    <i id="star_yel" class="fa fa-star"></i>
                    <i id="empty" class="fa fa-star"></i>
                    <i id="star_yel" class="fa fa-star"></i>
                    <i id="empty" class="fa fa-star"></i>
                    <i id="star_yel" class="fa fa-star"></i>
                </div>
        );
    }
}
function MovieViewPoster(props){
    const key = props.key;
    const title = props.title;
    const poster = props.poster;
    const releaseDate = props.releaseDate.toString().substring(0,4);
    const genres = props.genres.map((genre) =>
        <p className="movieDescGenre">{genre}</p>
    );
    const rating = props.rating;
    const durationHours = Math.floor(props.duration / 50);
    const durationMinutes = props.duration % 60;
    return(
        <Col className="backStyle" style={{backgroundImage: `linear-gradient(rgba(41, 44, 52, 0.5), rgba(41, 44, 52, 0.5), rgba(41, 44, 52, 0.5), rgba(41, 44, 52, 0.5), rgba(41, 44, 52, 0.5), rgba(41, 44, 52, 0.7), rgba(41, 44, 52, 0.9), rgba(41, 44, 52)), url(${poster})`}}>
            <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/4.7.0/css/font-awesome.min.css"></link>
            <Row className="top">
                <button className="buttonReturn"><i id="return" class="fa fa-arrow-left"></i></button>
            </Row>
            <Row className="center">
                <div id="divTitle">
                    <p className="movieTitle">{title}</p>
                </div>
                <div id="divFavor">
                    <button className="buttonLove"><i class="fa fa-heart"></i></button>
                </div>
                <div id="divShare">
                    <button className="buttonShare"><i class="fa fa-share-alt"></i>  SHARE</button>
                </div>
            </Row>
            <Row className="bottom">
                <div id="divDesc">
                    <p className="movieDesc">{releaseDate}</p>
                    <p className="movieDescGenres">{genres}</p>
                    {durationMinutes > 0
                        ? <p className="movieDesc">{durationHours} hours and {durationMinutes} minutes</p>
                        : <p className="movieDesc">{durationHours} hours</p>
                    }
                </div>
                <RatingStars stars={rating}/>
            </Row>
        </Col>
    );
}
export default MovieViewPoster;