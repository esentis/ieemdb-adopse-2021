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
import UserReviews from './UserReviews';
import {Link} from 'react-router-dom';
function RatingStars(rating){
  return (<div id="divRate">
    <p className="rating">{rating.stars}/10</p>
    <ReactStars {...{value: rating.stars, size: 40, count: 10, color: "black", activeColor: "yellow", isHalf: true, edit: false,
      emptyIcon: <i className="fa fa-star-o" />, halfIcon: <i className="fa fa-star-half" />,
      filledIcon: <i className="fa fa-star" />}} />
    </div>
  );
}
function MovieViewPoster(props) {
    const [opre, setopre] = useState(false);
    const [item, setItem] = useState([]);
    const [onLoad, setOnLoad] = useState(true);
    const [reviewCheck,setReviewCheck]=useState(false);
    const [userReview,setUserReview]=useState();
    const [storeFavorite, setStoreFavorite] = useState("");
    const history=useHistory();
    const [addFavoriteButtonColor, setaddFavoriteButtonColor] = useState({background: 'rgba(52, 52, 52, 0)'});
    function HandleGenres(id,name){
        history.push('/Genre/GenreValue='+name+'/Id='+id);
    }
    const releaseDate = <Moment format="YYYY">{props.releaseDate}</Moment>
    const genres = props.genres.map((genre) =>
        <Genre name={genre.name} id={genre.id} onClick={HandleGenres}/>
    );
    const rating = props.rating;
    if (onLoad == true) {
        setStoreFavorite(props.checkFavorite);
        if (localStorage.getItem('token') == null) {
            setaddFavoriteButtonColor({background: 'rgba(52, 52, 52, 0)'});
        }
        else {
          console.log(storeFavorite);
          if (storeFavorite == true || props.checkFavorite == true) {
            setaddFavoriteButtonColor({background: 'white'});
          }
          else if (storeFavorite == false){
            setaddFavoriteButtonColor({background: 'rgba(52, 52, 52, 0)'});
          }
        }
        setOnLoad(false);
    }
    async function onFavButtonClick(){
        if (localStorage.getItem('token') == null) {
          history.push('/Login/');
        }
        else {
          if (storeFavorite == true) {
            await axios({
              method: 'delete', url: `https://${window.location.host}/api/favorite`, headers: { 'Authorization': 'Bearer ' + localStorage.getItem('token') }, params: {
                "movieId": props.id
              }
            }).then()
            setStoreFavorite(false);
            setaddFavoriteButtonColor({background: 'rgba(52, 52, 52, 0)'});
          }
          else if (storeFavorite == false) {
            await axios({
              method: 'post', url: `https://${window.location.host}/api/favorite`, headers: { 'Authorization': 'Bearer ' + localStorage.getItem('token') }, params: {
                "movieId": props.id
              }
            }).then()
            setStoreFavorite(true);
            setaddFavoriteButtonColor({background: 'white'});
          }
        }
    }
    function popupReview(){
        setopre(current => !current);
        FindRatings();
    }
    function backButton(){
        history.goBack();
    }
    async function FindRatings() {
      await axios({
        method: 'get', url: `https://${window.location.host}/api/movie/${props.id}/ratings`
      }).then(function(res){setItem(res.data);})
    }
    async function AddUserRating(rate,review) {
      if (rate == "0") {
        window.alert("You need to add star rating");
      }
      else {
        await axios({
          method:'post', url:`https://${window.location.host}/api/rating?movieId=${props.id}&rate=${rate}&review=${review}`, headers:{'Authorization':'Bearer '+localStorage.getItem('token')} 
        }).then(function(res){
          const newItem=[...item,res.data];
          setItem(newItem);
          setReviewCheck(true);
          setUserReview(res.data);
        })
      }
    }
    async function checkIfReviewed(){
        await axios({
          method:'post', url:`https://${window.location.host}/api/rating/check?movieId=${props.id}`, headers:{'Authorization':'Bearer '+localStorage.getItem('token')} 
        }).then(function(res){ 
          setReviewCheck(true);
          setUserReview(res.data);
          console.log(res.data);
        })
      }
      if (onLoad == true) {
        checkIfReviewed();
        setOnLoad(false);
      }
    function CheckIfLogin() {
      if (localStorage.getItem('token') == null) {
        return (
          <div id="add_review">
            <div className="col1Rev">
              <p className="addRevTitle">Add your review</p>
              <p className="revComment">You need to <Link className='toLogin' to='/Login'>Login</Link> in order to review</p>
            </div>
          </div>
        )
      }
      else{
        if(reviewCheck){
          return (
            <div id="removePanel">
              <button className="removeRatingButton" onClick={deleteComment}>Remove your rating</button>
            </div>
          ) 
        }
        else{
          return <ReviewPanel movieId={props.id} onClick={AddUserRating}/>
        }
      }
    }
    async function deleteComment(){
      await axios({
        method:'delete', url:`https://${window.location.host}/api/rating/delete?movieId=${props.id}`, headers:{'Authorization':'Bearer '+localStorage.getItem('token')} 
      }).then(res => {console.log(res);
        const newItem=item.filter((i)=>userReview.username!==i.username);
      setItem(newItem);
      setReviewCheck(false);
      })
      
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
                    <button className="buttonLove" style={addFavoriteButtonColor} onClick={onFavButtonClick}><i className="fa fa-heart"></i></button>
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
                        <hr className="line" />
                        {item.map(i=> <UserReviews userName={i.username}reviewText={i.review} ratingStars={i.rate} /> )}
                        <CheckIfLogin/>
                    </div>
                </div>
            </Modal>
        </Col>
    );
}
export default MovieViewPoster;
