import Card from "../bootstrap/Card";
import Row from "../bootstrap/Row";
import Col from "../bootstrap/Col";
import CenterElement from "../bootstrap/CenterElement";
import LargeSpinner from "../loading/LargeSpinner";
import { userService } from "../../services/userService";
import { alertService } from "../../services/alertService";
import { useState, useContext } from "react";
import { UserAuthContext } from "../UserAuthContext";

const HomePage = () => {
  const {signedIn, setSignedIn} = useContext(UserAuthContext);
  return (
    <div className="page">
      <Row>
        <Col ColNumSize="3"> </Col>
        <Col ColNumSize="6">
          <CenterElement>
            <Card header="Home Page">
              <p className="text-center">Welcome! {signedIn.firstname} This is the homepage...</p>
              <CenterElement>
                {/* <LargeSpinner /> */}
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
