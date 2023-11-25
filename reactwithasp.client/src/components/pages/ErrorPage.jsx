import Card from "../bootstrap/Card";
import Row from "../bootstrap/Row";
import Col from "../bootstrap/Col";
import CenterElement from "../bootstrap/CenterElement";
import { Link } from "react-router-dom";

const ErrorPage = () => {
  return (
    <div className="page">
      <Row>
        <Col ColNumSize="3"> </Col>
        <Col ColNumSize="6">
          <CenterElement>
            <Card header="Error Page">
              <CenterElement>
              <Link className="btn btn-primary" to="/">Go Home</Link>
              </CenterElement>
            </Card>
          </CenterElement>
        </Col>
        <Col ColNumSize="3"> </Col>
      </Row>
    </div>
  );
};

export default ErrorPage;
