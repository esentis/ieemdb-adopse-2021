import React, { useState } from 'react';
import axios from 'axios';
import '../Styles/Forms.css';
import Snackbar from '@material-ui/core/Snackbar';
import MuiAlert from '@material-ui/lab/Alert';
/*import auth from './auth';*/
import { useHistory } from 'react-router-dom';
import jwt_decode from "jwt-decode";
import {/*useLoginState,*/ useChangeLoginState,useUpdateRole} from './GlobalContext';
function LoginForm() {
  const [userNameLogin, setUserNameLogin] = useState("");
  const [passwordLogin, setPasswordLogin] = useState("");
  const [disableButton,setDisableButton]=useState(true);
  /*const [deviceName, setDeviceName] = useState("");*/
  const history = useHistory();
  const [open,setOpen]=useState(false);
        function Alert(props) {
            return <MuiAlert elevation={6} variant="filled" {...props} />;
          }
        
    function CloseSnackBar(){setOpen(false)};    
    const isLoggedIn = useChangeLoginState();
    const updateRole=useUpdateRole();
    const handleSubmitLogin = async (evt) => {
    evt.preventDefault();
    const url = 'https://' + window.location.host + '/api/account/login';
    axios.post(url, {
      userName: userNameLogin,
      password: passwordLogin,
      deviceName: 'fdsfadsfas' //na perasw deviceName
    }).then(function (res) {
      //console.log(res.data.accessToken);
      //console.log(res.status);
      if (res.status === 200) {
        localStorage.setItem('token', res.data.accessToken);
        localStorage.setItem('username',userNameLogin);
        isLoggedIn(true);
        history.push("/");
        var decoded = jwt_decode(res.data.accessToken);
        var role=Object.values(decoded)[5];
        updateRole(role);
      }
    })
    .catch(error=>{
      setOpen(true);
    });
  }

 function onFormChange(e){
    var userName=userNameLogin;
    var passWord=passwordLogin;
    if(e.target.name==="username"){
        userName=e.target.value;
    }else if(e.target.name==="password"){
        passWord=e.target.value;
    } 

    if(userName.length<5||passWord.length<5){
      setDisableButton(true)
    }else{setDisableButton(false)}
  }


  return (
    <div className="backForm">
      <label className="formTitle">Login to your account</label>
      <form className="divForm" onSubmit={handleSubmitLogin} onChange={onFormChange} autoComplete="off">
        <label className="formText">Username</label>
        <input className="formInput" type="text" name="username" placeholder="Your username here" value={userNameLogin} onChange={e => setUserNameLogin(e.target.value)}/>
        <label className="formText">Password</label>
        <input className="formInput" type="password" name="password" placeholder="Your password here" value={passwordLogin} onChange={e => setPasswordLogin(e.target.value)}/>
        <input className="formButton" type="submit" value="Login" disabled={disableButton} />
      </form>
      <div className="snackbar"><Snackbar open={open} autoHideDuration={6000} onClose={CloseSnackBar} anchorOrigin={{vertical:'bottom',horizontal:'center'}}  >
            <Alert onClose={CloseSnackBar} severity={"error"}>
            Wrong username/password
        </Alert>
      </Snackbar>  </div>
    </div>
  );
}
export default LoginForm;