import React from "react";
import Moment from "react-moment";
import '../Styles/ResultCard.css'
import {useHistory} from 'react-router-dom';

const ResultCard = (props) => {

  const history=useHistory();

  function MovieClick(){
    history.push("/Movie/"+props.id);
  }

  return (    
    <div className="result-card">
      <div className="poster-wrapper">
          <img
            src={props.poster}
            alt={props.title}
            onClick={MovieClick}
          />
      </div>
      <div className="info">
        <div className="header">
          <h3 className="title1" onClick={MovieClick}>{props.title}</h3>
          <h4 className="release-date">
            <Moment format="YYYY">{props.release_date}</Moment>
          </h4>
        </div>
    </div>
    </div>
  );
};
export default ResultCard;
