import { yupResolver } from "@hookform/resolvers/yup";
import * as Yup from "yup";
import { useForm } from "react-hook-form";
import EmailFormComponent from "./inputs/Email";
import PasswordFormComponent from "./inputs/Password";
import ConfirmPasswordFormComponent from "./inputs/ConfirmPassword";
import FirstNameEmailComponent from "./inputs/FirstName";
import LastNameEmailComponent from "./inputs/LastName";
import { yupFormAuth } from "../../helpers/yupFormAuth";
import SmallSpinner from "../loading/SmallSpinner";

import { useState, useContext } from "react";
import { UserAuthContext } from "../UserAuthContext";
import { userService } from "../../services/userService";
import { useNavigate } from "react-router-dom";
import { alertService } from "../../services/alertService";

const RegisterForm = () => {
  const {signedIn, setSignedIn} = useContext(UserAuthContext);
  const [loaderState, setLoaderState] = useState(false);
  const navigate = useNavigate();

  const validationSchema = Yup.object().shape(
    yupFormAuth.buildFormSchema({
      email: true,
      password: true,
      confirmPassword: true,
      firstname: true,
      lastname: true
    })
  );

  const formOptions = { resolver: yupResolver(validationSchema) };

  const { register, handleSubmit, setError, formState } = useForm(formOptions);
  const { errors } = formState;

  const onSubmit = async (data) => {
    await userService.register(data.email, data.firstname, data.lastname, data.password, data.confirmPassword)
    .then((result) => {
      setSignedIn({loggedIn: true, firstname: data.firstname, lastname: data.lastname, email: data.email, role: null})
      navigate("/account/home");
      alertService.success("Successfully registered and signed in!", true);
      localStorage.setItem("last-updated", new Date());
    })
    .catch((error) => {
      setLoaderState(false);
      console.error("Unable to register a new user at this time. If this problem persists, please reach out to our admin at dev@tuckerjohnson.me.");
      alertService.error(error);
    })
  };

  return (
    <form onSubmit={handleSubmit(onSubmit)}>
      <EmailFormComponent register={register} errors={errors} />
      <FirstNameEmailComponent register={register} errors={errors}/>
      <LastNameEmailComponent register = {register} errors={errors}/>
      <PasswordFormComponent register={register} errors={errors} />
      <ConfirmPasswordFormComponent register={register} errors={errors} />
      <button
        type="submit"
        className="btn btn-primary mt-2 w-100"
        disabled={loaderState}
      >
        {loaderState ? <SmallSpinner /> : "Register"}
      </button>
    </form>
  );
};

export default RegisterForm;
