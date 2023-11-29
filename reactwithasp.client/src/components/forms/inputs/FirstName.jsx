const FirstNameFormComponent = ({ register, errors, disable }) => {
    return (
      <div className="mb-3">
        <label htmlFor="registerFirstName" className="form-label">
          First Name
        </label>
        <input
          type="text"
          {...register("firstname")}
          className={`form-control ${errors.firstname ? "is-invalid" : ""}`}
          id="registerFirstName"
          disabled={disable}
        />
        <div className="invalid-feedback">{errors.firstname?.message}</div>
      </div>
    );
  };
  export default FirstNameFormComponent;