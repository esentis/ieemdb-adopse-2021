import React from 'react'
import {Col} from 'react-bootstrap';
import '../Styles/AdvancedSearch.css'


function AdvancedSearchView(props) {
    return (
       <Col className='column-right-AdvancedSearch'>
       <div style={{color:'white'}}>
       <p>{props.name}</p>
       </div>
       </Col>
    )
}

export default AdvancedSearchView;