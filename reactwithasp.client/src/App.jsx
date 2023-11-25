import { Routes, Route } from "react-router-dom";
import HomePage from "./components/pages/Home";
import LoginPage from "./components/pages/account/Login";
import RegisterPage from "./components/pages/account/Register";
import InfoPage from "./components/pages/account/Info";
import ErrorPage from "./components/pages/ErrorPage";
import ForgotPasswordPage from "./components/pages/account/ForgotPassword";
import ResetPasswordPage from "./components/pages/account/ResetPassword";
import Navbar from "./components/Navbar";
import Footer from "./components/Footer";

export default function App() {
  return (
    <>
      <Routes>
        <Route element={<Navbar />}>
          <Route path="/" element={<HomePage />}></Route>
          <Route path="/account/login" element={<LoginPage />}></Route>
          <Route path="/account/register" element={<RegisterPage />}></Route>
          <Route path="/account/info" element={<InfoPage />}></Route>
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
    </>
  );
}
