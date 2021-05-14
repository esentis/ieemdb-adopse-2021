import React from 'react'
import Select from 'react-select';


function DateSelect(props) {

    const years=[];
    var d=new Date();
    for(var max=d.getFullYear(); max>=1900;max--){
            years.push({value:max,label:max})
    }
    
    return (
        <>
        <p></p>
        <div className="RatingDatesSelector" >
        <Select styles={props.style} 
         placeholder="Select Date" options={years} onChange={props.onChange1} />
        </div>
        <span className="spanClass">To</span>
        <div className="RatingDatesSelector">
        <Select styles={props.style} 
         placeholder="Select Date" onChange={props.onChange2} options={props.options}  value={props.value} />
        </div>
        </>
    )
}

export default DateSelect
