import React from 'react';
import '../Styles/SearchView.css';
import ResultCard from "./ResultCard";

function Results({results,flag,onClick,disabled,featured}) {
    return (
        <ul className="ListStyle">
            {results.map(result=>(
                <li key={result.id}><ResultCard
                    id={result.id}
                    title={result.title}
                    poster={result.posterUrl?result.posterUrl:"https://media.comicbook.com/files/img/default-movie.png"}
                    release_date={result.releaseDate}
                    overview={result.plot}
                    flag={flag}
                    onClick={onClick}
                    disabled={disabled}
                    featured={featured}
                    /></li>
            ))}
        </ul>
    )
}

export default Results;
