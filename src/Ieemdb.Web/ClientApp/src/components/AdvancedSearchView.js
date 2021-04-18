import React,{useEffect} from 'react'
import {Col} from 'react-bootstrap';
import '../Styles/AdvancedSearch.css'
import {useUpdatePage} from './GlobalContext';


function AdvancedSearchView(props) {

    const setPage=useUpdatePage();
    useEffect(() => {
        setPage("1")})
    return (
       <Col className='column-right-AdvancedSearch'>
       <div style={{color:'white'}}>
       <p>Advanced Search View</p>
       </div>
       </Col>
    )
}

export default AdvancedSearchView;