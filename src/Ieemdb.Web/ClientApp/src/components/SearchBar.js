import React ,{useState} from "react";
import  * as FaIcons  from "react-icons/fa";
import {useUpdatePage} from './Navigate'
<<<<<<< HEAD
=======
import {useHistory} from 'react-router-dom';




>>>>>>> 43c3444f2315957844f44fd247f6465180b3cf63
const SearchBar = () => {
    const [value,setValue]=useState("");
    const setPage=useUpdatePage();
<<<<<<< HEAD
=======
    const history=useHistory();
    
>>>>>>> 43c3444f2315957844f44fd247f6465180b3cf63
    function onChange(e){
      setValue(e.target.value);
    }
    function onEnter(e){
<<<<<<< HEAD
      if (e.keyCode===13){
=======
       if (e.keyCode===13){
>>>>>>> 43c3444f2315957844f44fd247f6465180b3cf63
        setPage({name:"SearchView",value:value})
        history.push('/Search')
        
      }
    }
<<<<<<< HEAD
=======

    

  
>>>>>>> 43c3444f2315957844f44fd247f6465180b3cf63
  return (
    <>
    <FaIcons.FaSearch className='fa-cog'/>
    <input className="inputClass"  placeholder="Search" onChange={onChange} onKeyUp={onEnter}></input>
    </>
  );
}
export default SearchBar;