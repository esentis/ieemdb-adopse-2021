import React from 'react'
import {Col} from 'react-bootstrap';
import '../Styles/SearchView.css'



function SearchView(props) {
    return (
       <Col className='column-right-SearchView'>
       <div style={{color:'white'}}>
       <p>{props.name}</p>
       <p>{props.SearchValue}</p>
       </div>
       </Col>
    )
}

export default SearchView;