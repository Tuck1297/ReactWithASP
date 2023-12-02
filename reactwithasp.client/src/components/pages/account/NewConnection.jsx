import Row from "../../bootstrap/Row";
import Col from "../../bootstrap/Col";
import ConnectionStringForm from "../../forms/ConnectionString";
import Card from "../../bootstrap/Card";
import CardBody from "../../bootstrap/CardBody";
import CardHeader from "../../bootstrap/CardHeader";
import CenterElement from "../../bootstrap/CenterElement";

export default function NewConnectionPage() {
  return (
    <>
      <Row>
        <Col ColNumSize="1"> </Col>
        <Col ColNumSize="10">
          <CenterElement>
            <Card className="mt-5" header="New Database Connection">
              <ConnectionStringForm />
            </Card>
          </CenterElement>
        </Col>
        <Col ColNumSize="1"> </Col>
      </Row>
    </>
  );
}
