import React,{useEffect,useState} from 'react'
import {Col} from 'react-bootstrap';
import '../Styles/SearchView.css'
import {useUpdatePage} from './GlobalContext'
import { useParams } from "react-router-dom";
import Results from './Results';
import Paginate from './Paginate';
import '../Styles/Paginate.css'
import axios from 'axios';
import LoadingSpinner from './LoadingSpinner';



function SearchView() {
    var SearchValue=""
    var { value,SearchType,MovieTitle,ActorName,DirectorName,WriterName,MinDuration,MaxDuration,Genres,FromRating,ToRating,FromDate,ToDate,GenreId }=useParams();
    if(value===undefined){
        value=null
    }
    const [loading,setLoading]=useState(true);
    var genres=[];
    if(SearchType==="AdvancedSearchResults"){
        SearchValue="AdvancedSearch"
        if(Genres!==undefined){
           if(Genres.length>1){
            var commaCount=Genres.match((/,/g) || []).length
            for(var i=0;i<=commaCount;i++){
            genres.push(Genres.split(',')[i]);
        }
           }else{genres.push(Genres)}}
        
        
       
    }else{SearchValue=value}
    
    const [currentPage,setCurrentPage]=useState(0);
    const [items,setItems]=useState({
        data:[],
        totalResults:0,
        pageCount:0
    });

    const setPage=useUpdatePage();
    useEffect(() => {
        setPage("1")
    })
    const postersPerPage=10;

    //GenreSearch
    async function fetchDataByGenre(arg){
        await axios({method:'post',url:`https://${window.location.host}/api/movie/search`,data:{"page":arg+1,"itemsPerPage":postersPerPage,"genres":[GenreId]  
    }
    })
        .then(function (res){
            setItems({data:res.data.results,
                      pageCount:Math.ceil(res.data.totalElements/postersPerPage),
                      totalResults:res.data.totalElements})
                      setLoading(false)});
                        
    }
    //advancedSearch
   async function advancedSearchFetch(arg){
        await axios({method:'post',url:`https://${window.location.host}/api/movie/search`,data:{"page":arg+1,"itemsPerPage":postersPerPage,...(MovieTitle?{"titleCriteria":MovieTitle}:{}),
        ...(ActorName?{"actor":ActorName}:{}),...(DirectorName?{"director":DirectorName}:{}),...(WriterName?{"writer":WriterName}:{}),...(FromRating?{"minRating":FromRating}:{}),...(ToRating?{"maxRating":ToRating}:{}),
        ...(FromDate?{"fromYear":`${FromDate}-01-01`}:{}),...(ToDate?{"toYear":`${ToDate}-12-31`}:{}),...(MinDuration?{"minDuration":MinDuration}:{}),...(MaxDuration?{"maxDuration":MaxDuration}:{}),...(Genres?{"genres":genres}:{})
    }}
    ).then(function(res){
        setItems({data:res.data.results,
            pageCount:Math.ceil(res.data.totalElements/postersPerPage),
            totalResults:res.data.totalElements},
            setLoading(false))
    })}
    
    //SimpleSearch
    async function fetchData(arg){
        await axios({method:'post',url:`https://${window.location.host}/api/movie/search`,data:{"page":arg+1,"itemsPerPage":postersPerPage,"titleCriteria": value}})
        .then(function (res){
            setItems({data:res.data.results,
                      pageCount:Math.ceil(res.data.totalElements/postersPerPage),
                      totalResults:res.data.totalElements })
                        setLoading(false)});
                        
   } 
        useEffect(()=>{
            setCurrentPage(0);
            setLoading(true);
            switch(SearchType){
                case "Search":fetchData(0);
                            break;
                case "Genre": fetchDataByGenre(0);
                            break; 
                case "AdvancedSearchResults" : advancedSearchFetch(0);
                            break;        
                default://default             
            }
    },[value,SearchType]);
    
    function handlePageClick({selected:selectedPage}){
        setLoading(true);
        setCurrentPage(selectedPage);
        document.body.scrollTop=0;
        document.documentElement.scrollTop = 0;
        switch(SearchType){
            case "Search":fetchData(selectedPage);
                            break;
            case "Genre": fetchDataByGenre(selectedPage);
                            break;  
            case "AdvancedSearchResults" : advancedSearchFetch(selectedPage);
                            break;                   
            default://default                             
        }
    }
    
    return (
       <Col className='column-right-SearchView'>
       <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/4.7.0/css/font-awesome.min.css"></link>
       {!loading?
       <div style={{color:'white'}}>
       <p className="ResultTitle">Results for "{SearchValue}"<span className="ResultsLength">{items.totalResults} Movies</span></p>
       <Results results={items.data} />
       {items.totalResults>0 && <Paginate pageCount={items.pageCount} handlePageClick={handlePageClick} currentPage={currentPage} />}
       </div>:<div className='center-spinner'><LoadingSpinner color="#D3D3D3" loading={loading} size={20} /></div>}
       </Col>
    )
}

export default SearchView;