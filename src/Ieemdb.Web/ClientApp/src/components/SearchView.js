import React,{useEffect,useState} from 'react'
import {Col} from 'react-bootstrap';
import '../Styles/SearchView.css'
import {useUpdatePage} from './GlobalContext'
import movies from './Movie_Dataset';
import { useParams } from "react-router-dom";
import Results from './Results';
import Paginate from 'react-paginate';
import '../Styles/Paginate.css'



function SearchView() {
    var SearchValue=""
    var { value,SearchType,MovieTitle,ActorName,DirectorName,WriterName,Duration,Genres,FromRating,ToRating,FromDate,ToDate }=useParams();
    if(value==undefined){
        value=null
    }
    console.log(SearchType,value,MovieTitle,ActorName,DirectorName,WriterName,Duration,Genres,FromRating,ToRating,FromDate,ToDate)
    if(SearchType=="AdvancedSearchResults"){
        SearchValue="AdvancedSearch"

    }else{SearchValue=value}
    

    const [currentPage,setCurrentPage]=useState(0);
    const [postersPerPage,setPostersPerPage]=useState(10);

    const setPage=useUpdatePage();
    useEffect(() => {
        setPage("1")})


    const indexOfLastPoster=currentPage * postersPerPage;
    const currentPosters=movies.slice(indexOfLastPoster,indexOfLastPoster+postersPerPage);
    const pageCount = Math.ceil(movies.length / postersPerPage);

    function handlePageClick({selected:selectedPage}){
        setCurrentPage(selectedPage);
        document.body.scrollTop=0;
        document.documentElement.scrollTop = 0;
    }
    
    return (
       <Col className='column-right-SearchView'>
       <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/4.7.0/css/font-awesome.min.css"></link>
       <div style={{color:'white'}}>
       <p className="ResultTitle">Results for "{SearchValue}"<span className="ResultsLength">{movies.length} Movies</span></p>
       <Results results={currentPosters} />
       <Paginate previousLabel={<i className="fa fa-chevron-left"></i>}
                  nextLabel={<i className="fa fa-chevron-right"></i>}
                  breakLabel={".."}
                  pageCount={pageCount}
                  marginPagesDisplayed={1}
                  pageRangeDisplayed={2}
                  onPageChange={handlePageClick}
                  containerClassName={"pagination"}
                  subContainerClassName={"pages pagination"}
                  activeClassName={"active"}/>  
       </div>
       </Col>
    )
}

export default SearchView;