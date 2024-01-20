import axios from "axios";
import React from "react";
import ChatInput from "./ChatInput";
import { useState,useEffect } from "react";
import{TbMessageReport} from "react-icons/tb";
import {MdDelete} from "react-icons/md";
import { ToastContainer, toast } from 'react-toastify';
import 'react-toastify/dist/ReactToastify.css';



const Chat= ({forwardedUserId,fullName})=>{
   
    const [text, setText]= useState("");
    const [messages, setMessages] = useState([]);
    const[senderId, setSenderId]=useState(sessionStorage.getItem("idUser"));
    const[receiverId,setReciverId]=useState("");
    const [selectedFullName, setSelectedFullName] = useState("");
    const [messageId, setMessageId]=useState("");

    useEffect(() => {
        fetchMessages();
      }, [senderId,receiverId ]);

      const fetchMessages = async () => {
        console.log(fullName);
        setSelectedFullName(fullName);
        setReciverId(parseInt(forwardedUserId));
        try {
          const response = await axios.get(
            "http://localhost:5149/api/Message/sender/"+senderId+"/receiver/"+receiverId,
            {
              headers: {
                Authorization: "Bearer " + sessionStorage.getItem("token"),
              },
            }
          );
  
          setMessages(response.data);
        } catch (error) {
          console.error("Error:", error);
        }
      };

    const handleMessageSubmit= async()=>{
        try{
            const response= await axios.post(
                "http://localhost:5149/api/Message/send",
                {
                    senderId:senderId,
                    receiverId:receiverId,
                    text:text
                },
                {
                 headers:{
                    Authorization: "Bearer " + sessionStorage.getItem("token")
                    },
                }
            )
            console.log("Message sent:", response.data);

            fetchMessages();
            setText("");
        } catch(error){
            console.error("Error:", error);
        }
    }
    const deleteMessage = async (messageId) => {
      try {
        await axios.delete("http://localhost:5149/api/Message/"+messageId, {
          headers: {
            Authorization: "Bearer " + sessionStorage.getItem("token"),
          },
        });
    
       
        fetchMessages();
      } catch (error) {
        console.error("Error:", error);
      }
    };
    

    const reportMessage= async(reason,messageId)=>{
      try{
          const response= await axios.post(
              "http://localhost:5149/api/Message/report",
              {
                 reason:reason,
                 messageId:messageId,
                userSenderId:senderId
              },
              {
               headers:{
                  Authorization: "Bearer " + sessionStorage.getItem("token")
                  },
              }
          )
          console.log("Message report:", response.data);

          fetchMessages();
          setText("");

          notify();
      
      } catch(error){
          console.error("Error:", error);
      }
  }
  const notify=()=>{
    toast.success("Message reported succesful", {
      position: "top-center",
      autoClose: 5000,
      hideProgressBar: false,
      closeOnClick: true,
      pauseOnHover: true,
      draggable: true,
      progress: undefined,
      theme: "colored",
      });
  }
    return(
        <div className="chat-display"style={{ height: "300px", overflow: "auto" }}>
            <h2>Messages with {selectedFullName}</h2> 
            {messages.map((message)=>
           
            <div key={message.idmessage} className={message.isSender ? "message-orange" : "message-blue"}>                
              <p className="message-content">{message.text}</p>
              <TbMessageReport onClick={()=>reportMessage("This message was reported by:"+sessionStorage.getItem("firstName")+" "+sessionStorage.getItem("lastName")
              ,message.idmessage)}></TbMessageReport>
             
             <MdDelete onClick={()=>deleteMessage(message.idmessage)}></MdDelete>
              <div className={message.isSender ? "message-timestamp-right" : "message-timestamp-left"}>{message.dateTime.toString()}</div>
              
            </div>
            )}   
             <input
             id="messageInput"
        type="text"
        value={text}
        onChange={(e) => setText(e.target.value)}
        className="custom-text"
        placeholder="Type your message..."
      />
            <button className="button-chat" onClick={handleMessageSubmit} >Submit</button>
            <ToastContainer 
              position="top-center"
              autoClose={5000}
              hideProgressBar={false}
              newestOnTop={false}
              closeOnClick
              rtl={false}
              pauseOnFocusLoss
              draggable
              pauseOnHover
              theme="colored"
              /> 
             </div>



    ) 
}

export default Chat