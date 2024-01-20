import React, { useState } from "react";
import { Link, useNavigate } from "react-router-dom";
import axios from "axios";
import { toast } from 'react-toastify';
import 'react-toastify/dist/ReactToastify.css';

export const Register = (props) => {
  const [email, setEmail] = useState('');
  const [password, setPassword] = useState('');
  const [firstName, setFirstName] = useState('');
  const [lastName, setLastName] = useState('');
  const [projectDescription, setDescription] = useState('');
  const [technology, setTechnology] = useState('');
  const navigate = useNavigate();
  const [error, setError] = useState(null);

  const handleSubmit = async (e) => {
    e.preventDefault();

    try {
      const response = await axios.post(
        "http://localhost:5149/api/User/register",
        {
          firstName: firstName,
          lastName: lastName,
          email: email,
          password: password,
          technology: technology,
          projectDescription: projectDescription,
        },
        {
          headers: {
            "Content-Type": "application/json",
          },
        }
      );

     /* console.log("Registration successful:", response.data);*/
      notify();
      navigate("/login");

  
    } catch (error) {
      console.error("Error", error);
      setError("Registration failed. Please try again.");
    }
  };
  const notify=()=>{
    toast.success("Registration succesful", {
      position: "top-center",
      autoClose: 1000,
      hideProgressBar: false,
      closeOnClick: true,
      pauseOnHover: true,
      draggable: true,
      progress: undefined,
      theme: "colored",
      });
  }
    return (
        <div className="main">
        <div className="auth-form-container">
        <h2>Register</h2>
        <form className="register-form" onSubmit={handleSubmit}>
            <label htmlFor="firstName">First name</label>
            <input value={firstName} onChange={(e)=>setFirstName(e.target.value)} type="firstName" placeholder="firstName" id="firstName" name="firstName" required />
            <label htmlFor="lastName">Last name</label>
            <input value={lastName} onChange={(e)=>setLastName(e.target.value)}  type="lastName" placeholder="lastName" id="lastName" name="lastName" required />    
            <label htmlFor="email">E-mail</label>
            <input value={email} onChange={(e)=>setEmail(e.target.value)}  type="email" placeholder="email@mail.com" id="email" name="email" required />  
            <label htmlFor="password">Password</label>
            <input value={password} onChange={(e)=>setPassword(e.target.value)} type="password" placeholder="******" id="password" name="password" required />   
            <label htmlFor="technology">Technology</label>
            <input value={technology} onChange={(e)=>setTechnology(e.target.value)} type="technology" placeholder="technology" id="technology" name="technology" required /> 
            <label htmlFor="description">Description</label>
            <input value={projectDescription} onChange={(e)=>setDescription(e.target.value)}  type="projectDescription" placeholder="projectDescription" id="projectDescription" name="projectDescription" required /> 
            {error && <p className="error-message">{error}</p>}                           
            <button type="submit" onClick={handleSubmit}> Register</button>
         
        </form>
        <Link to="/login" className="link">Have an account? Log in here.</Link>
       </div>
          
       </div>
    )
    
}