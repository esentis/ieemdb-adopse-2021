import React,{createContext,useContext,useState} from 'react'

const NavigateContext=createContext();
const UpdateState=createContext();

export const usePage=()=>{
    return useContext(NavigateContext);
}


export const useUpdatePage=()=>{
    return useContext(UpdateState);
}
    function NavigateContextProvider(props){
        
        const [page,setPage]=useState({});
         
        
        const handleClick=(arg)=>{
            setPage(arg);
       }

        return(
            <NavigateContext.Provider value={page}>
            <UpdateState.Provider value={handleClick}>
                {props.children}
            </UpdateState.Provider>  
            </NavigateContext.Provider>
        )
    }

export default NavigateContextProvider;