import { NavLink } from "react-router-dom";
import { useContext } from "react";
import { UserAuthContext } from "./UserAuthContext";
const Footer = () => {
  const { signedIn, setSignedIn } = useContext(UserAuthContext);
  return (
    <footer className="py-3 my-4">
      <ul className="nav justify-content-center border-bottom pb-3 mb-3">
        <li className="nav-item">
          <NavLink className="nav-link px-2 text-muted" to="/">
            Home
          </NavLink>
        </li>
        {signedIn.loggedIn ? (
          <>
            <li className="nav-item">
              <NavLink className="nav-link px-2 text-muted" to="/account/home">
                Profile
              </NavLink>
            </li>
            <li className="nav-item">
              <NavLink className="nav-link px-2 text-muted" to="/db/cs-manage">
                Databases
              </NavLink>
            </li>
            <li className="nav-item">
              <NavLink className="nav-link px-2 text-muted" to="/db/cs-new">
                New Connection
              </NavLink>
            </li>
            <li className="nav-item">
              <NavLink
                className="nav-link px-2 text-muted"
                to="/account/manage"
              >
                Admin Tools
              </NavLink>
            </li>
          </>
        ) : (
          <>
            <li className="nav-item">
              <NavLink className="nav-link px-2 text-muted" to="/account/login">
                Login
              </NavLink>
            </li>
            <li className="nav-item">
              <NavLink
                className="nav-link px-2 text-muted"
                to="/account/register"
              >
                Register
              </NavLink>
            </li>
          </>
        )}
        {/* <li className="nav-item">
          <NavLink
            className="nav-link px-2 text-muted"
            to="/account/forgotpassword"
          >
            Forgot Password
          </NavLink>
        </li> */}
        {/* <li className="nav-item">
          <NavLink
            className="nav-link px-2 text-muted"
            to="/account/resetpassword"
          >
            Reset Password
          </NavLink>
        </li> */}
        {/* <li className="nav-item">
          <NavLink
            className="nav-link px-2 text-muted"
            to="/error"
          >
            Error Page
          </NavLink>
        </li> */}
      </ul>
      <p className="text-center text-muted">
        © 2023 Tucker Johnson, DotNet 8 Hackathon
      </p>
    </footer>
  );
};

export default Footer;
