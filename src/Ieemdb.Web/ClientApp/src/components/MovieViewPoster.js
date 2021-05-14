import React, {useState} from 'react';
import {Col,Row} from 'react-bootstrap';
import '../Styles/MovieViewPoster.css';
import {useHistory} from 'react-router-dom';
import Modal from 'react-awesome-modal';
import ReactStars from "react-rating-stars-component";
import Genre from './Genre';
import Moment from "react-moment";
import ReviewPanel from './ReviewPanel';
import axios from 'axios';

function RatingStars(rating){
    if (rating.stars < 1){
        return (<div id="divRate">
                    <p className="rating">{rating.stars}/5</p>
                    <ReactStars {...{value: 0, size: 40, count: 5, color: "black", activeColor: "yellow", isHalf: false, edit: false,
                                            emptyIcon: <i className="fa fa-star-o" />, halfIcon: <i className="fa fa-star-half" />,
                                            filledIcon: <i className="fa fa-star" />}} />
                </div>
        );
    }
    else if (rating.stars < 2){
        return (<div id="divRate">
                    <p className="rating">{rating.stars}/5</p>
                    <ReactStars {...{value: 1, size: 40, count: 5, color: "black", activeColor: "yellow", isHalf: false, edit: false,
                                            emptyIcon: <i className="fa fa-star-o" />, halfIcon: <i className="fa fa-star-half" />,
                                            filledIcon: <i className="fa fa-star" />}} />
                </div>
        );
    }
    else if (rating.stars < 3){
        return (<div id="divRate">
                    <p className="rating">{rating.stars}/5</p>
                    <ReactStars {...{value: 2, size: 40, count: 5, color: "black", activeColor: "yellow", isHalf: false, edit: false,
                                            emptyIcon: <i className="fa fa-star-o" />, halfIcon: <i className="fa fa-star-half" />,
                                            filledIcon: <i className="fa fa-star" />}} />
                </div>
        );
    }
    else if (rating.stars < 4){
        return (<div id="divRate">
                    <p className="rating">{rating.stars}/5</p>
                    <ReactStars {...{value: 3, size: 40, count: 5, color: "black", activeColor: "yellow", isHalf: false, edit: false,
                                            emptyIcon: <i className="fa fa-star-o" />, halfIcon: <i className="fa fa-star-half" />,
                                            filledIcon: <i className="fa fa-star" />}} />
                </div>
        );
    }
    else if (rating.stars < 5){
        return (<div id="divRate">
                    <p className="rating">{rating.stars}/5</p>
                    <ReactStars {...{value: 4, size: 40, count: 5, color: "black", activeColor: "yellow", isHalf: false, edit: false,
                                            emptyIcon: <i className="fa fa-star-o" />, halfIcon: <i className="fa fa-star-half" />,
                                            filledIcon: <i className="fa fa-star" />}} />
                </div>
        );
    }
    else{
        return (<div id="divRate">
                    <p className="rating">{rating.stars}/5</p>
                    <ReactStars {...{value: 5, size: 40, count: 5, color: "black", activeColor: "yellow", isHalf: false, edit: false,
                                            emptyIcon: <i className="fa fa-star-o" />, halfIcon: <i className="fa fa-star-half" />,
                                            filledIcon: <i className="fa fa-star" />}} />
                </div>
        );
    }
}
function MovieViewPoster(props) {
    const [opre, setopre] = useState(false);
    const history=useHistory();
    function HandleGenres(id,name){
        history.push('/Genre/GenreValue='+name+'/Id='+id);
    }
    /*const id=props.id;*/
    const releaseDate = <Moment format="YYYY">{props.releaseDate}</Moment>
    const genres = props.genres.map((genre) =>
        <Genre name={genre.name} id={genre.id} onClick={HandleGenres}/>
    );
    const rating = props.rating;
    function onFavButtonClick(){
        //Otan kanei klik sto ADD FAVORITE button
        console.log("Click on ADD FAVORITE button");
    }
    function popupReview(){
        setopre(current => !current);
    }
    function backButton(){
        history.goBack();
    }
    
    function CheckIfLogin() {
      console.log(localStorage.getItem('token'));
      if (localStorage.getItem('token') == null) {
        return <p>You need to Login in order to review</p>
      }
      else {
        return <ReviewPanel movieId={props.id}/>
      }
    }

    console.log("Favorite:",props.checkFavorite);
    return(
        <Col className="backStyle" style={{backgroundImage: `linear-gradient(rgba(41, 44, 52, 0.5), rgba(41, 44, 52, 0.5), rgba(41, 44, 52, 0.5), rgba(41, 44, 52, 0.5), rgba(41, 44, 52, 0.5), rgba(41, 44, 52, 0.7), rgba(41, 44, 52, 0.9), rgba(41, 44, 52)), url(${props.poster})`}}>
            <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/4.7.0/css/font-awesome.min.css"></link>
            <Row className="dtop">
                <button className="buttonReturn" onClick={backButton}><i id="return" className="fa fa-arrow-left"></i></button>
            </Row>
            <Row className="dcenter">
                <div id="divTitle">
                    <p className="movieTitle">{props.title} ({releaseDate})</p>
                </div>
                <div id="divFavorReview">
                    <button className="buttonLove" onClick={onFavButtonClick}><i className="fa fa-heart"></i></button>
                    <button className="buttonReview" onClick={popupReview}><i className="fa fa-star"></i>  REVIEWS</button>
                </div>
            </Row>
            <Row className="dbottom">
                <div id="divDesc">
                    <p className="movieDescGenres">{genres}</p>
                </div>
                <RatingStars stars={rating}/>
            </Row>
            <Modal keyboard='true' visible={opre} width="90%" height="80%" effect="fadeInRight" onClickAway={popupReview}>
                <div id="popRev">
                    <div id="popBody">
                        <div id="popHeader"> 
                            <button className="buttonClose" onClick={popupReview}><i id="return" className="fa fa-close"></i></button>
                            <p className="revTitle">Review for {props.title} ({releaseDate})</p>
                            <div className="revStars">
                                <RatingStars stars={rating}/>
                            </div>
                        </div>
                        <hr className="line"/>
                        <div id="review">
                            <div id="review1">
                                <p className="revWriter">Fanis Georgiou</p>
                                <div className="revStars2">
                                    <p className="rating">1/5</p>
                                    <ReactStars {...{value: 5, size: 30, count: 5, color: "black", activeColor: "yellow", isHalf: false, edit: false,
                                            emptyIcon: <i className="fa fa-star-o" />, halfIcon: <i className="fa fa-star-half" />,
                                            filledIcon: <i className="fa fa-star" />}} />
                                </div>
                            </div>
                            <div id="review2">
                                <p className="revComment">I've liked Brie Larson in other films, but she showed ZERO range in this. When your main character in a superhero movie is unwatchable, you already have a problem. In addition, Captain Marvel has no weaknesses, which kills the tension immediately. There is no point at which you feel she is in any danger of losing, or any danger at all for that matter.
                                                        It's an OK origin story, but it makes no sense as to WHY she's supposedly so powerful. The cat was good.</p>
                            </div>
                            </div>
                            <CheckIfLogin/>
                    </div>
                </div>
            </Modal>
        </Col>
    );
}
export default MovieViewPoster;
