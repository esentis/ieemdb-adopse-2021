import React,{useEffect,useState} from 'react'
import {Col} from 'react-bootstrap';
import { useParams } from "react-router-dom";
import '../Styles/MovieView.css' 
import MovieViewPoster from './MovieViewPoster';
import MovieViewTrailer from './MovieViewTrailer';
import MovieViewSynopsis from './MovieViewSynopsis';
import movies from './Movie_Dataset';
import {useUpdatePage} from './GlobalContext'
import axios from 'axios'
import { css } from "@emotion/core";
import PropagateLoader from "react-spinners/PropagateLoader";
function MovieView() {
    const override = css`
  display: block;
  margin: auto;
  border-color: "#D3D3D3";
`;
    const setPage=useUpdatePage();
    const [items,setItems]=useState();
    const [isLoading,setIsLoading]=useState(true);
    
    useEffect(() => {
        setPage("1")})
    const { id }=useParams();
    

    useEffect(()=>{
        async function fetchData(){
              const result=await axios.get(`https://${window.location.host}/api/movie/${id}`)
              setItems(result.data);
              setIsLoading(false);
              
          }
          fetchData();
      },[]);
       
    return (
        <Col className='column-right-MovieView'>
        {!isLoading? <><div className='MovieViewPoster'><MovieViewPoster key={items.id} id={items.id} title={items.title} poster={items.posterUrl} releaseDate={items.releaseDate} genres={items.genres} rating={items.rating} duration={items.duration}/></div>
            <div className='splitScreen'>
                <div className='MovieViewSynopsis'><MovieViewSynopsis key={items.id} id={items.id} overview={items.plot} actors={items.actors} writers={items.writers} directors={items.directors} countryOrigin={items.countries}/></div>
                <div className='MovieViewTrailer'><MovieViewTrailer id={items.id} trailer={items.trailerUrl}/></div>
            </div></>:<PropagateLoader color="#D3D3D3" loading={isLoading} css={override} size={20} />}
            
        </Col>
        
    )
}
export default MovieView;