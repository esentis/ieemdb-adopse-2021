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
                    poster={result.posterUrl==="https://image.tmdb.org/t/p/w600_and_h900_bestv2"?"https://media.comicbook.com/files/img/default-movie.png":result.posterUrl}
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
