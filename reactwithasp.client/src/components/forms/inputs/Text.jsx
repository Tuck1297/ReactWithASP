const GeneralTextFormComponent = ({ register, errors, disable, title, registerName }) => {
    return (
      <div className="mb-3">
        <label htmlFor="registerTextGeneral" className="form-label">
          {title}
        </label>
        <input
          type="text"
          {...register(registerName)}
          className={`form-control ${errors[registerName] ? "is-invalid" : ""}`}
          id={`register${registerName}`}
          disabled={disable}
        />
        <div className="invalid-feedback">{errors[registerName]?.message}</div>
      </div>
    );
  };
  export default GeneralTextFormComponent;