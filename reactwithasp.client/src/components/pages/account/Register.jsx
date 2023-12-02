import Card from "../../bootstrap/Card";
import Row from "../../bootstrap/Row";
import Col from "../../bootstrap/Col";
import CenterElement from "../../bootstrap/CenterElement";

import RegisterForm from "../../forms/Register";
const RegisterPage = () => {
  return (
    <div className="page">
      <Row>
        <Col ColNumSize="3"> </Col>
        <Col ColNumSize="6">
          <CenterElement>
            <Card header="Register" className="mt-5">
              <RegisterForm />
            </Card>
          </CenterElement>
        </Col>
        <Col ColNumSize="3"> </Col>
      </Row>
    </div>
  );
};

export default RegisterPage;
