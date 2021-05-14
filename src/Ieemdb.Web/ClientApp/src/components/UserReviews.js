import React, { useState } from 'react';
import '../Styles/MovieViewPoster.css';
import ReactStars from "react-rating-stars-component";
import axios from 'axios';

function UserReviews(props) {
  /*
    <ReactStars {...{
            value: { ratingStars }, size: 30, count: 5, color: "black", activeColor: "yellow", isHalf: false, edit: false,
            emptyIcon: <i className="fa fa-star-o" />, halfIcon: <i className="fa fa-star-half" />,
            filledIcon: <i className="fa fa-star" />
          }} />*/
  return (
    <div id="review">
      <div id="review1">
        <p className="revWriter">{props.userName}</p>
        <div className="revStars2">
          <p className="rating">{props.ratingStars}/10</p>
          
        </div>
      </div>
      <div id="review2">
        <p className="revComment">{props.reviewText}</p>
      </div>
    </div>
  );
}

export default UserReviews;
