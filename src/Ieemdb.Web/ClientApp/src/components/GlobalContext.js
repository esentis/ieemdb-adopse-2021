import React,{createContext,useContext,useState} from 'react'

const GlobalContext=createContext();
const UpdateState=createContext();

export const usePage=()=>{
    return useContext(GlobalContext);
}


export const useUpdatePage=()=>{
    return useContext(UpdateState);
}
    function GlobalContextProvider(props){
        const [page,setPage]=useState("");
        const handleClick=(arg)=>{
            setPage(arg);
       }

        return(
            <GlobalContext.Provider value={page}>
            <UpdateState.Provider value={handleClick}>
                {props.children}
            </UpdateState.Provider>  
            </GlobalContext.Provider>
        )
    }

export default GlobalContextProvider;