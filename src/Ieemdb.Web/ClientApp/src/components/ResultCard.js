import React,{useState,useEffect} from "react";
import Moment from "react-moment";
import '../Styles/ResultCard.css'
import {useHistory} from 'react-router-dom';


const ResultCard = (props) => {
  var results=[];
  const history=useHistory();
  const [disable, setDisable] = useState(false)

  function MovieClick(){
    history.push("/Movie/"+props.id);
  }
  if(props.flag){
     results=props.featured.map(i=>i.id);
  }
  
  useEffect(()=>{
    if(results.find(x=>x===props.id)){
      setDisable(true);}
     else{setDisable(false)}
  },[results,props.id])
  
  
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
        
          
          {props.flag?<button
            className="addToFeaturedButton"
            disabled={disable}
            onClick={() => props.onClick(props.id,props.poster,props.title)}
          >
            Add to Featured
          </button>:""}
          
          
    </div>
    </div>
  );
};
export default ResultCard;
