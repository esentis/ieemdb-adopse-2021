import React from 'react'
import Select from 'react-select'
import '../Styles/AdvancedSearch.css'

function GenresSelect(props) {
    const genres=[
        {value:1,
        label:"Action",
        id:16},
        {value:2,
        label:"Horror",
        id:17},
        {value:3,
        label:"Adventure",
        id:18},
        {value:4,
        label:"Comedy",
        id:19},
        {value:5,
        label:"Fantasy",
        id:20},
        {value:6,
        label:"Science fiction",
        id:21},
        {value:7,
        label:"Western",
        id:22}
        ]
        

    return (
            <div className="GenresSelector">
            <Select styles={props.style} isMulti
            options={genres} placeholder="Select Movie Genres" onChange={props.onChange}/></div>  
    )
}

export default GenresSelect;
