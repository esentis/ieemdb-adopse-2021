import axios from 'axios';
import React,{useEffect,useState} from 'react'
import Select from 'react-select'
import '../Styles/AdvancedSearch.css'

function GenresSelect(props) {

    const [Genres,setGenres]=useState();
   async function getGenres(){
    await axios({method:'post',url:`https://${window.location.host}/api/genre/all`,data:{"page":1,"itemsPerPage":50,}}).then(function(res){
    const genres=res.data.results.map((genre,index)=>{
        const genres={value:index,label:genre.name,id:genre.id};
        return genres;
    })
    setGenres(genres);
    })
    }
   
   useEffect(() => {
            getGenres();
        }, [])
    return (
            <div className="GenresSelector">
            <Select styles={props.style} isMulti
            options={Genres} placeholder="Select Movie Genres" onChange={props.onChange}/></div>  
    )
}

export default GenresSelect;
