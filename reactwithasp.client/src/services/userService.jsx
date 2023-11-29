import { fetchWrapper } from "../helpers/fetch-wrapper";

export const userService = {
  login,
  logout,
  getUserInfo,
  register,
  update,
  refresh,
  getAllUserInfo,
  delete: _delete,
};

async function login(email, password) {
  return await fetchWrapper.post("auth/login", {
    email: email,
    passwordhash: password,
  });
}

async function logout() {
  localStorage.clear();
  return await fetchWrapper.get("auth/logout");
}

async function refresh() {
    return await fetchWrapper.post("auth/refresh-token");
}

async function getUserInfo() {
  return await fetchWrapper.get("user/getbyid");
}

async function getAllUserInfo() {
  return await fetchWrapper.get("user/getall");
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
}

async function update(
  email,
  firstname,
  lastname,
  passwordhash = null,
  confirmedpasswordhash = null
) {
  if (passwordhash === null) {
    return fetchWrapper.put("user/update", { email, firstname, lastname });
  } else {
    return fetchWrapper.put("user/update", {
      email,
      firstname,
      lastname,
      passwordhash,
      confirmedpasswordhash,
    });
  }
}

async function _delete(email) {
  return fetchWrapper.delete(`user/delete/${email}`);
}

async function roleUpdate(email, role) {
  return fetchWrapper.put("user/roleupdate", { email, role });
}

async function getAllUsers() {
  return fetchWrapper.get("auth/getall");
}