import { Routes, Route } from "react-router-dom";
import HomePage from "./components/pages/Home";
import LoginPage from "./components/pages/account/Login";
import RegisterPage from "./components/pages/account/Register";
import InfoPage from "./components/pages/account/Info";
import ErrorPage from "./components/pages/ErrorPage";
import ForgotPasswordPage from "./components/pages/account/ForgotPassword";
import ResetPasswordPage from "./components/pages/account/ResetPassword";
import ManageUsersPage from "./components/pages/account/ManageUsers";
import Navbar from "./components/Navbar";
import Footer from "./components/Footer";
import { UserAuthContext } from "./components/UserAuthContext";
import { useState, useEffect } from "react";
import Alert from "./components/Alert";
import { useNavigate } from "react-router-dom";
import { alertService } from "./services/alertService";
import { userService } from "./services/userService";

export default function App() {
  const navigate = useNavigate();

  const [signedIn, setSignedIn] = useState({
    loggedIn: false,
    email: null,
    firstname: null,
    lastname: null
  });

  async function checkSignedIn() {
    if (!signedIn.loggedIn) {
      await userService
        .refresh()
        .then((result) => {
          console.log("Token refreshed.");
          setSignedIn((prev) => ({ ...prev, loggedIn: true }));
        })
        .catch((error) => {
          // alertService.warning("Your session has expired. Please log back in.")
          // navigate("/account/login");
        });
    }
  }

  useEffect(() => {
    checkSignedIn();
  }, []);

  return (
    <>
      <UserAuthContext.Provider value={{ signedIn, setSignedIn }}>
        <Alert />
        <Routes>
          <Route element={<Navbar />}>
            <Route path="/" element={<HomePage />}></Route>
            <Route path="/account/login" element={<LoginPage />}></Route>
            <Route path="/account/register" element={<RegisterPage />}></Route>
            <Route path="/account/home" element={<InfoPage />}></Route>
            <Route path="/account/manage" element={<ManageUsersPage/>}></Route>
            <Route
              path="/account/forgotpassword"
              element={<ForgotPasswordPage />}
            ></Route>
            <Route
              path="/account/resetpassword"
              element={<ResetPasswordPage />}
            ></Route>
            <Route path="*" element={<ErrorPage />}></Route>
          </Route>
        </Routes>
        <Footer />
      </UserAuthContext.Provider>
    </>
  );
}
