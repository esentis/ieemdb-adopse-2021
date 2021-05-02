import React, { useState } from 'react';
import axios from 'axios';
import '../Styles/Forms.css';
function RegisterForm() {
  const [userName, setUserName] = useState("");
  const [password, setPassword] = useState("");
  const [email, setEmail] = useState("");
  const handleSubmitRegister = async (evt) => {
    evt.preventDefault();
    const url = 'https://' + window.location.host + '/api/account/';
    axios.post(url, {
      userName,
      email,
      password
    }).then(function (res) {
      console.log(res);
      console.log(res.status);
      if (res.status === 200) {
        //do some
        window.alert("Check your email inbox for confirmation");
      }
    });
  }
  return (
    <div className="backForm">
      <label className="formTitle">Create a new account</label>
      <form className="divForm" onSubmit={handleSubmitRegister}>
        <label className="formText">Username</label>
        <input className="formInput" type="text" placeholder="Username" value={userName} onChange={e => setUserName(e.target.value)}/>
        <label className="formText">Email</label>
        <input className="formInput" type="email" placeholder="Email" value={email} onChange={e => setEmail(e.target.value)}/>
        <label className="formText">Password</label>
        <input className="formInput" type="password" placeholder="Password" value={password} onChange={e => setPassword(e.target.value)}/>
        <input className="formButton" type="submit" value="Register" />
      </form>
    </div>
  );
}
export default RegisterForm;