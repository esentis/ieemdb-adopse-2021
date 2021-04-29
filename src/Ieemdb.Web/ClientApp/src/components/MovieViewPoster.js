import React, {useState} from 'react';
import {Col,Row} from 'react-bootstrap';
import '../Styles/MovieViewPoster.css';
import {useHistory} from 'react-router-dom';
import Modal from 'react-awesome-modal';
import ReactStars from "react-rating-stars-component";
function RatingStars(rating){
    if (rating.stars/2 < 1){
        return (<div id="divRate">
                    <p className="rating">{rating.stars}/10</p>
                    <ReactStars {...{value: 0, size: 40, count: 5, color: "black", activeColor: "yellow", isHalf: false, edit: false,
                                            emptyIcon: <i className="fa fa-star-o" />, halfIcon: <i className="fa fa-star-half" />,
                                            filledIcon: <i className="fa fa-star" />}} />
                </div>
        );
    }
    else if (rating.stars/2 < 2){
        return (<div id="divRate">
                    <p className="rating">{rating.stars}/10</p>
                    <ReactStars {...{value: 1, size: 40, count: 5, color: "black", activeColor: "yellow", isHalf: false, edit: false,
                                            emptyIcon: <i className="fa fa-star-o" />, halfIcon: <i className="fa fa-star-half" />,
                                            filledIcon: <i className="fa fa-star" />}} />
                </div>
        );
    }
    else if (rating.stars/2 < 3){
        return (<div id="divRate">
                    <p className="rating">{rating.stars}/10</p>
                    <ReactStars {...{value: 2, size: 40, count: 5, color: "black", activeColor: "yellow", isHalf: false, edit: false,
                                            emptyIcon: <i className="fa fa-star-o" />, halfIcon: <i className="fa fa-star-half" />,
                                            filledIcon: <i className="fa fa-star" />}} />
                </div>
        );
    }
    else if (rating.stars/2 < 4){
        return (<div id="divRate">
                    <p className="rating">{rating.stars}/10</p>
                    <ReactStars {...{value: 3, size: 40, count: 5, color: "black", activeColor: "yellow", isHalf: false, edit: false,
                                            emptyIcon: <i className="fa fa-star-o" />, halfIcon: <i className="fa fa-star-half" />,
                                            filledIcon: <i className="fa fa-star" />}} />
                </div>
        );
    }
    else if (rating.stars/2 < 5){
        return (<div id="divRate">
                    <p className="rating">{rating.stars}/10</p>
                    <ReactStars {...{value: 4, size: 40, count: 5, color: "black", activeColor: "yellow", isHalf: false, edit: false,
                                            emptyIcon: <i className="fa fa-star-o" />, halfIcon: <i className="fa fa-star-half" />,
                                            filledIcon: <i className="fa fa-star" />}} />
                </div>
        );
    }
    else{
        return (<div id="divRate">
                    <p className="rating">{rating.stars}/10</p>
                    <ReactStars {...{value: 5, size: 40, count: 5, color: "black", activeColor: "yellow", isHalf: false, edit: false,
                                            emptyIcon: <i className="fa fa-star-o" />, halfIcon: <i className="fa fa-star-half" />,
                                            filledIcon: <i className="fa fa-star" />}} />
                </div>
        );
    }
}
function MovieViewPoster(props){
    const [opre, setopre] = useState(false);
    const [starrev, setstarrev] = useState('0');
    const history=useHistory();
    const id=props.id; 
    const releaseDate = props.releaseDate.substring(0,4);
    const genres = props.genres.map((genre) =>
        <p className="movieDescGenre" alt={id}>{genre}</p>
    );
    const rating = props.rating;
    const durationHours = Math.floor(props.duration / 50);
    const durationMinutes = props.duration % 60;
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
    return(
        <Col className="backStyle" style={{backgroundImage: `linear-gradient(rgba(41, 44, 52, 0.5), rgba(41, 44, 52, 0.5), rgba(41, 44, 52, 0.5), rgba(41, 44, 52, 0.5), rgba(41, 44, 52, 0.5), rgba(41, 44, 52, 0.7), rgba(41, 44, 52, 0.9), rgba(41, 44, 52)), url(${props.poster})`}}>
            <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/4.7.0/css/font-awesome.min.css"></link>
            <Row className="top">
                <button className="buttonReturn" onClick={backButton}><i id="return" className="fa fa-arrow-left"></i></button>
            </Row>
            <Row className="center">
                <div id="divTitle">
                    <p className="movieTitle">{props.title}</p>
                </div>
                <div id="divFavorShare">
                    <button className="buttonLove" onClick={onFavButtonClick}><i className="fa fa-heart"></i></button>
                    <button className="buttonShare" onClick={popupReview}><i className="fa fa-star"></i>  REVIEWS</button>
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
            <Modal keyboard='true' visible={opre} width="90%" height="90%" effect="fadeInRight" onClickAway={popupReview}>
                <div id="popRev">  
                    <div id="popHeader"> 
                        <button className="buttonClose" onClick={popupReview}><i id="return" className="fa fa-close"></i></button>
                        <p className="revTitle">Review for {props.title} ({releaseDate})</p>
                        <div className="revStars">
                            <RatingStars stars={rating}/>
                        </div>
                    </div>
                    <hr className="line"/>
                    <div id="popBody">
                        <div id="review">
                            <div id="review1">
                                <p className="revWriter">Petros Apostolopoulos</p>
                                <div className="revStars2">
                                    <p className="rating">1/5</p>
                                    <ReactStars {...{value: 1, size: 30, count: 5, color: "black", activeColor: "yellow", isHalf: false, edit: false,
                                            emptyIcon: <i className="fa fa-star-o" />, halfIcon: <i className="fa fa-star-half" />,
                                            filledIcon: <i className="fa fa-star" />}} />
                                </div>
                            </div>
                            <div id="review2">
                                <p className="revComment">Our comprehensive guidiuyie tooo CSS flexbox layout. This complete guide explains everything about flexbox, focusing on all the different possible properties for the parent element (the flex container) and the child elements (the flex items). It also includes history, demos, patterns, and a browser support chart.</p>
                            </div>
                        </div>
                        <div id="add_review">
                            <div className="col1Rev">
                                <p className="addRevTitle">Add your review</p>
                                <input className="nameInput" type="text" id="name" name="name" placeholder="Name"/>
                                <input className="surnameInput" type="text" id="surname" name="surname" placeholder="Surname"/>
                                <div className="revStars3">
                                    <ReactStars {...{size: 30, count: 5, color: "black", activeColor: "yellow", value: 0, a11y: true, isHalf: false, 
                                            emptyIcon: <i className="fa fa-star-o" />, halfIcon: <i className="fa fa-star-half" />,
                                            filledIcon: <i className="fa fa-star" />, onChange: newValue => {setstarrev(`${newValue}`)}}} />
                                    <p className="rating">{starrev}/5</p>
                                </div>
                            </div>
                            <div className="col2Rev">
                                <p className="emptyRow"></p>
                                <textarea className="reviewInput" type="text" id="fname" name="review" placeholder="Add your review" multiple/>
                                <input className="reviewSubmit" type="submit" value="Submit"></input>
                            </div>
                        </div>
                    </div>
                </div>
            </Modal>
        </Col>
    );
}
export default MovieViewPoster;