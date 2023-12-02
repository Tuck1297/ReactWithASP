const PasswordFormComponent = ({ register, errors, disable }) => {
    return (
      <div className="mb-3">
        <label htmlFor="registercsPassword" className="form-label">
          Password
        </label>
        <input
          type="password"
          {...register("cspassword")}
          className={`form-control ${errors.cspassword ? "is-invalid" : ""}`}
          id="registercsPassword"
          aria-describedby="cspasswordHelp"
          disabled={disable}
        />
        <div className="invalid-feedback">{errors.cspassword?.message}</div>
      </div>
    );
  };
  export default PasswordFormComponent;
  