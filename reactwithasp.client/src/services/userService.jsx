import { fetchWrapper } from "../helpers/fetch-wrapper";

export const userService = {
  login,
  logout,
  UpdateJWT,
  getUserInfo,
  register,
  update,
  delete: _delete,
};

async function login(email, password) {
  const {token} = await fetchWrapper.post("auth/login", {
    email: email,
    passwordhash: password,
  });
  console.log(token)

  return "Signed in successfully!";
}

async function logout() {
  // logout logic -- getting rid of tokens and so forth
}

async function UpdateJWT() {
  return await fetchWrapper.post("auth/reset-token");
}

async function getUserInfo() {
    return await fetchWrapper.get('account/getbyid');
}

async function register(
  email,
  firstname,
  lastname,
  passwordhash,
  confirmedpasswordhash
) {
  return await fetchWrapper.post("auth/register", {
    email,
    firstname,
    lastname,
    passwordhash,
    confirmedpasswordhash,
  });
  // set tokens here...
}

async function update(email, firstname, lastname, passwordhash, confirmedpasswordhash) {
    return fetchWrapper.put('account/update', {email, firstname, lastname, passwordhash, confirmedpasswordhash});
    // set tokens here...
}

async function _delete(email) {
    return fetchWrapper.delete(`account/delete/${email}`);
}

async function roleUpdate(email, role) {
    return fetchWrapper.put('account/roleupdate', {email, role});
    // set tokens here...
}

async function getAllUsers() {
    return fetchWrapper.get('auth/getall');
}
