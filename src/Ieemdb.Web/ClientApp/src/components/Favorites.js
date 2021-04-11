import React from 'react'
import TopRight from './TopRight'
import MovieCard from './MovieCard';
import movies from './Movie_Dataset';

function Favorites() {
    const items = movies.map(i => <div> {MovieCard(i.id, i.title, i.poster, "250vh", 'auto',true)}</div>);
    const title='FAVORITES';
    
    return (
        <>
        <TopRight
         title={title}
         items={items}    
         />
        </>
    )
}

export default Favorites;
