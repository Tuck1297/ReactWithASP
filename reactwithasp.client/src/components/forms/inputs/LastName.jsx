const LastNameFormComponent = ({ register, errors, disable }) => {
    return (
      <div className="mb-3">
        <label htmlFor="registerLastName" className="form-label">
          Last Name
        </label>
        <input
          type="text"
          {...register("lastname")}
          className={`form-control ${errors.lastname ? "is-invalid" : ""}`}
          id="registerLastName"
          disabled={disable}
        />
        <div className="invalid-feedback">{errors.lastname?.message}</div>
      </div>
    );
  };
  export default LastNameFormComponent;