import React, { useState } from "react";
import {Link,useNavigate} from 'react-router-dom'
import axios from "axios";
import {BiCodeBlock}from "react-icons/bi";
import { Icons } from "react-toastify";


export const Login = (props) => {
    const [email, setEmail] = useState('');
    const [pass, setPass] = useState('');
    const navigate = useNavigate();

    const handleSubmit = async(e) => {
        e.preventDefault();

        try{
            const response= await axios.post
            ("http://localhost:5149/api/User/authenticate",
            {
                email:email,
                password:pass,
            },
            {
                headers: {
                  "Content-Type": "application/json", 
                },
              }
            
            
            );
            console.log("Authentication succesful:", response.data);
            sessionStorage.setItem("token", response.data.token);
            sessionStorage.setItem("firstName", response.data.firstName);
            sessionStorage.setItem("lastName", response.data.lastName);
            sessionStorage.setItem("idUserProfile", response.data.idUserProfile);
            sessionStorage.setItem("idUser", response.data.iduser);
            sessionStorage.setItem("email", response.data.email);
            sessionStorage.setItem("projectDescription", response.data.projectDescription);
            sessionStorage.setItem("technology", response.data.technology);


            navigate("/mainpage");
        }catch(error){
            console.error("Error", error);
        }
   
       
    }
    

    return (     
        <div className="main">
       <div className="auth-form-container">
       <div className="icon-container">
          <BiCodeBlock size={70} />
        </div>
            <h2>Login</h2>                   
            <form className="login-form" onSubmit={handleSubmit}>
                <label className="label" htmlFor="email">E-mail</label>
                <input className="input" value={email} onChange={(e) => setEmail(e.target.value)}type="email" placeholder="email@mail.com" id="email" name="email" required/>
                <label className="label" htmlFor="password">Password</label>
                <input className="input" value={pass} onChange={(e) => setPass(e.target.value)} type="password" placeholder="********" id="password" name="password" required />
                <button className="button" type="submit">Log In</button>
            </form>
            <Link to="/register" className="link">Don't have an account? Register here.</Link>
        </div>
        </div>
  
    )
}