import {Link} from 'react-router-dom'
import { CgProfile } from 'react-icons/cg';
import { CiLogout } from 'react-icons/ci';
import React from 'react';
import { confirmAlert } from 'react-confirm-alert';
import 'react-confirm-alert/src/react-confirm-alert.css';

const ChatHeader= ()=>{
const iconStyle={
    fontSize:'24px',
    marginRight: '10px'

  
};
const firstName= sessionStorage.getItem("firstName");

const handleLogout=()=>{
  
    confirmAlert({
        title: "Confirm logout",
        message:"Are you sure you want to logout?",
        buttons: [
            {
              label: 'Yes',
              onClick: () => {
                
                window.location.href = '/login';
              }
            },
            {
              label: 'No',
              onClick: () => {
               
              }
            }
          ]
    })
}
const Greetings = () => {
    let myDate = new Date();
    let hours= myDate.getHours();
    let greet;

    if (hours < 12)
        greet =  "morning";
    else if (hours >= 12 && hours <= 19)
        greet = "afternoon";
    else if (hours >= 19 && hours <= 24)
       greet = "evening";
    
    return <span>Good {greet}, {firstName}</span>
}
    return(
        <div className="chat-container-header">
        <Greetings className="greetings" />
        <Link to="/profile">
          <CgProfile style={{ ...iconStyle, marginLeft: '50px' }} />
        </Link>
        <button onClick={handleLogout}>
          <CiLogout style={{ ...iconStyle, marginRight: '50px' , backgroundImage: 'linear-gradient(79deg, #7439db, #C66FBC 48%, #F7944D)',
         color:"#F7944D",
          padding: '3px 5px',
          borderRadius: '5px',}} />
        </button>
      </div>



    );
};

export default ChatHeader;