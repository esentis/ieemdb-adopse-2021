import React from "react";
import  * as FaIcons  from "react-icons/fa";


const SearchBar = () => {  
  return (
    <>
    <FaIcons.FaSearch className='fa-cog'/>
    <input className="inputClass"  placeholder="Search"></input>
    </>
  );
}
export default SearchBar;