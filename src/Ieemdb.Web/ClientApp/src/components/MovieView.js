import React,{useEffect,useState} from 'react'
import {Col} from 'react-bootstrap';
import { useParams } from "react-router-dom";
import '../Styles/MovieView.css' 
import MovieViewPoster from './MovieViewPoster';
import MovieViewTrailer from './MovieViewTrailer';
import MovieViewSynopsis from './MovieViewSynopsis';
import LoadingSpinner from './LoadingSpinner';
import {useUpdatePage} from './GlobalContext'
import axios from 'axios'
function MovieView() {
   
    const setPage=useUpdatePage();
    const [items,setItems]=useState();
    const [loading,setLoading]=useState(true);
    
    useEffect(() => {
        setPage("1")})
    const { id }=useParams();
    

    useEffect(()=>{
        async function fetchData(){
              const result=await axios.get(`https://${window.location.host}/api/movie/${id}`)
              setItems(result.data);
              setLoading(false);
          }
          fetchData();
      },[id]);
       
    return (
        <Col className='column-right-MovieView'>
        {!loading? <><div className='MovieViewPoster'><MovieViewPoster key={items.id} id={items.id} title={items.title} poster={items.posterUrl} releaseDate={items.releaseDate} genres={items.genres} rating={items.rating}/></div>
            <div className='splitScreen'>
                <div className='MovieViewSynopsis'><MovieViewSynopsis key={items.id} id={items.id} overview={items.plot} actors={items.actors} writers={items.writers} directors={items.directors} countryOrigin={items.countries} duration={items.duration}/></div>
                <div className='MovieViewTrailer'><MovieViewTrailer id={items.id} trailer={items.trailerUrl}/></div>
            </div></>:<LoadingSpinner color="#D3D3D3" loading={loading} size={20} />}
            
        </Col>
        
    )
}
export default MovieView;