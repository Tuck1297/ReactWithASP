const EmailFormComponent = ({ register, errors, disable }) => {
    return (
      <div className="mb-3">
        <label htmlFor="registerEmail" className="form-label">
          Email address
        </label>
        <input
          type="email"
          {...register("email")}
          className={`form-control ${errors.email ? "is-invalid" : ""}`}
          id="registerEmail"
          aria-describedby="emailHelp"
          disabled={disable}
        />
        <div className="invalid-feedback">{errors.email?.message}</div>
        <div id="emailHelp" className="form-text">
          We&apos;ll never share your email with anyone else.
        </div>
      </div>
    );
  };
  export default EmailFormComponent;