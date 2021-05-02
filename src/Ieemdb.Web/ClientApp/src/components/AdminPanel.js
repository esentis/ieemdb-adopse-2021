import React,{useEffect,useState} from 'react'
import {Col} from 'react-bootstrap';
import '../Styles/AdminPanel.css';
import {useUpdatePage} from './GlobalContext';
import TopRight from './TopRight';
import MovieCard from './MovieCard';
import movies from './Movie_Dataset';
import AdminSearchbar from './AdminSearchbar';
import Results from './Results';
import Paginate from 'react-paginate';
import '../Styles/Paginate.css'


function AdminPanel() {
    
    const setPage=useUpdatePage();
    useEffect(() => {
        setPage("1")
        setFeatured(movies)},[setPage]);
        
        const [featured,setFeatured]=useState([]);
        const [unFeatured,setUnFeatured]=useState([]);
        const [searchValue,setSearchValue]=useState("");
        const [results,setResults]=useState([]);
        const [display,setDisplay]=useState("none");

        const removeFeatured=(arg)=>{
            const newFeatured=featured.filter((movie)=>arg!==movie.id)
            setFeatured(newFeatured);
            setUnFeatured([...unFeatured,arg]);
        }

        const addFeatured=(id,poster,title)=>{
            const newFeatured=[...featured,{id,poster,title}]
            setFeatured(newFeatured);
            const newUnFeatured=unFeatured.filter((movie)=>id!==movie);
            setUnFeatured(newUnFeatured);
        }

        function saveClick(){
            console.log("unfeatured:",unFeatured);
            const newFeatured=featured.map(x=>x.id);
            console.log("newFeatured:",newFeatured);
        }

        const title=' Current Featured Movies';
        const items=featured.map(i => <MovieCard 
        id={i.id}
        Title={i.title} 
        Poster={i.poster}
        height={"250vh"} 
        width={'auto'}
        posterClass='poster-Admin'
        flag={true}
        onClick={removeFeatured} />)

        function onEnter(e){
            const searchResults=[];  
            if (e.keyCode===13){
            if(e.target.value.length>0){
                movies.map(movie=>searchResults.push(movie));
                setSearchValue(e.target.value);
                setResults(searchResults);
                setDisplay("flex"); 
            }             
        }}

    const [currentPage,setCurrentPage]=useState(0);
    const [postersPerPage]=useState(10);

    const indexOfLastPoster=currentPage * postersPerPage;
    const currentPosters=results.slice(indexOfLastPoster,indexOfLastPoster+postersPerPage);
    const pageCount = Math.ceil(results.length / postersPerPage);

    function handlePageClick({selected:selectedPage}){
        setCurrentPage(selectedPage);
        document.body.scrollTop=0;
        document.documentElement.scrollTop = 0;
    }

    return (
        <>
        <Col className="column-right-Admin">
        <TopRight title={title}
                  items={items}
                  ColClassName={""} />
                <div className='DivButton'><button className='buttonSave' onClick={saveClick}>Save Changes</button></div>
                <AdminSearchbar onKeyUp={onEnter} />
                <div className='resultsTitles' style={{display:display}}><p className="ResultTitle">Results for "{searchValue}"<span className="ResultsLength">{movies.length} Movies</span></p></div>
                <div className='resultsDiv'><Results results={currentPosters} featured={featured} flag={true} onClick={addFeatured}/></div>
                {results.length>10 && <Paginate previousLabel={<i className="fa fa-chevron-left"></i>}
                  nextLabel={<i className="fa fa-chevron-right"></i>}
                  breakLabel={".."}
                  pageCount={pageCount}
                  marginPagesDisplayed={1}
                  pageRangeDisplayed={2}
                  onPageChange={handlePageClick}
                  containerClassName={"pagination"}
                  subContainerClassName={"pages pagination"}
                  activeClassName={"active"}/>  }
        </Col>
        </> 
    )
}
export default AdminPanel;
