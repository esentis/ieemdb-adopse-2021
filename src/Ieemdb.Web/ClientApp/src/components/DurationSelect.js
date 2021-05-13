import React from 'react'
import Select from 'react-select';

function DurationSelect(props) {
    var Duration=[];
    for(var i=20;i<=360;i+=10){
        Duration.push({value:i,label:i})
    }
    return (
        <>
        <p></p>
        <div className="RatingDatesSelector" >
        <Select styles={props.style} 
         placeholder="Pick Duration" options={Duration} onChange={props.onChange1} />
        </div>
        <span className="spanClass">To</span>
        <div className="RatingDatesSelector">
        <Select styles={props.style} 
         placeholder="Pick Duration" onChange={props.onChange2} options={props.options}  value={props.value} />
        </div> 
        </>
    )
}

export default DurationSelect
