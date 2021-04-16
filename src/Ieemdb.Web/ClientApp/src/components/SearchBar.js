import React ,{useState} from "react";
import  * as FaIcons  from "react-icons/fa";
import {useUpdatePage} from './Navigate'
const SearchBar = () => {
    const [value,setValue]=useState("");
    const setPage=useUpdatePage();
    function onChange(e){
      setValue(e.target.value);
    }
    function onEnter(e){
      if (e.keyCode===13){
        setPage({name:"SearchView",value:value})
      }
    }
  return (
    <>
    <FaIcons.FaSearch className='fa-cog'/>
    <input className="inputClass"  placeholder="Search" onChange={onChange} onKeyUp={onEnter}></input>
    </>
  );
}
export default SearchBar;