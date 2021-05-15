import React,{useEffect,useState} from 'react'
import TopRight from './TopRight'
import MovieCard from './MovieCard';
import movies from './Movie_Dataset';
import {useUpdatePage} from './GlobalContext';
import {Col} from 'react-bootstrap';
import axios from 'axios';
function Favorites() {
    const setPage=useUpdatePage();
    const [data,setData]=useState([]);
    const [loading,setLoading]=useState(true);
    useEffect(() => {
        setPage("2")
        async function fetchData(){
            await axios({method:'get',url:`https://${window.location.host}/api/favorite`,headers:{'Authorization':'Bearer ' + localStorage.getItem('token')}})
            .then(function(res){
                setData(res.data);
                setLoading(false);
            })}
        fetchData();},[setPage]);
    const title='Favorites';
    const items=data.map(i => <MovieCard 
        id={i.id}
        Title={i.title} 
        Poster={i.posterUrl?i.posterUrl:"https://media.comicbook.com/files/img/default-movie.png"} 
        height={"250vh"} 
        width={'auto'}
        posterClass='poster'
        flag={false} />)
    return (
        <Col className="column-right">
            <TopRight title={title}
                items={items}
                loading={loading} 
            />
        </Col>
    )
}
export default Favorites;