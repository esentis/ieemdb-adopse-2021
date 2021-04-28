import React ,{useState} from "react";
import  * as FaIcons  from "react-icons/fa";
import {useHistory} from 'react-router-dom';




const SearchBar = () => {
    const [value,setValue]=useState("");
    const history=useHistory();
    
    function onChange(e){
      setValue(e.target.value);
    }
    function onEnter(e){
       if (e.keyCode===13){
        history.push('/Search/value='+value);
      }
    }

    

  
  return (
    <>
    <FaIcons.FaSearch className='fa-cog'/>
    <input className="inputClass"  placeholder="Search" onChange={onChange} onKeyUp={onEnter}></input>
    {/* <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/4.7.0/css/font-awesome.min.css"></link>
    <div class="form-group has-search input-group">
    <span class="fa fa-search form-control-feedback "></span>
    <input type="text" class="form-control shadow-none" placeholder="Search"></input>
    <div class="input-group-append">
    
      <button class="advbutt" type="button">
        Advanced
      </button>
    </div>
    
    
    </div> */}

  

    
    </>
  );
}
export default SearchBar;