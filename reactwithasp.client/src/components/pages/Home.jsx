import Card from "../bootstrap/Card";
import Row from "../bootstrap/Row";
import Col from "../bootstrap/Col";
import CenterElement from "../bootstrap/CenterElement";
import LargeSpinner from "../loading/LargeSpinner";
import { userService } from "../../services/userService";
import { useState } from "react";

const HomePage = () => {
  const [token, setToken] = useState("");
  async function login() {
    await userService.login('dev@tuckerjohnson.me', "Password1!")
    .then((result) => {
      setToken(result);
      console.log(result);
    })
    // let response = await fetch(`auth/login`, {
    //             method: "POST",
    //             credentials: "include", //--> send/receive cookies
    //             body: JSON.stringify({
    //                 email: "dev@tuckerjohnson.me",
    //                 passwordhash: "Password1!"
    //             }),
    //             headers: {
    //                 "Content-Type": "application/json",
    //             },
    //         });
    //         console.log(response)
    // await userService.UpdateJWT()
  }

  return (
    <div className="page">
      <Row>
        <Col ColNumSize="3"> </Col>
        <Col ColNumSize="6">
          <CenterElement>
            <Card header="Home Page">
              <p className="text-center">Welcome!</p>
              <button className="btn btn-secondary" onClick={login}>
                Login
              </button>
              <p>{token}</p>
              <CenterElement>
                <LargeSpinner />
              </CenterElement>
            </Card>
          </CenterElement>
        </Col>
        <Col ColNumSize="3"> </Col>
      </Row>
    </div>
  );
};

export default HomePage;
