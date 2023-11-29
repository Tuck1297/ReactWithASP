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
import { NavLink } from "react-router-dom";

const InfoPage = () => {
  const { signedIn, setSignedIn } = useContext(UserAuthContext);
  const [userInfoExists, setUserInfoExists] = useState(null);
  const navigate = useNavigate();
  async function getUserInfo() {
    setTimeout(async () => {
      if (
        signedIn.loggedIn &&
        signedIn.firstname != null &&
        signedIn.lastname != null &&
        signedIn.email != null
      ) {
        setUserInfoExists(true);
        return;
      }
      await userService
        .getUserInfo()
        .then((result) => {
          const combinedName = `${result.firstName} ${result.lastName}`;
          setUserInfoExists({ email: result.email, name: combinedName });
          setSignedIn({
            firstname: result.firstName,
            lastname: result.lastName,
            email: result.email,
            loggedIn: true,
            role: result.role,
          });
        })
        .catch(async (error) => {
          console.log(error);
          navigate("/account/login");
          await userService.logout();
          alertService.warning("In order to access this page you must login");
        });
    }, 1000);
  }

  useEffect(() => {
    getUserInfo();
  }, []);
  return (
    <div className="page">
      <Row>
        <Col ColNumSize="3"> </Col>
        <Col ColNumSize="6">
          <CenterElement>
            <Card header="Profile">
              <CenterElement>
                <section>
                  <div>
                    {userInfoExists ? (
                      <div>
                        <h2 className="text-center">Update User Info</h2>
                        <UpdateUserForm info={signedIn} />
                      </div>
                    ) : (
                      <CenterElement>
                        <LargeSpinner />
                      </CenterElement>
                    )}
                  </div>
                  <CenterElement>
                  {signedIn.role === "Admin" ? (
                    <NavLink to="/account/manage" className="btn btn-primary mt-4">
                      Manage Accounts
                    </NavLink>
                  ) : (
                    ""
                  )}
                  </CenterElement>
                </section>
              </CenterElement>
            </Card>
          </CenterElement>
        </Col>
        <Col ColNumSize="3"> </Col>
      </Row>
    </div>
  );
};

export default InfoPage;
