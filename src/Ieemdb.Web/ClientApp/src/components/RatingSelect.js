import React from 'react'
import Select from 'react-select'
import '../Styles/AdvancedSearch.css'


function RatingSelect(props) {
    
   const Ratings=[
            {value:1,
            label:1,
            id:1},
            {value:2,
            label:2,
            id:2},
            {value:3,
            label:3,
            id:3},
            {value:4,
            label:4,
            id:4},
            {value:5,
            label:5,
            id:5}
    ]
   

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
        options={props.options} placeholder="Select rating"   onChange={props.onChange2} isDisabled={props.isDisabled} value={props.value}/>
    </div> 
    </>
    )
}

export default RatingSelect;
