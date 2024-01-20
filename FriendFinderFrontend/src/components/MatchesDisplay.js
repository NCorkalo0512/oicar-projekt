
import { useEffect, useState } from "react";
import axios from "axios";
import {TiDeleteOutline,TiMessages} from "react-icons/ti";
import Chat from "./Chat";
import { useNavigate } from "react-router-dom";
  

const MatchesDisplay= ()=>{

    const [matchedUsers, setMatchedUsers]=useState([]);
    const idUser= sessionStorage.getItem("idUser");
    const [selectedUserId, setSelectedUserId] = useState("");
    const [selectedFullName, setSelectedFullName] = useState("");
    const navigate = useNavigate();
    const [isShown, setIsShown] = useState(false);

    useEffect(() => {
        const fetchMatchedUsers = async () => {
          try {
            const response = await axios.get(
              "http://localhost:5149/api/Match/all/matched/"+idUser,
              {
                headers: {
                  Authorization: "Bearer " + sessionStorage.getItem("token")
                }
              }
            );
            const matchedUsersData = response.data;
            setMatchedUsers(matchedUsersData);
          } catch (error) {
            console.error("Error:", error);
          }
        };
    
        fetchMatchedUsers();
      }, []);

      const handleSendMessage = (userId, firstName, lastName) => {
        setSelectedUserId(userId);
        setSelectedFullName(firstName + ' ' + lastName);
        setIsShown(current => !current);
      };
    
      const handleUnmatch = (userId) => {
        console.log("Unmatching user:", userId);
        try {
          const response = axios.post(
            "http://localhost:5149/api/Match/swipe",
            {
              swiperUser: sessionStorage.getItem("idUser"),
              swippedUser: userId.toString(),
              swipe: false
            },
            {
              headers: {
                Authorization: "Bearer " + sessionStorage.getItem("token")
              }
            }
          );
          
        } catch (error) {
          console.error("Error:", error);
        }
      };







      
    return(
      
        <div className="matches-display">
        <h2>Matches</h2>
        {matchedUsers.map((user) => (
             <div key={user.userId} className="matched-user">
            <div className="user-info">
             <h3>
               {user.firstName} {user.lastName}: {user.technology}
               <span className="icon-container">
               <TiMessages 
                 className="icon"
                 onClick={() => handleSendMessage(user.userId, user.firstName, user.lastName)}
               />
                 <TiDeleteOutline
                 className="icon"
                 onClick={() => handleUnmatch(user.userId)}
               />
             </span>
             </h3>
             </div>
              
           </div>         
          ))}
          {isShown && <Chat forwardedUserId = {selectedUserId} fullName={selectedFullName} />}
        </div>
       
       
    ) 
}

export default MatchesDisplay