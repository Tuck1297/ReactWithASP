const CodeFormComponent = ({ register, errors, disable }) => {
    return (
      <div className="mb-3">
        <label htmlFor="registerCode" className="form-label">
          Code
        </label>
        <input
          type="text"
          {...register("code")}
          className={`form-control ${errors.code ? "is-invalid" : ""}`}
          id="registerCode"
          disabled={disable}
        />
        <div className="invalid-feedback">{errors.code?.message}</div>
      </div>
    );
  };
  export default CodeFormComponent;