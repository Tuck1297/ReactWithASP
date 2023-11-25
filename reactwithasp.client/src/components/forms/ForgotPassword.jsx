import { yupResolver } from "@hookform/resolvers/yup";
import * as Yup from "yup";
import { useForm } from "react-hook-form";
import EmailFormComponent from "./inputs/Email";
import { yupFormAuth } from "../../helpers/yupFormAuth";
import SmallSpinner from "../loading/SmallSpinner";

import { useState } from "react";

const ForgotPasswordForm = () => {
    const [loaderState, setLoaderState] = useState(false);

  const validationSchema = Yup.object().shape(
    yupFormAuth.buildFormSchema({ email: true })
  );

  const formOptions = { resolver: yupResolver(validationSchema) };

  const { register, handleSubmit, setError, formState } = useForm(formOptions);
  const { errors } = formState;

  const onSubmit = async (data) => {
    setLoaderState(true);
    console.log(data);
  };

  return   <form onSubmit={handleSubmit(onSubmit)}>
    <EmailFormComponent register={register} errors={errors}/>
    <button
            type="submit"
            className="btn btn-primary mt-2 w-100"
            disabled={loaderState}
          >
            {loaderState ? <SmallSpinner /> : "Submit"}
          </button>
  </form>;
};

export default ForgotPasswordForm;