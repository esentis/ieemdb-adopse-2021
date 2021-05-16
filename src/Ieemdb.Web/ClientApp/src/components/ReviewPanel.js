import React, { useState } from 'react';
import '../Styles/MovieViewPoster.css';
import ReactStars from "react-rating-stars-component";
import axios from 'axios';

function ReviewPanel(props) {
  const [starrev, setstarrev] = useState('0');
  const [ratingText, setRatingText] = useState('');
  

  function handleChange(event) {
    setRatingText(event.target.value);
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
            size: 30, count: 10, color: "black", activeColor: "yellow", value: 0, a11y: true, isHalf: false,
            emptyIcon: <i className="fa fa-star-o" />, halfIcon: <i className="fa fa-star-half" />,
            filledIcon: <i className="fa fa-star" />, onChange: newValue => { setstarrev(`${newValue}`) }
          }} />
          <p className="rating">{starrev}/10</p>
        </div>
        <input className="reviewSubmit" type="submit" value="Submit" onClick={()=>props.onClick(starrev,ratingText)}></input>
      </div>
    </div>
  );
}

export default ReviewPanel;
