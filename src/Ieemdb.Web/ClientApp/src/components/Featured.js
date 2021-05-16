import React,{useEffect,useState} from 'react'
import TopRight from './TopRight'
import MovieCard from './MovieCard';
import {useUpdatePage} from './GlobalContext';
import axios from 'axios';
import {Col} from 'react-bootstrap';

function Featured() {
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
        Poster={i.posterUrl==="https://image.tmdb.org/t/p/w600_and_h900_bestv2"?"https://media.comicbook.com/files/img/default-movie.png":i.posterUrl} 
        height={"350vh"} 
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
