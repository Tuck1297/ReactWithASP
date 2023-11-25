import Card from "../bootstrap/Card";
import Row from "../bootstrap/Row";
import Col from "../bootstrap/Col";
import CenterElement from "../bootstrap/CenterElement";
import LargeSpinner from "../loading/LargeSpinner";

const HomePage = () => {
  async function getWeather() {
    const result = await fetch("weatherforecast");
    const data = await result.json();
    console.log(data);
  }

  return (
    <div className="page">
      <Row>
        <Col ColNumSize="3"> </Col>
        <Col ColNumSize="6">
          <CenterElement>
            <Card header="Home Page">
              <p className="text-center">Welcome!</p>
              <button className="btn btn-secondary" onClick={getWeather}>
                Get Weather
              </button>
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
