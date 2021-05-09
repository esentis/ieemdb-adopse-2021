import React,{useEffect,useState} from 'react'
import {Col} from 'react-bootstrap';
import '../Styles/SearchView.css'
import {useUpdatePage} from './GlobalContext'
import movies from './Movie_Dataset';
import { useParams } from "react-router-dom";
import Results from './Results';
import Paginate from 'react-paginate';
import '../Styles/Paginate.css'
import axios from 'axios'



function SearchView() {
    var SearchValue=""
    var { value,SearchType,MovieTitle,ActorName,DirectorName,WriterName,Duration,Genres,FromRating,ToRating,FromDate,ToDate }=useParams();
    if(value===undefined){
        value=null
    }
    
    if(SearchType==="AdvancedSearchResults"){
        SearchValue="AdvancedSearch"

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


    async function fetchData(arg){
        await axios({method:'post',url:`https://${window.location.host}/api/movie/search`,data:{"page":arg+1,"itemsPerPage":1,"titleCriteria": value}})
        .then(function (res){
            setItems({data:res.data.results,
                      pageCount:res.data.totalPages-1,
                      totalResults:res.data.totalElements })
                        console.log(res.data)});
   } 

        

        useEffect(()=>{
            setCurrentPage(0);
            // console.log(SearchType,value,MovieTitle,ActorName,DirectorName,WriterName,Duration,Genres,FromRating,ToRating,FromDate,ToDate);
            if(SearchType==="Search"){
                fetchData(0);
        }},[value,SearchType]);

       

    function handlePageClick({selected:selectedPage}){
        setCurrentPage(selectedPage);
        document.body.scrollTop=0;
        document.documentElement.scrollTop = 0;
        fetchData(selectedPage);
    }
    
    return (
       <Col className='column-right-SearchView'>
       <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/4.7.0/css/font-awesome.min.css"></link>
       <div style={{color:'white'}}>
       <p className="ResultTitle">Results for "{SearchValue}"<span className="ResultsLength">{items.totalResults} Movies</span></p>
       <Results results={items.data} />
       {movies.length>10 && <Paginate previousLabel={<i className="fa fa-chevron-left"></i>}
                  nextLabel={<i className="fa fa-chevron-right"></i>}
                  breakLabel={".."}
                  pageCount={items.pageCount}
                  marginPagesDisplayed={1}
                  forcePage={currentPage}
                  pageRangeDisplayed={2}
                  onPageChange={handlePageClick}
                  containerClassName={"pagination"}
                  subContainerClassName={"pages pagination"}
                  activeClassName={"active"}/> }
       </div>
       </Col>
    )
}

export default SearchView;