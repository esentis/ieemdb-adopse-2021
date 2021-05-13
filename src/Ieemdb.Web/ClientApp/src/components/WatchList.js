import React,{useEffect} from 'react'
import TopRight from './TopRight'
import MovieCard from './MovieCard';
import movies from './Movie_Dataset';
import {useUpdatePage} from './GlobalContext'
import {Col} from 'react-bootstrap';

function WatchList() {
    const setPage=useUpdatePage();
    useEffect(() => {
        setPage("2")})
    
    const title='Watch List';
    var posters=movies.map(i => <MovieCard 
        id={i.id}
        Title={i.title} 
        Poster={i.posterUrl?i.posterUrl:"https://media.comicbook.com/files/img/default-movie.png"}  
        height={"250vh"} 
        width={'auto'}
        posterClass='poster'
        flag={false}/>)
    return (
        <Col className="column-right">
        <TopRight title={title}
                  items={posters} />
                  </Col>

    )
}

export default WatchList;
