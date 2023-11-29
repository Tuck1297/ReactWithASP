import Card from "../../bootstrap/Card";
import Row from "../../bootstrap/Row";
import Col from "../../bootstrap/Col";
import CenterElement from "../../bootstrap/CenterElement";
import LargeSpinner from "../../loading/LargeSpinner";
import { useEffect, useState, useContext } from "react";
import { userService } from "../../../services/userService";
import { useNavigate } from "react-router-dom";
import { alertService } from "../../../services/alertService";
import { UserAuthContext } from "../../UserAuthContext";
import UpdateUserForm from "../../forms/UpdateUser";

const InfoPage = () => {
  const { signedIn, setSignedIn } = useContext(UserAuthContext);
  const [allUserInfo, setAllUserInfo] = useState(null);
  const navigate = useNavigate();
  async function getAllUserInfo() {
    if (signedIn.loggedIn != true && signedIn.role != "Admin") {
      navigate("/account/home");
      alertService.warning("You currently do not have access to this page.");
      return;
    }

    setTimeout(async () => {
      await userService
        .getAllUserInfo()
        .then((result) => {
          setAllUserInfo(result);
        })
        .catch(async (error) => {
          console.log(error);
          navigate("/account/login");
          await userService.logout();
          alertService.warning("In order to access this page you must login.");
        });
    }, 1000);
  }

  useEffect(() => {
    getAllUserInfo();
  }, []);
  return (
    <div className="page">
      <Row>
        <Col ColNumSize="1"> </Col>
        <Col ColNumSize="10">
            <section className="mt-4">
              <div>
                {allUserInfo ? (
                  <div className="d-flex flex-column justify-content-center align-items-center">
                    <h2 className="text-center">All User Information</h2>
                    <table>
                      <thead>
                        <tr>
                          <th style={{ width: "20%" }} className="text-center pt-3 pb-3">
                            First Name
                          </th>
                          <th style={{ width: "20%" }} className="text-center pt-3 pb-3">
                            Last Name
                          </th>
                          <th style={{ width: "20%" }} className="text-center pt-3 pb-3">
                            Email
                          </th>
                          <th style={{ width: "20%" }} className="text-center pt-3 pb-3">
                            Role
                          </th>
                          <th style={{ width: "20%" }} className="text-center pt-3 pb-3">
                            Delete
                          </th>
                        </tr>
                      </thead>
                      <tbody>
                        {allUserInfo.map((x, i) => (
                          <tr key={i}>
                            <td className="text-center pt-3 pb-3">{x.firstName}</td>
                            <td className="text-center pt-3 pb-3">{x.lastName}</td>
                            <td className="text-center pt-3 pb-3">{x.email}</td>
                            <td className="text-center pt-3 pb-3">{x.role}</td>
                            <td className="text-center pt-3 pb-3"><button className="btn btn-danger">Delete</button></td>
                          </tr>
                        ))}
                      </tbody>
                    </table>
                  </div>
                ) : (
                  <CenterElement>
                    <LargeSpinner />
                  </CenterElement>
                )}
              </div>
            </section>
        </Col>
        <Col ColNumSize="1"> </Col>
      </Row>
    </div>
  );
};

export default InfoPage;
