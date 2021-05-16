import React,{useEffect,useState} from 'react'
import {Col} from 'react-bootstrap';
import '../Styles/UserSettings.css'
import {useUpdatePage} from './GlobalContext'
import Snackbar from '@material-ui/core/Snackbar';
import MuiAlert from '@material-ui/lab/Alert';
import axios from 'axios';
import validator from 'validator';
import PasswordChecklist from "react-password-checklist";
import getRefreshToken from './refreshToken';


function UserSettings() {

    const setPage=useUpdatePage();
    useEffect(() => {
        setPage("1")})

        const [open,setOpen]=useState(false);
        function Alert(props) {
            return <MuiAlert elevation={6} variant="filled" {...props} />;
          }

        function CloseSnackBar(){setOpen(false)};
        const [passWordChange,setPassWordChange]=useState(false);
        const [severity,setSeverity]=useState("");
        const [errorMessage,setErrorMessage]=useState("");
        
        
        const [data,setData]=useState({
            username:localStorage.getItem('username'),
            oldPassword:"",
            newPassword:"",
            confirmPassword:""
        })
        
        
        const [disableUsername,setDisableUsername]=useState(true);
        const [disablePassword,setDisablePassword]=useState(true);


        async function changeUsername(e){
            e.preventDefault();
            await axios({method:'post',url:`https://${window.location.host}/api/account/changeUsername?username=${data.username}`,
            headers:{'Authorization':'Bearer ' + localStorage.getItem('token')}}).then(function(){
                                setOpen(true)
                                setSeverity("success")
                                setErrorMessage("Saved");
                                setDisableUsername(true);
                                localStorage.setItem('username',data.username);})
            .catch(error=>{
                if(error.response.status===500){
                setOpen(true);
                setSeverity("error");
                setErrorMessage("Username has already been taken");
                
                }else{if(error.response.status===401){
                    (async()=>{
                        var token=await getRefreshToken();
                        axios({method:'post',url:`https://${window.location.host}/api/account/changeUsername?username=${data.username}`,
                        headers:{'Authorization':'Bearer ' + token}}).then(function(){
                                            setOpen(true)
                                            setSeverity("success")
                                            setErrorMessage("Saved");
                                            setDisableUsername(true);
                                            localStorage.setItem('username',data.username);})
                      })();
                }else{
                    setOpen(true);
                    setSeverity("error");
                    setErrorMessage("Something went wrong.Try again");}}}
                );
        }

        async function changePassword(e){
            e.preventDefault();
            async function updatePassword(){
            await axios({method:'post',url:`https://${window.location.host}/api/account/changePassword`,
            data:{"oldPassword":data.oldPassword,"newPassword":data.newPassword},
            headers:{'Authorization':'Bearer ' + localStorage.getItem('token')}}).then(function(res){
                    setOpen(true);
                    setSeverity("success");
                    setErrorMessage("Saved");
                        }).catch(function(error){
                            if(error.response.status===409){
                            setOpen(true);
                            setSeverity("error");
                            setErrorMessage("Old Password do not match");
                        }else{if(error.response.status===401){
                            (async()=>{
                                var token=await getRefreshToken();
                                axios({method:'post',url:`https://${window.location.host}/api/account/changePassword`,
                                data:{"oldPassword":data.oldPassword,"newPassword":data.newPassword},
                                headers:{'Authorization':'Bearer ' + token}}).then(function(res){
                                setOpen(true);
                                setSeverity("success");
                                setErrorMessage("Saved");    
                            })
                              })();
                        }}
                        })} 
                            if(data.oldPassword.length<2){
                                setOpen(true);
                                setSeverity("error");
                                setErrorMessage("You must fill in old Password");
                            }else{updatePassword();}
                            }
                        
       function onFormChange(e){
           var userName=data.username;
           var NewPassword=data.newPassword;
           var confirmPassword=data.confirmPassword;
        if(e.target.name==='username'){
            userName=e.target.value;
        }
        else if(e.target.name==='newPassword'){
            NewPassword=e.target.value;   
        } 
        else if(e.target.name==='confirmPassword'){
            confirmPassword=e.target.value;
        }

        if(NewPassword.length>0){
            setPassWordChange(true);
        }else{setPassWordChange(false)}


        if (validator.isStrongPassword(NewPassword, {
            minLength: 8, minLowercase: 1,
            minUppercase: 1, minNumbers: 1, minSymbols: 1
          })) {
              if(NewPassword!==confirmPassword){
                setDisablePassword(true);
          }else{setDisablePassword(false)}
        }else{setDisablePassword(true)}

        
        if(userName===localStorage.getItem('username')||userName.length<5){
            setDisableUsername(true);
        }else{
            setDisableUsername(false);
        }
       }

        
        
    return (
        <Col className='column-right-Login'>
        <div className='center-Form'>
        <div className="backForm-userSettings">
        <label className="formTitle">Edit your profile</label>
        <form className="divForm" onChange={onFormChange} autoComplete="off" >
        <label className="formText">Username</label>
        <input className="formInput" type="text" placeholder="Your username here" value={data.username} name="username" onChange={e=>setData({...data,username:e.target.value})} />
        <button className="userSettingsButton" disabled={disableUsername} onClick={changeUsername}>Save Username</button>
        <label className="formText">Old Password</label>
        <input className="formInput" type="password" placeholder="Old password here" value={data.oldPassword} name="oldPassword" onChange={e=>setData({...data,oldPassword:e.target.value})} />
        <label className="formText">New Password</label>
        <input className="formInput" type="password" placeholder="New password here" value={data.newPassword} name="newPassword" onChange={e=>setData({...data,newPassword:e.target.value})} />
        <label className="formText">Confirm Password</label>
        <input className="formInput" type="password" placeholder="Confirmation password here" value={data.confirmPassword} name="confirmPassword" onChange={e=>setData({...data,confirmPassword:e.target.value})} />
        <button className="userSettingsButton" disabled={disablePassword} onClick={changePassword}>Save New password</button>
      </form>
      <div className="center-SnackBar">
      <Snackbar open={open} autoHideDuration={6000} onClose={CloseSnackBar} anchorOrigin={{vertical:'bottom',horizontal:'center'}}  >
            <Alert onClose={CloseSnackBar} severity={severity}>
            {errorMessage}
        </Alert>
      </Snackbar></div>
    </div>
    {passWordChange &&
    <div className="passWordValidator">
    <PasswordChecklist
				rules={["length","specialChar","number","capital","match"]}
				minLength={8}
				value={data.newPassword}
				valueAgain={data.confirmPassword}
			/>
    </div>}
    
    
    {/* <Snackbar open={open} autoHideDuration={6000} onClose={CloseSnackBar} anchorOrigin={{vertical:'bottom',horizontal:'center'}}  >
            <Alert onClose={CloseSnackBar} severity={severity}>
            {errorMessage}
        </Alert>
      </Snackbar> */}
       
       </div>
       
       </Col>
    )
}

export default UserSettings;