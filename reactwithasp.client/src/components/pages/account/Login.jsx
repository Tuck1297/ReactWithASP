import Card from "../../bootstrap/Card";
import Row from "../../bootstrap/Row";
import Col from "../../bootstrap/Col";
import CenterElement from "../../bootstrap/CenterElement";

import LoginForm from "../../forms/Login";

const LoginPage = () => {
  return (
    <div className="page">
      <Row>
        <Col ColNumSize="3"> </Col>
        <Col ColNumSize="6">
          <CenterElement>
            <Card header="Login">
              <LoginForm />
            </Card>
          </CenterElement>
        </Col>
        <Col ColNumSize="3"> </Col>
      </Row>
    </div>
  );
};

export default LoginPage;
