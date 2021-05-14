import React,{useEffect,useState} from 'react'
import {Col} from 'react-bootstrap';
import { useParams } from "react-router-dom";
import '../Styles/MovieView.css' 
import MovieViewPoster from './MovieViewPoster';
import MovieViewTrailer from './MovieViewTrailer';
import MovieViewSynopsis from './MovieViewSynopsis';
import LoadingSpinner from './LoadingSpinner';
import {useUpdatePage} from './GlobalContext';
import {useCheckLogin} from './GlobalContext';
import axios from 'axios'
function MovieView() {
   
    const setPage=useUpdatePage();
    const [items,setItems]=useState();
    const CheckLoginState=useCheckLogin();
    const [loading,setLoading]=useState(true);
    const [trailerUrl,setTrailerUrl]=useState();
    const [CheckWatchList,setCheckWatchList]=useState(false);
    const [CheckFavorite,setCheckFavorite]=useState(false);
    const [people,setPeople]=useState({
        Directors:{},
        Actors:{},
        Writers:{}
    });
    
    useEffect(() => {
        setPage("1")})
    const { id }=useParams();
    

    useEffect(()=>{
        async function fetchData(){
                const movieInfo=axios.get(`https://${window.location.host}/api/movie/${id}`);
                const trailer=axios.get(`https://${window.location.host}/api/movie/${id}/videos`);
                var checkFavoriteRequest="";
                var checkWatchListRequest="";

                if(CheckLoginState()){
                    checkFavoriteRequest=axios({method:'post',url:`https://${window.location.host}/api/favorite/check?movieId=${id}`,headers:{'Authorization':'Bearer ' + localStorage.getItem('token')}});
                    checkWatchListRequest=await axios({method:'post',url:`https://${window.location.host}/api/watchlist/check?movieId=${id}`,headers:{'Authorization':'Bearer ' + localStorage.getItem('token')}});
                }
            
                await axios.all([movieInfo,trailer,checkFavoriteRequest,checkWatchListRequest]).then(axios.spread((...responses)=>{
                   const MovieInfoResponse=responses[0];
                   const trailerResponse=responses[1];
                   const checkFavoriteResponse=responses[2];
                   const checkWatchListResponse=responses[3];
                   const MovieTrailer=trailerResponse.data.filter((video)=>"Trailer"===video.type)[0];
                   const directors=MovieInfoResponse.data.people.filter((movie)=>"Directing"===movie.knownFor);
                   const actors=MovieInfoResponse.data.people.filter((movie)=>"Acting"===movie.knownFor);
                   const writers=MovieInfoResponse.data.people.filter((movie)=>"Writing"===movie.knownFor);
                   setItems(MovieInfoResponse.data);
                   setPeople({Directors:directors,Actors:actors,Writers:writers});
                   setTrailerUrl(MovieTrailer);
                   setCheckFavorite(checkFavoriteResponse.data);
                   setCheckWatchList(checkWatchListResponse.data);
                   setLoading(false);
                }))
          }
          fetchData();
      },[id]);
       
    return (
        <Col className='column-right-MovieView'>
        {!loading? <><div className='MovieViewPoster'><MovieViewPoster key={items.id} id={items.id} title={items.title} poster={items.posterUrl} releaseDate={items.releaseDate} genres={items.genres} rating={items.averageRating} checkFavorite={CheckFavorite}/></div>
            <div className='splitScreen'>
                <div className='MovieViewSynopsis'><MovieViewSynopsis key={items.id} id={items.id} overview={items.plot} actors={people.Actors} writers={people.Writers} directors={people.Directors} countryOrigin={items.countries} duration={items.duration} checkWatchList={CheckWatchList}/></div>
                <div className='MovieViewTrailer'><MovieViewTrailer id={items.id} trailerKey={trailerUrl}/></div>
            </div></>:<LoadingSpinner color="#D3D3D3" loading={loading} size={20} />}
            
        </Col>
        
    )
}
export default MovieView;