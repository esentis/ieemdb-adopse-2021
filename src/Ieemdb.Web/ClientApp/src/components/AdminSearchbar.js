import React,{useState} from 'react'

function AdminSearchbar(props) {

    const [searchValue,setSearchValue]=useState("");

    function onInputChange(e){
            e.preventDefault();
            setSearchValue(e.target.value);
        }
    return (
        <>
           <div className="input-wrapper">
            <input
              placeholder="Search for a movie"
              onKeyUp={props.onKeyUp}
              onChange={onInputChange}
              value={searchValue}
            ></input>
          </div> 
        </>
    )
}

export default AdminSearchbar
