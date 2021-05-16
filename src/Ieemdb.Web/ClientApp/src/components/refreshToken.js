import axios from 'axios';
async function getRefreshToken(){
        var token;
         await axios({method:'post',url:`https://${window.location.host}/api/account/refresh`,data:{"expiredToken":localStorage.getItem('token'),"refreshToken":localStorage.getItem('refreshToken')}}).then(res=>{
           localStorage.setItem('token',res.data.accessToken);
           localStorage.setItem('refreshToken',res.data.refreshToken);
          token=res.data.accessToken
         });
         return token;
} export default getRefreshToken;