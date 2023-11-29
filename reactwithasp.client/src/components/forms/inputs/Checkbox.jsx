"use client";

import { Controller } from "react-hook-form";

const Checkbox = ({ name, register, checkboxMsg, ...props }) => {
  return (
    <div className="mb-3 form-check mt-3">
      <input
        {...register(`${name}`)}
        {...props}
        type="checkbox"
        className="form-check-input"
        id={`${name}Check`}
      />
      <label className="form-check-label ms-2" htmlFor={`${name}Check`}>
        {checkboxMsg}
      </label>
    </div>
  );
};
export default Checkbox;
