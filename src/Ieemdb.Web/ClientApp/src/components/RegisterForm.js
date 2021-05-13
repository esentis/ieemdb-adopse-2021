import React, { useState } from 'react';
import axios from 'axios';
import '../Styles/Forms.css';
import validator from 'validator';
import PasswordChecklist from "react-password-checklist";
import emailValidator from 'email-validator'
import Snackbar from '@material-ui/core/Snackbar';
import MuiAlert from '@material-ui/lab/Alert';
function RegisterForm() {
  const [userName, setUserName] = useState("");
  const [password, setPassword] = useState("");
  const [email, setEmail] = useState("");
  const [confirmPassword,setConfirmPassword]=useState("");
  const [disableButton,setDisableButton]=useState(true);

  const [open,setOpen]=useState(false);
  const [errorMessage,setErrorMessage]=useState("");
        function Alert(props) {
            return <MuiAlert elevation={6} variant="filled" {...props} />;
          }

        function CloseSnackBar(){setOpen(false)};
  
  const handleSubmitRegister = async (evt) => {
    evt.preventDefault();
    const url = 'https://' + window.location.host + '/api/account/';
    if(userName.length<5){
        setOpen(true);
        setErrorMessage("Username must be at least 5 characters");
    }else if(!emailValidator.validate(email)){
      setOpen(true);
      setErrorMessage("Email is not valid");
    }
    else{
    axios.post(url, {
      userName,
      email,
      password
    }).then(function (res) {
      if (res.status === 200) {
        //do some
        window.alert("Check your email inbox for confirmation");
      }
    }).catch(err=>{
      console.log(err.response.status);
      if(err.response.status===409){
        setOpen(true);
        setErrorMessage("Username has already been taken");
      }else{setOpen(true);
        setErrorMessage("Something went wrong.Try again!");}
    });
  }}


  function onFormChange(e){
    var passWord=password;
    var ConfirmPassword=confirmPassword;

    if(e.target.name==="password"){
      passWord=e.target.value;
    }else if(e.target.name==="confirmPassword"){
      ConfirmPassword=e.target.value;
    }
    
    if (validator.isStrongPassword(password, {
      minLength: 8, minLowercase: 1,
      minUppercase: 1, minNumbers: 1, minSymbols: 1
    })) {
      
      if(passWord!==ConfirmPassword){
        setDisableButton(true);
  }else{setDisableButton(false)}
}else{setDisableButton(true)}
    
  }
  return (
    <>
    <div className="backForm">
      <label className="formTitle">Create a new account</label>
      <form className="divForm" onSubmit={handleSubmitRegister} onChange={onFormChange} autocomplete="off">
        <label className="formText">Username</label>
        <input className="formInput" type="text" name="username" placeholder="Username" value={userName} onChange={e => setUserName(e.target.value)}/>
        <label className="formText">Email</label>
        <input className="formInput"  name="email" placeholder="Email" value={email} onChange={e => setEmail(e.target.value)}/>
        <label className="formText">Password</label>
        <input className="formInput" type="password" name="password" placeholder="Password" value={password} onChange={e => setPassword(e.target.value)}/>
        <label className="formText">Confirm password</label>
        <input className="formInput" type="password" name="confirmPassword" placeholder="Confirm password" value={confirmPassword} onChange={e => setConfirmPassword(e.target.value)}/>
        <input className="formButton" type="submit" value="Register" disabled={disableButton} />
      </form>
      <div className="center-SnackBar">
      <Snackbar open={open} autoHideDuration={6000} onClose={CloseSnackBar} anchorOrigin={{vertical:'bottom',horizontal:'center'}}  >
            <Alert onClose={CloseSnackBar} severity={"error"}>
            {errorMessage}
        </Alert>
      </Snackbar></div>
    </div>
    <div className="passWordValidator">
    <PasswordChecklist
				rules={["length","specialChar","number","capital","match"]}
				minLength={8}
				value={password}
				valueAgain={confirmPassword}
			/>
    </div>
    </>
  );
}
export default RegisterForm;