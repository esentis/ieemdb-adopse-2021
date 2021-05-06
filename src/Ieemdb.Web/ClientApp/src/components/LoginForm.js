import React, { useState } from 'react';
import axios from 'axios';
import '../Styles/Forms.css';
import Snackbar from '@material-ui/core/Snackbar';
import MuiAlert from '@material-ui/lab/Alert';
/*import auth from './auth';*/
import { useHistory } from 'react-router-dom';
import {/*useLoginState,*/ useChangeLoginState } from './GlobalContext';
function LoginForm() {
  const [userNameLogin, setUserNameLogin] = useState("");
  const [passwordLogin, setPasswordLogin] = useState("")
  /*const [deviceName, setDeviceName] = useState("");*/
  const history = useHistory();
  const [open,setOpen]=useState(false);
        function Alert(props) {
            return <MuiAlert elevation={6} variant="filled" {...props} />;
          }
        
    function CloseSnackBar(){setOpen(false)};    
    const isLoggedIn = useChangeLoginState();
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
      }
    })
    .catch(error=>{
      setOpen(true);
    });
  }
  return (
    <div className="backForm">
      <label className="formTitle">Login to your account</label>
      <form className="divForm" onSubmit={handleSubmitLogin}>
        <label className="formText">Username</label>
        <input className="formInput" type="text" placeholder="Your username here" value={userNameLogin} onChange={e => setUserNameLogin(e.target.value)}/>
        <label className="formText">Password</label>
        <input className="formInput" type="password" placeholder="Your password here" value={passwordLogin} onChange={e => setPasswordLogin(e.target.value)}/>
        <input className="formButton" type="submit" value="Login" />
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