import React from 'react'
import Select from 'react-select'
import '../Styles/AdvancedSearch.css'


function RatingSelect(props) {
    
    const Ratings=[];
    for(var i=1;i<=5;i++){
        Ratings.push({value:i,label:i});
    }

return (
    <>
    <p></p>
    <div className="RatingDatesSelector">
        <Select styles={props.style}
        options={Ratings} placeholder="Select rating" onChange={props.onChange1} />
    </div>
   
     <span className="spanClass">To</span>
    <div className="RatingDatesSelector">
        <Select styles={props.style}  
        options={props.options} placeholder="Select rating"   onChange={props.onChange2} value={props.value}/>
    </div> 
    </>
    )
}

export default RatingSelect;
