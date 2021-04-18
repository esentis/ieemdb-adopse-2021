import React from 'react'
import TopRight from './TopRight'
import MovieCard from './MovieCard';
import movies from './Movie_Dataset';

function Featured() {
    const title='FEATURED';
    const items=movies.map(i => <MovieCard 
        id={i.id}
        Title={i.title} 
        Poster={i.poster} 
        Overview={i.overview}
        ReleaseDate={i.release_date}
        Genres={i.genres}
        Actors={i.actors}
        Writers={i.writers}
        Directors={i.directors}
        Rating={i.rating}
        Duration={i.duration}
        CountryOrigin={i.countryOrigin}
        height={"250vh"} 
        width={'auto'} />)
    return (
        <TopRight title={title}
                  items={items} />
    )
}

export default Featured
