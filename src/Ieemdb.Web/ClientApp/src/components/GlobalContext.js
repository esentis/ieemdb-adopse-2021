import React,{createContext,useContext,useEffect,useState} from 'react'
import jwt_decode from "jwt-decode";

const GlobalContext=createContext();
const UpdateState=createContext();
const LoginState=createContext();
const UpdateLoginState=createContext();
const Role=createContext();
const UpdateRole=createContext();
const CheckLogin=createContext();

export const usePage=()=>{
    return useContext(GlobalContext);
}

export const useCheckLogin=()=>{
    return useContext(CheckLogin);
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

export const useRole=()=>{
    return useContext(Role);
}

export const useUpdateRole=()=>{
    return useContext(UpdateRole);
}
    function GlobalContextProvider(props){
        const [page,setPage]=useState("");
        const [role,setRole]=useState(localStorage.getItem('role'));
        const handleClick=(arg)=>{
            setPage(arg);
       }
       
       const [isLoggedIn,setIsLoggedIn]=useState(false);

       function getRole(){
        var decoded = jwt_decode(localStorage.getItem('token'));
        var role=Object.values(decoded)[5];
        setRole(role);
       }

        useEffect(()=>{
            if(localStorage.getItem('token')!==null){
                setIsLoggedIn(true)
                getRole();}
        },[])

        window.onstorage = () => {
            if(localStorage.getItem('token')!==null){
                setIsLoggedIn(true)
                getRole();
            }
            else{setIsLoggedIn(false);}
        }

        const CheckLoginState=()=>{
            if(localStorage.getItem('token')!==null){
              return true;
            }else{return false}
          }
           

       
            
        return(
            <GlobalContext.Provider value={page}>
            <UpdateState.Provider value={handleClick}>
            <UpdateLoginState.Provider value={setIsLoggedIn}>
            <LoginState.Provider value={isLoggedIn}>
            <Role.Provider value={role}>
            <CheckLogin.Provider value={CheckLoginState}>
            <UpdateRole.Provider value={setRole}>
                {props.children}
                </UpdateRole.Provider>
                </CheckLogin.Provider>
                </Role.Provider>
                </LoginState.Provider>
                </UpdateLoginState.Provider>
            </UpdateState.Provider>  
            </GlobalContext.Provider>
        )
    }

export default GlobalContextProvider;
