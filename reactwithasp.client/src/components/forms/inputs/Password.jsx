const PasswordFormComponent = ({ register, errors }) => {
  return (
    <div className="mb-3">
      <label htmlFor="registerPassword" className="form-label">
        Password
      </label>
      <input
        type="password"
        {...register("password")}
        className={`form-control ${errors.password ? "is-invalid" : ""}`}
        id="registerPassword"
        aria-describedby="passwordHelp"
      />
      <div className="invalid-feedback">{errors.password?.message}</div>
    </div>
  );
};
export default PasswordFormComponent;
