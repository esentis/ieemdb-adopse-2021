import React,{useEffect,useState} from 'react'
import {Col} from 'react-bootstrap';
import '../Styles/UserSettings.css'
import {useUpdatePage} from './GlobalContext'
import Snackbar from '@material-ui/core/Snackbar';
import MuiAlert from '@material-ui/lab/Alert';



function UserSettings() {

    const setPage=useUpdatePage();
    useEffect(() => {
        setPage("1")})

        const [open,setOpen]=useState(false);
        function Alert(props) {
            return <MuiAlert elevation={6} variant="filled" {...props} />;
          }

        function CloseSnackBar(){setOpen(false)};
        
        const [severity,setSeverity]=useState("");
        const [errorMessage,setErrorMessage]=useState("");
        
        
        const [data,setData]=useState({
            username:localStorage.getItem('username'),
            newPassword:"",
            confirmPassword:""
        })
        
        
        const [disable,setDisable]=useState(true);


        const handleSubmitLogin = async (e) => {
            e.preventDefault();
            console.log("username:",data.username,"newPassword:",data.newPassword,"confirmPassword:",data.confirmPassword)
            if(data.newPassword!==data.confirmPassword){
                setOpen(true)
                setSeverity("error")
                setErrorMessage("Your password and confirmation password do not match");
            }
            else{
                setOpen(true)
                setSeverity("success")
                setErrorMessage("Saved");
            }
        }

       function onFormChange(e){
        setDisable(false) ; 
        var userName=data.username;
           var NewPassword=data.newPassword;
        if(e.target.name==='username'){
            userName=e.target.value;
            
        }
        else if(e.target.name==='newPassword'){
            NewPassword=e.target.value;   
        }
        

        if(userName===localStorage.getItem('username')&&NewPassword.length<5){
            setDisable(true);
        }
        

       }

        
        
    return (
        <Col className='column-right-Login'>
        <div className='center-Form'>
        <div className="backForm-userSettings">
        <label className="formTitle">Edit your profile</label>
        <form className="divForm" onSubmit={handleSubmitLogin} onChange={onFormChange} >
        <label className="formText">Username</label>
        <input className="formInput" type="text" placeholder="Your username here" value={data.username} name="username" onChange={e=>setData({...data,username:e.target.value})} />
        <label className="formText">New Password</label>
        <input className="formInput" type="password" placeholder="New password here" value={data.newPassword} name="newPassword" onChange={e=>setData({...data,newPassword:e.target.value})} />
        <label className="formText">Confirm Password</label>
        <input className="formInput" type="password" placeholder="Confirmation password here" value={data.confirmPassword} name="confirmPassword" onChange={e=>setData({...data,confirmPassword:e.target.value})} />
        <input className="formButton" type="submit" value="Save" disabled={disable} />
      </form>
         
    </div>
    <div className="center-SnackBar"><Snackbar open={open} autoHideDuration={6000} onClose={CloseSnackBar} anchorOrigin={{vertical:'bottom',horizontal:'center'}}  >
            <Alert onClose={CloseSnackBar} severity={severity}>
            {errorMessage}
        </Alert>
      </Snackbar></div>  
       </div>
       
       </Col>
    )
}

export default UserSettings;