import React from 'react'
import Paginate from 'react-paginate';
import '../Styles/Paginate.css';

export default function myPaginate(props) {
    return (
        <>
        <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/4.7.0/css/font-awesome.min.css"></link>
        <Paginate previousLabel={<i className="fa fa-chevron-left"></i>}
                  nextLabel={<i className="fa fa-chevron-right"></i>}
                  breakLabel={".."}
                  pageCount={props.pageCount}
                  marginPagesDisplayed={1}
                  forcePage={props.currentPage}
                  pageRangeDisplayed={2}
                  onPageChange={props.handlePageClick}
                  containerClassName={"pagination"}
                  subContainerClassName={"pages pagination"}
                  activeClassName={"active"}/>
                  </>
    )
}

