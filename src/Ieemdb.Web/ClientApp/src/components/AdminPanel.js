import React,{useEffect,useState} from 'react'
import {Col} from 'react-bootstrap';
import '../Styles/AdminPanel.css';
import {useUpdatePage} from './GlobalContext';
import TopRight from './TopRight';
import MovieCard from './MovieCard';
import AdminSearchbar from './AdminSearchbar';
import Results from './Results';
import '../Styles/Paginate.css';
import axios from 'axios';
import Paginate from './Paginate';



function AdminPanel() {
    
    const setPage=useUpdatePage();
    useEffect(() => {
        setPage("1")
        async function fetchData(){
            await axios({method:'post',url:`https://${window.location.host}/api/movie/search`,data:{"page":1,"itemsPerPage":20,"isFeatured": true}})
            .then(function(res){setFeatured(res.data.results)
            setLoading(false)});}
        fetchData();
    },[setPage]);
        
        const [value,setValue]=useState();
        const [loading,setLoading]=useState(true);
        const [currentPage,setCurrentPage]=useState(0);
        const [featured,setFeatured]=useState([]);
        const [searchValue,setSearchValue]=useState("");
        const [display,setDisplay]=useState("none");
        const [items,setItems]=useState({
            data:[],
            totalResults:0,
            pageCount:0
        });

       async function removeFeatured(arg){
            const newFeatured=featured.filter((movie)=>arg!==movie.id)
            setFeatured(newFeatured);
            await axios.post(`https://${window.location.host}/api/movie/unfeature?id=${arg}`)
            
        }

        async function addFeatured(id,posterUrl,title){
            const newFeatured=[...featured,{id,posterUrl,title}]
            setFeatured(newFeatured);
            await axios.post(`https://${window.location.host}/api/movie/feature?id=${id}`)
        }

        const title=' Current Featured Movies';
        const movies=featured.map(i => <MovieCard 
        id={i.id}
        Title={i.title} 
        Poster={i.posterUrl?i.posterUrl:"https://media.comicbook.com/files/img/default-movie.png"} 
        height={"250vh"} 
        width={'auto'}
        posterClass='poster-Admin'
        flag={true}
        onClick={removeFeatured} />)

        const postersPerPage=10;
        async function fetchData(arg){
            console.log("admin value:",value);
            await axios({method:'post',url:`https://${window.location.host}/api/movie/search`,data:{"page":arg+1,"itemsPerPage":postersPerPage,"titleCriteria": value}})
            .then(function (res){
            setItems({data:res.data.results,
                  pageCount:Math.ceil(res.data.totalElements/postersPerPage),
                  totalResults:res.data.totalElements })});
} 

       async function onEnter(e){
            if (e.keyCode===13){
                setCurrentPage(0);
            if(e.target.value.length>0){
                setSearchValue(e.target.value);
                fetchData(0);            
                setDisplay("flex"); 
            }             
        }}

    

    function onChange(e){
        setValue(e.target.value);
    }   
    
    function handlePageClick({selected:selectedPage}){
        setCurrentPage(selectedPage);
        fetchData(selectedPage);
        document.body.scrollTop=0;
        document.documentElement.scrollTop = 0;
    }

    return (
        <>
        <Col className="column-right-Admin">
        <TopRight title={title}
                  items={movies}
                  loading={loading}
                  onClick={removeFeatured} />
                <AdminSearchbar onKeyUp={onEnter} onChange={onChange} value={value} />
                <div className='resultsTitles' style={{display:display}}><p className="ResultTitle">Results for "{searchValue}"<span className="ResultsLength">{items.totalResults} Movies</span></p></div>
                <div className='resultsDiv'><Results results={items.data} featured={featured} flag={true} onClick={addFeatured}/></div>
                {items.totalResults>0 &&<Paginate pageCount={items.pageCount} handlePageClick={handlePageClick} currentPage={currentPage} /> }
        </Col>
        </> 
    )
}
export default AdminPanel;
