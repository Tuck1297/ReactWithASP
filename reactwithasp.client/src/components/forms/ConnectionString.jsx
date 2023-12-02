import { yupResolver } from "@hookform/resolvers/yup";
import * as Yup from "yup";
import { useForm } from "react-hook-form";
import { yupFormAuth } from "../../helpers/yupFormAuth";
import { alertService } from "../../services/alertService";
import { dbCSService } from "../../services/dbCSService";
import { useState } from "react";
import GeneralTextFormComponent from "./inputs/Text";
import CSPasswordFormComponent from "./inputs/CSPassword";
import CSConfirmPasswordFormComponent from "./inputs/CSConfirmPassword";
import Dropdown from "./inputs/Dropdown";
import SmallSpinner from "../loading/SmallSpinner";
import Row from "../bootstrap/Row";
import Col from "../bootstrap/Col";

const ConnectionStringForm = () => {
  const [loaderState, setLoaderState] = useState(false);

  const validationSchema = Yup.object().shape(
    yupFormAuth.buildFormSchema({
      cspassword: true,
      csconfirmPassword: true,
      csStringName: true,
      host: true,
      dbName: true,
      port: true,
      dbUserId: true,
      dbSchema: true,
    })
  );

  const formOptions = { resolver: yupResolver(validationSchema) };

  const { register, handleSubmit, setError, formState } = useForm(formOptions);
  const { errors } = formState;

  const onSubmit = async (data) => {
    setLoaderState(true);
    console.log(data);
  };

  return (
    <>
      <form onSubmit={handleSubmit(onSubmit)} className="">
        <Row>
          <Col ColNumSize="12">
            <GeneralTextFormComponent
              errors={errors}
              register={register}
              disable={loaderState}
              title="Connection String Name"
              registerName="csStringName"
            />
          </Col>
          <Col ColNumSize="12">
            <Dropdown
              errors={errors}
              register={register}
              elements={["Postgres", "SQL Server", "SQLLite", "Firebase"]}
              initial="postgres"
              title="Database Type"
              disable={loaderState}
            />
          </Col>
          <Col>
            <GeneralTextFormComponent
              errors={errors}
              register={register}
              disable={loaderState}
              title="Database Host"
              registerName="host"
            />
            <GeneralTextFormComponent
              errors={errors}
              register={register}
              disable={loaderState}
              title="Database Name"
              registerName="dbName"
            />
          </Col>
          <Col>
            <GeneralTextFormComponent
              errors={errors}
              register={register}
              disable={loaderState}
              title="Database Port"
              registerName="port"
            />
            <GeneralTextFormComponent
              errors={errors}
              register={register}
              disable={loaderState}
              title="Database User Id"
              registerName="dbUserId"
            />
            <GeneralTextFormComponent
              errors={errors}
              register={register}
              disable={loaderState}
              title="Database Schema"
              registerName="dbSchema"
            />
          </Col>
          <Col ColNumSize="12">
            <CSPasswordFormComponent
              errors={errors}
              register={register}
              disable={loaderState}
            />
            <CSConfirmPasswordFormComponent
              errors={errors}
              register={register}
              disable={loaderState}
            />
          </Col>
        </Row>
        <button
          type="submit"
          className="btn btn-primary mt-2 w-100"
          disabled={loaderState}
        >
          {loaderState ? <SmallSpinner /> : "Submit"}
        </button>
      </form>
    </>
  );
};

export default ConnectionStringForm;
