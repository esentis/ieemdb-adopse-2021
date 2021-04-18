import React,{useEffect} from 'react'
import {Col} from 'react-bootstrap';
import '../Styles/SearchView.css'
import {useUpdatePage} from './GlobalContext'



function SearchView() {
    const setPage=useUpdatePage();
    useEffect(() => {
        setPage("1")})
    return (
       <Col className='column-right-SearchView'>
       <div style={{color:'white'}}>
       <p>SearchView</p>
       </div>
       </Col>
    )
}

export default SearchView;