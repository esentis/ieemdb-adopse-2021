import React from 'react';
import '../Styles/SearchView.css';
import ResultCard from "./ResultCard";

function Results({results}) {
    return (
        <ul className="ListStyle">
            {results.map(result=>(
                <li key={result.id}><ResultCard
                    id={result.id}
                    title={result.title}
                    poster={result.poster}
                    release_date={result.release_date}
                    overview={result.overview}/></li>
            ))}
        </ul>
    )
}

export default Results;
