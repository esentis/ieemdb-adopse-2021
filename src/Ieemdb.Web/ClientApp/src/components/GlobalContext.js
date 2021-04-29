import React,{createContext,useContext,useEffect,useState} from 'react'

const GlobalContext=createContext();
const UpdateState=createContext();
const LoginState=createContext();
const UpdateLoginState=createContext();

export const usePage=()=>{
    return useContext(GlobalContext);
}

export const useChangeLoginState=()=>{
    return useContext(UpdateLoginState)
}

export const useLoginState=()=>{
    return useContext(LoginState)
}

export const useUpdatePage=()=>{
    return useContext(UpdateState);
}
    function GlobalContextProvider(props){
        const [page,setPage]=useState("");
        const handleClick=(arg)=>{
            setPage(arg);
       }
       
       const [isLoggedIn,setIsLoggedIn]=useState(false);

        useEffect(()=>{
            if(localStorage.getItem('Data')!==null){
                setIsLoggedIn(true)}
        })
           

       
            
        return(
            <GlobalContext.Provider value={page}>
            <UpdateState.Provider value={handleClick}>
            <UpdateLoginState.Provider value={setIsLoggedIn}>
            <LoginState.Provider value={isLoggedIn}>
                {props.children}
                </LoginState.Provider>
                </UpdateLoginState.Provider>
            </UpdateState.Provider>  
            </GlobalContext.Provider>
        )
    }

export default GlobalContextProvider;