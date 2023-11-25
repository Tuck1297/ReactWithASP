import * as Yup from "yup";

export const yupFormAuth = {
  buildFormSchema,
};

/* Builds the form Yup Schema based on array params */
/* Sample FormSchemaObj: 
{
    name: true,
    email: true,
    password: true,
    confirmPassword: true,
    message: false
}

If set to true, then it will be added to the resulting 
schema and not added if set to false.
*/

/*
    formObj: Object defined to build Yup Form Schema from
    currentSchema: Previously build schema that needs to be modified.
*/
function buildFormSchema(formObj, currentSchema) {
  let schema = { ...currentSchema };

  if (formObj.name) {
    schema.name = Yup.string().required("Full Name is required");
  }
  if (formObj.email) {
    schema.email = Yup.string()
      .email("Invalid email address")
      .test(
        "is-email",
        "Invalid email address",
        (value) =>
          !value || /^[A-Z0-9._%+-]+@[A-Z0-9.-]+\.[A-Z]{2,4}$/i.test(value)
      )
      .required("Email is required");
  }
  if (formObj.password) {
    schema.password = Yup.string()
      .required("Password is required")
      .min(6, "Password must be at least 6 characters")
      .matches(
        /^(?=.*[a-zA-Z])(?=.*\d)(?=.*[@#$%^&!])[A-Za-z\d@#$%^&!]{6,}$/,
        "Password must contain at least 6 characters, including letters, numbers, and special characters (@, #, $, %, ^, &, !)"
      );
  }
  if (formObj.confirmPassword) {
    schema.confirmPassword = Yup.string()
      .required("Confirmation of Password is required")
      .min(6, "Password must be at least 6 characters")
      .oneOf([Yup.ref("password"), null], "Passwords must match");
  }
  if (formObj.message) {
    schema.message = Yup.string().required("Message is required");
  }
  if (formObj.code) {
    schema.code = Yup.string().required("Code is required");
  }
  return schema;
}
// TODO: add to password that at least one capital letter needs
// to be present... also check and adjust to what it says in .net...
// passwords must have at least one non alphanumeric character
// passwords must have at least one digit (0-9)
// passwords must have at least one uppercase
// passwords must have at least 6 characters