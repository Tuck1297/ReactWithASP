import { useState } from "react";

const Dropdown = ({ register, errors, elements, initial, title, disable }) => {
  const [val, setVal] = useState(initial);
  return (
    <div className="mb-3">
      <label htmlFor="dropdown" className="form-label">
        {title}
      </label>
      <select
        id="dropdown"
        className={`form-select ${errors.dropdown ? "is-invalid" : ""}`}
        {...register("dropdown")}
        value={val}
        onChange={(e) => setVal(e.target.value)}
        disabled={disable}
      >
        {elements.map((element, index) => (
          <option value={element.toLowerCase()} key={index} disabled={element != "Postgres"}>{element} {element != "Postgres" ? " ~ Not Available" : ""}</option>
        ))}
      </select>
    </div>
  );
};
export default Dropdown;
