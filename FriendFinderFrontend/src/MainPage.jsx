import TinderCard from "react-tinder-card";
import { useEffect, useState } from "react";
import ChatContainer from "./components/ChatContainer";
import axios from "axios";
import { useNavigate } from "react-router-dom";


const MainPage = () =>{
  
  const [unmatchedUsers, setUnmatchedUsers]=useState([]);
  const [lastDirection, setLastDirection]= useState("");
  const navigate=useNavigate();


  const idUser= sessionStorage.getItem("idUser");

  useEffect(() => {
    const fetchUnmatchedUsers = async () => {
      try {
        const response = await axios.get(
          "http://localhost:5149/api/Match/all/notmatched/"+idUser,
          {
            headers: {
              Authorization: "Bearer " + sessionStorage.getItem("token")
            }
          }
        );
        const unmatchedUsersData = response.data;
        setUnmatchedUsers(unmatchedUsersData);
      } catch (error) {
        console.error("Error:", error);
      }
    };

    fetchUnmatchedUsers();
  }, []);

  const swiped = async (direction, nameToDelete, swipedUser) => {
    console.log("Swiped: " + nameToDelete);
    setLastDirection(direction);

    try {
      const response = await axios.post(
        "http://localhost:5149/api/Match/swipe",
        {
          swiperUser: sessionStorage.getItem("idUser"),
          swippedUser: swipedUser.toString(),
          swipe: direction == 'right' ? true : false
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

  const outOfFrame = (name) => {
    console.log(name + " left the screen!");
  };


  return (
    <div className="mainpage">
      <ChatContainer />
      <div className="swiper-container">
        <div className="card-container">
          {unmatchedUsers.map((user) => (
            <TinderCard
              className="swipe"
              key={user.userId}
              onSwipe={(dir) => swiped(dir, user.technology, user.userId)}
              onCardLeftScreen={() => outOfFrame(user.technology)}
            >
              <div className="card card-design">
                <h3>{user.technology}</h3>
                
                 {/* <p>Technology: {user.technology}</p> */}
                <p>Project Description: {user.projectDescription}</p>
                 <p>First Name: {user.firstName}</p>
                 <p>Last Name: {user.lastName}</p>
              </div>
            </TinderCard>
          ))}
          <div className="swipe-info">
            {lastDirection ? <p>You swiped {lastDirection}</p> : <p />}
          </div>
        </div>
      </div>
     
    </div>
  );
}

export default MainPage;