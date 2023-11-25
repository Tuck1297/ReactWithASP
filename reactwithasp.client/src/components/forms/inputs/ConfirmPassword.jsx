const ConfirmPasswordFormComponent = ({ register, errors }) => {
  return (
    <div className="mb-3">
      <label htmlFor="registerConfirmPassword" className="form-label">
        ConfirmPassword
      </label>
      <input
        type="password"
        {...register("confirmPassword")}
        className={`form-control ${errors.confirmPassword ? "is-invalid" : ""}`}
        id="registerConfirmPassword"
        aria-describedby="confirmPasswordHelp"
      />
      <div className="invalid-feedback">{errors.confirmPassword?.message}</div>
    </div>
  );
};
export default ConfirmPasswordFormComponent;
