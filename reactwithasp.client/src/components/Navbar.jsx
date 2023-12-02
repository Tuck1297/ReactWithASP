import { Outlet } from "react-router-dom";
import { NavLink } from "react-router-dom";
import { userService } from "../services/userService";
import { alertService } from "../services/alertService";
import { useNavigate } from "react-router-dom";
import { UserAuthContext } from "./UserAuthContext";
import { useContext, useEffect, useState } from "react";

const Navbar = () => {
  const { signedIn, setSignedIn } = useContext(UserAuthContext);

  const navigate = useNavigate();

  async function logout() {
    await userService
      .logout()
      .then((result) => {
        alertService.success(result);
        navigate("/");
        setSignedIn({loggedIn: false, firstname: null, lastname: null, email: null, role: null});
      })
      .catch((error) => {
        alertService.error("Error while logging out.");
      });
  }

  return (
    <>
      <nav className="navbar navbar-expand-lg bg-light">
        <div className="container-fluid">
          <NavLink className="navbar-brand" to="/">
            Navbar
          </NavLink>
          <button
            className="navbar-toggler"
            type="button"
            data-bs-toggle="collapse"
            data-bs-target="#navbarSupportedContent"
            aria-controls="navbarSupportedContent"
            aria-expanded="false"
            aria-label="Toggle navigation"
          >
            <span className="navbar-toggler-icon"></span>
          </button>
          <div className="collapse navbar-collapse" id="navbarSupportedContent">
            <ul className="navbar-nav mb-2 mb-lg-0 text-center" id="mainNavbar">
              <li className="nav-item">
                <NavLink
                  style={({ isActive }) => {
                    return isActive ? { color: "red" } : {};
                  }}
                  className="nav-link"
                  to="/"
                >
                  Home
                </NavLink>
              </li>
              {signedIn.loggedIn ? (
                <>
                  {" "}
                  <li className="nav-item">
                    <NavLink
                      style={({ isActive }) => {
                        return isActive ? { color: "red" } : {};
                      }}
                      className="nav-link"
                      to="/account/home"
                    >
                      Profile
                    </NavLink>
                  </li>
                  <li className="nav-item">
                    <NavLink
                      style={({ isActive }) => {
                        return isActive ? { color: "red" } : {};
                      }}
                      className="nav-link"
                      to="/db/cs-manage"
                    >
                      Databases
                    </NavLink>
                  </li>
                  <li className="nav-item">
                    <NavLink
                      style={({ isActive }) => {
                        return isActive ? { color: "red" } : {};
                      }}
                      className="nav-link"
                      to="/db/cs-new"
                    >
                      New Connection
                    </NavLink>
                  </li>
                  <li className="nav-item">
                    <NavLink
                      style={({ isActive }) => {
                        return isActive ? { color: "red" } : {};
                      }}
                      className="nav-link"
                      to="/account/manage"
                    >
                      Admin Tools
                    </NavLink>
                  </li>
                  <li className="nav-item">
                    <button
                      className="nav-link text-center w-100"
                      onClick={logout}
                    >
                      Logout
                    </button>
                  </li>
                </>
              ) : (
                <>
                  <li className="nav-item">
                    <NavLink
                      style={({ isActive }) => {
                        return isActive ? { color: "red" } : {};
                      }}
                      className="nav-link"
                      to="/account/login"
                    >
                      Login
                    </NavLink>
                  </li>
                  <li className="nav-item">
                    <NavLink
                      style={({ isActive }) => {
                        return isActive ? { color: "red" } : {};
                      }}
                      className="nav-link"
                      to="/account/register"
                    >
                      Register
                    </NavLink>
                  </li>
                </>
              )}
            </ul>
          </div>
        </div>
      </nav>
      <Outlet />
    </>
  );
};

export default Navbar;
