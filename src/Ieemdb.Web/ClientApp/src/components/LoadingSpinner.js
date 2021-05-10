import React from 'react'
import PropagateLoader from "react-spinners/PropagateLoader";
import { css } from "@emotion/core";

function LoadingSpinner(props) {
    const override = css`
    display: block;
    margin: auto;
    border-color: "#D3D3D3";
  `;
    return (
        <PropagateLoader color={props.color} loading={props.loading} css={override} size={props.size} />
    )
}

export default LoadingSpinner
