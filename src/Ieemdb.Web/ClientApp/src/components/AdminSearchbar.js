import React from 'react'

function AdminSearchbar(props) {
    return (
        <>
           <div className="input-wrapper">
            <input
              placeholder="Search for a movie"
              onKeyUp={props.onKeyUp}
              onChange={props.onChange}
              value={props.value}
            ></input>
          </div> 
        </>
    )
}

export default AdminSearchbar
