import React,{useEffect,useState} from 'react'
import TopRight from './TopRight'
import MovieCard from './MovieCard';
import movies from './Movie_Dataset';
import {useUpdatePage} from './GlobalContext';
import axios from 'axios';
import {Col} from 'react-bootstrap';
import PropagateLoader from "react-spinners/PropagateLoader";
import { css } from "@emotion/core";

function Featured() {
    const override = css`
  display: block;
  margin: auto;
  border-color: "#D3D3D3";
`;
    const setPage=useUpdatePage();
    const [data,setData]=useState([]);
    const [loading,setLoading]=useState(true);
    useEffect(() => {
        setPage("2")
        async function fetchData(){
            await axios({method:'post',url:`https://${window.location.host}/api/movie/search`,data:{"page":1,"itemsPerPage":20,"isFeatured": true}})
            .then(function(res){
                setData(res.data.results);
                setLoading(false);
            })}
        fetchData();},[setPage]);
    const title='Featured';
    const items=data.map(i => <MovieCard 
        id={i.id}
        Title={i.title} 
        Poster={i.posterUrl} 
        Overview={i.overview}
        ReleaseDate={i.release_date}
        Genres={i.genres}
        Actors={i.actors}
        Writers={i.writers}
        Directors={i.directors}
        Rating={i.rating}
        Duration={i.duration}
        CountryOrigin={i.countryOrigin}
        height={"250vh"} 
        width={'auto'}
        posterClass='poster'
        flag={false} />)
    return (
        <Col className="column-right">
        <TopRight title={title}
                  items={items}
                  loading={loading}  />
                  </Col>
    )
}

export default Featured
