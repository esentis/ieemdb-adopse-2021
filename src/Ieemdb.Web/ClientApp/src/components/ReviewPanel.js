import React, { useState } from 'react';
import '../Styles/MovieViewPoster.css';
import ReactStars from "react-rating-stars-component";
import axios from 'axios';

function ReviewPanel({movieId}) {
  const [starrev, setstarrev] = useState('0');
  const [ratingText, setRatingText] = useState('');
  

  function handleChange(event) {
    setRatingText(event.target.value);
    console.log(ratingText);
  }

  async function AddUserRating() {
    if (starrev == "0") {
      window.alert("You need to add star rating");
    }
    else {
      console.log(movieId);
      await axios({
        method:'post', url:`https://${window.location.host}/api/rating?movieId=${movieId}&rate=${starrev}&review=${ratingText}`, headers:{'Authorization':'Bearer '+localStorage.getItem('token')} 
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
