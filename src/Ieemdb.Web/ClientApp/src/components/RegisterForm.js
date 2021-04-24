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
      }
    });
  }
  return (
    <form onSubmit={handleSubmitRegister}>
      <label>
        UserNameReg:
            <input
          type="text"
          value={userName}
          onChange={e => setUserName(e.target.value)}
        />
      </label>
      <label>
        PasswordReg:
            <input
          type="password"
          value={password}
          onChange={e => setPassword(e.target.value)}
        />
      </label>
      <label>
        Email:
          <input
          type="email"
          value={email}
          onChange={e => setEmail(e.target.value)}
        />
      </label>
      <input type="submit" value="Register" />
    </form>
  );
}

export default RegisterForm;
