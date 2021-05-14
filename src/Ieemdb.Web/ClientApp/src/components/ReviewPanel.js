import React, { useState } from 'react';
import '../Styles/MovieViewPoster.css';
import ReactStars from "react-rating-stars-component";
import axios from 'axios';

function ReviewPanel(movieId) {
  const [starrev, setstarrev] = useState('0');
  const [ratingText, setRatingText] = useState('');

  function RatingStars(rating) {
    if (rating.stars < 1) {
      return (<div id="divRate">
        <p className="rating">{rating.stars}/5</p>
        <ReactStars {...{
          value: 0, size: 40, count: 5, color: "black", activeColor: "yellow", isHalf: false, edit: false,
          emptyIcon: <i className="fa fa-star-o" />, halfIcon: <i className="fa fa-star-half" />,
          filledIcon: <i className="fa fa-star" />
        }} />
      </div>
      );
    }
    else if (rating.stars < 2) {
      return (<div id="divRate">
        <p className="rating">{rating.stars}/5</p>
        <ReactStars {...{
          value: 1, size: 40, count: 5, color: "black", activeColor: "yellow", isHalf: false, edit: false,
          emptyIcon: <i className="fa fa-star-o" />, halfIcon: <i className="fa fa-star-half" />,
          filledIcon: <i className="fa fa-star" />
        }} />
      </div>
      );
    }
    else if (rating.stars < 3) {
      return (<div id="divRate">
        <p className="rating">{rating.stars}/5</p>
        <ReactStars {...{
          value: 2, size: 40, count: 5, color: "black", activeColor: "yellow", isHalf: false, edit: false,
          emptyIcon: <i className="fa fa-star-o" />, halfIcon: <i className="fa fa-star-half" />,
          filledIcon: <i className="fa fa-star" />
        }} />
      </div>
      );
    }
    else if (rating.stars < 4) {
      return (<div id="divRate">
        <p className="rating">{rating.stars}/5</p>
        <ReactStars {...{
          value: 3, size: 40, count: 5, color: "black", activeColor: "yellow", isHalf: false, edit: false,
          emptyIcon: <i className="fa fa-star-o" />, halfIcon: <i className="fa fa-star-half" />,
          filledIcon: <i className="fa fa-star" />
        }} />
      </div>
      );
    }
    else if (rating.stars < 5) {
      return (<div id="divRate">
        <p className="rating">{rating.stars}/5</p>
        <ReactStars {...{
          value: 4, size: 40, count: 5, color: "black", activeColor: "yellow", isHalf: false, edit: false,
          emptyIcon: <i className="fa fa-star-o" />, halfIcon: <i className="fa fa-star-half" />,
          filledIcon: <i className="fa fa-star" />
        }} />
      </div>
      );
    }
    else {
      return (<div id="divRate">
        <p className="rating">{rating.stars}/5</p>
        <ReactStars {...{
          value: 5, size: 40, count: 5, color: "black", activeColor: "yellow", isHalf: false, edit: false,
          emptyIcon: <i className="fa fa-star-o" />, halfIcon: <i className="fa fa-star-half" />,
          filledIcon: <i className="fa fa-star" />
        }} />
      </div>
      );
    }
  }
  function handleChange(event) {
    setRatingText({ value: event.target.value });
    console.log(ratingText);
  }

  async function AddUserRating() {
    if (starrev == "0") {
      window.alert("You need to add star rating and text rating");
    }
    else {
      await axios({
        method: 'post', url: `https://${window.location.host}/api/rating`, headers: { 'Authorization': 'Bearer ' + localStorage.getItem('token') }, params: {
          "movieId": movieId, "rate": starrev, "review": ratingText
        }
      }).then(res => console.log(res))
      setstarrev("0");
      setRatingText("");
    }
  }
  return (
    <div id="add_review">
      <div className="col1Rev">
        <p className="addRevTitle">Add your review</p>
        <textarea className="reviewInput" type="text" id="fname" name="review" placeholder="Write your review here"
          onChange={handleChange} multiple />
      </div>

      <div className="col2Rev">
        <p className="emptyRow"></p>
        <div className="revStars3">
          <ReactStars {...{
            size: 30, count: 5, color: "black", activeColor: "yellow", value: 0, a11y: true, isHalf: false,
            emptyIcon: <i className="fa fa-star-o" />, halfIcon: <i className="fa fa-star-half" />,
            filledIcon: <i className="fa fa-star" />, onChange: newValue => { setstarrev(`${newValue}`) }
          }} />
          <p className="rating">{starrev}/5</p>
        </div>
        <input className="reviewSubmit" type="submit" value="Submit" onClick={AddUserRating}></input>
      </div>
    </div>
  );
}

export default ReviewPanel;
