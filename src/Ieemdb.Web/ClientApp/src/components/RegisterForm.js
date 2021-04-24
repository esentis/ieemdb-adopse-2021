import React, { useEffect } from 'react';
import { useState } from 'react';
import axios from 'axios';

function RegisterForm() {
  const [userName, setUserName] = useState("");
  const [password, setPassword] = useState("");
  const [email, setEmail] = useState("");

  const handleSubmitRegister = async (evt) => {
    evt.preventDefault();

    axios.post('https://localhost:5001/api/account/', {
      userName,
      email,
      password
    }).then(function (res) {
      console.log(res);
      console.log(res.data.accessToken);
      console.log(res.status);
      if (res.status == 200) {
        //do some
        window.alert("Check your email inbox for confirmation");
      }
    });
  }
  return (
    <form className="centeredFields" onSubmit={handleSubmitRegister}>
      <label className="centeredText">
        UserNameReg:
      </label>
        <input
          type="text"
          value={userName}
          onChange={e => setUserName(e.target.value)}
        />
      <label className="centeredText">
        PasswordReg:
      </label>
            <input
          type="password"
          value={password}
          onChange={e => setPassword(e.target.value)}
        />
      <label className="centeredText">
        Email:
      </label>
      <input
          type="email"
          value={email}
          onChange={e => setEmail(e.target.value)}
        />
      <p className="buttonAlign"><input type="submit" value="Register" /></p>
    </form>
  );
}

export default RegisterForm;
