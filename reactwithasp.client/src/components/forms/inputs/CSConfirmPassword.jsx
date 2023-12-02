const CSConfirmPasswordFormComponent = ({ register, errors, disable }) => {
    return (
      <div className="mb-3">
        <label htmlFor="registercsConfirmPassword" className="form-label">
          ConfirmPassword
        </label>
        <input
          type="password"
          {...register("csconfirmPassword")}
          className={`form-control ${errors.csconfirmPassword ? "is-invalid" : ""}`}
          id="registercsConfirmPassword"
          aria-describedby="confirmPasswordHelp"
          disabled={disable}
        />
        <div className="invalid-feedback">{errors.csconfirmPassword?.message}</div>
      </div>
    );
  };
  export default CSConfirmPasswordFormComponent;
  