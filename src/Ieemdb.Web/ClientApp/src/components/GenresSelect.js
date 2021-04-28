import React from 'react'
import Select from 'react-select'
import '../Styles/AdvancedSearch.css'

function GenresSelect(props) {
    const genres=[
        {value:1,
        label:"Action"},
        {value:2,
        label:"Horror"},
        {value:3,
        label:"Adventure"},
        {value:4,
        label:"Comedy"},
        {value:5,
        label:"Fantasy"},
        {value:6,
        label:"Science fiction"},
        {value:7,
        label:"Western"}
        ]
        

    return (
            <div className="GenresSelector">
            <Select styles={props.style} isMulti
            options={genres} placeholder="Select Movie Genres" onChange={props.onChange}/></div>  
    )
}

export default GenresSelect;
