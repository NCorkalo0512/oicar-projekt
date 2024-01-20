import React, {useState, useEffect} from "react";
import { Link,useNavigate } from "react-router-dom";
import small from "./icons/small .png"
import axios from "axios";


const Profile=(props)=>{
    const [email, setEmail] = useState('');
    const [firstName, setFirstName] = useState('');
    const [lastName, setLastName] = useState('');
    const [projectDescription, setDescription] = useState('');
    const [technology, setTechnology] = useState('');
    const[userId,setIdUser]=useState("");
    const navigate = useNavigate();

useEffect(() => {
        fetchProfile();
      }, []);
    
      const fetchProfile = async () => {
        setEmail(sessionStorage.getItem("email"));
        setFirstName(sessionStorage.getItem("firstName"));
        setLastName(sessionStorage.getItem("lastName"));
        setDescription(sessionStorage.getItem("projectDescription"));
        setTechnology(sessionStorage.getItem("technology"));
      };
    
      const handleSave = async (e) => {
        e.preventDefault();
    
        try {
          let requestHeaders = {
            headers: {
              'Authorization': 'Bearer ' + sessionStorage.getItem("token")
            },
          }

          const requestBody={
            idProfile: sessionStorage.getItem("idUserProfile"),
            projectDescription: projectDescription,
              technology: technology
            };

          await axios.put("http://localhost:5149/api/Profile", requestBody, requestHeaders);
          console.log("Profile updated successfully");
          sessionStorage.setItem("projectDescription", projectDescription);
          sessionStorage.setItem("technology", technology);
          setDescription(sessionStorage.getItem("projectDescription"));
          setTechnology(sessionStorage.getItem("technology"));
        } catch (error) {
          console.error("Error updating profile", error);
        }
      };

      const deleteProfile = async (userId) => {
        try {
          await axios.delete("http://localhost:5149/api/User/"+userId, {
            headers: {
              Authorization: "Bearer " + sessionStorage.getItem("token"),
            },
          });
      
          navigate("/login");
          fetchProfile();
        } catch (error) {
          console.error("Error:", error);
        }
      };





    return(
        <div className="main">
        <div className="auth-form-container">
        <img src={small} alt="Profile Icon" className="profile-icon"></img>
        <h2>Profile</h2>
        <form className="profile-form">
            <label htmlfor="firstName">First name</label>
            <input value={firstName} onChange={(e)=>setFirstName(e.target.value)} type="firstName" placeholder="firstName" id="firstName" name="firstName" disabled/>
            <label htmlfor="lastName">Last name</label>
            <input value={lastName} onChange={(e)=>setLastName(e.target.value)}  type="lastName" placeholder="lastName" id="lastName" name="lastName" disabled/>          
            <label htmlfor="projectDescription">Description</label>
            <input value={projectDescription} onChange={(e)=>setDescription(e.target.value)}  type="projectDescription" placeholder="projectDescription" id="projectDescription" name="projectDescription"/>
            <label htmlfor="technology">Technology</label>
            <input value={technology} onChange={(e)=>setTechnology(e.target.value)} type="technology" placeholder="technology" id="technology" name="technology"/>
            <label htmlfor="email">E-mail</label>
            <input value={email} onChange={(e)=>setEmail(e.target.value)}  type="email" placeholder="email@mail.com" id="email" name="email" disabled/>       
            <button className="btn-submit" type="submit"onClick={handleSave}>Save</button>
        </form>
    
        <button className="btn-delete" type="delete" onClick={()=>deleteProfile(sessionStorage.getItem("idUser"))}>Delete profile</button>
        <Link to="/mainpage" className="log-out-icon" >⇦</Link>
   
       </div>
       </div>
    )
}

export default Profile
