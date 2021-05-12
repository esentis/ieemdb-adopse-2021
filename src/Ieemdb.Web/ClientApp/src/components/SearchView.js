import React,{useEffect,useState} from 'react'
import {Col} from 'react-bootstrap';
import '../Styles/SearchView.css'
import {useUpdatePage} from './GlobalContext'
import { useParams } from "react-router-dom";
import Results from './Results';
import Paginate from './Paginate';
import '../Styles/Paginate.css'
import axios from 'axios'



function SearchView() {
    var SearchValue=""
    var { value,SearchType,MovieTitle,ActorName,DirectorName,WriterName,Duration,Genres,FromRating,ToRating,FromDate,ToDate,GenreId }=useParams();
    if(value===undefined){
        value=null
    }

    if(SearchType==="AdvancedSearchResults"){
        SearchValue="AdvancedSearch"
        console.log(SearchType,value,MovieTitle,ActorName,DirectorName,WriterName,Duration,Genres,FromRating,ToRating,FromDate,ToDate);
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
    async function fetchDataByGenre(arg){
        await axios({method:'post',url:`https://${window.location.host}/api/movie/search`,data:{"page":arg+1,"itemsPerPage":postersPerPage,"genres":[GenreId]}})
        .then(function (res){
            setItems({data:res.data.results,
                      pageCount:Math.ceil(res.data.totalElements/postersPerPage),
                      totalResults:res.data.totalElements})
                        console.log(res.data)});
                        
    }
    
    async function fetchData(arg){
        await axios({method:'post',url:`https://${window.location.host}/api/movie/search`,data:{"page":arg+1,"itemsPerPage":postersPerPage,"titleCriteria": value}})
        .then(function (res){
            setItems({data:res.data.results,
                      pageCount:Math.ceil(res.data.totalElements/postersPerPage),
                      totalResults:res.data.totalElements })
                        console.log(res.data)});
                        
   } 
        useEffect(()=>{
            setCurrentPage(0);
            switch(SearchType){
                case "Search":fetchData(0);
                            break;
                case "Genre": fetchDataByGenre(0);
                            break;  
                default://default             
            }
    },[value,SearchType]);
    
    function handlePageClick({selected:selectedPage}){
        setCurrentPage(selectedPage);
        document.body.scrollTop=0;
        document.documentElement.scrollTop = 0;
        switch(SearchType){
            case "Search":fetchData(selectedPage);
                            break;
            case "Genre": fetchDataByGenre(selectedPage);
                            break;  
            default://default                             
        }
    }
    
    return (
       <Col className='column-right-SearchView'>
       <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/4.7.0/css/font-awesome.min.css"></link>
       <div style={{color:'white'}}>
       <p className="ResultTitle">Results for "{SearchValue}"<span className="ResultsLength">{items.totalResults} Movies</span></p>
       <Results results={items.data} />
       {items.totalResults>0 && <Paginate pageCount={items.pageCount} handlePageClick={handlePageClick} currentPage={currentPage} />}
       </div>
       </Col>
    )
}

export default SearchView;