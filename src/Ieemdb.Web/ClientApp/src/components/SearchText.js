import React from "react";
import {InputGroup,FormControl} from 'react-bootstrap';

function SearchText(){
    return(
    <InputGroup size="sm" className="mb-3">
        <InputGroup.Prepend>
            <InputGroup.Text id="inputGroup-sizing-sm">Small</InputGroup.Text>
        </InputGroup.Prepend>
        <FormControl aria-label="Small" aria-describedby="inputGroup-sizing-sm" />
    </InputGroup>
    );
}

export default SearchText;