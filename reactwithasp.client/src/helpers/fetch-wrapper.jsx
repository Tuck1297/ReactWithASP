import axios from "axios";

import { userService } from "../services/userService";
import { alertService } from "../services/alertService";

export const fetchWrapper = {
  get: request("GET"),
  post: request("POST"),
  put: request("PUT"),
  delete: request("DELETE"),
};

function request(method) {
  return async (url, body) => {
    console.log(url)
    const requestOptions = {
      method,
      headers: {},
      url: url, // Set the URL explicitly in Axios request options.
      withCredentials: false,
      baseURL: "https://localhost:5173/"
    };

    if (body) {
      requestOptions.headers["Content-Type"] = "application/json";
      requestOptions.data = body; // Use 'data' property instead of 'body' in Axios.
    }

    try {
      const response = await axios(requestOptions); // Use Axios for making the request.
      return handleResponse(response);
    } catch (error) {
      // console.error('Error during request:', error);
      return handleResponse(error.response);
      throw error; // Rethrow the error for further handling, if needed.
    }
  };
}

// helper functions

function authHeader(url) {
  // return auth header with jwt if user is logged in and request is to the api url
  const user = userService.userValue;
  const isLoggedIn = user?.token;
  const isApiUrl = url.startsWith(publicRuntimeConfig.apiUrl);
  if (isLoggedIn && isApiUrl) {
    return { Authorization: `Bearer ${user.token}` };
  } else {
    return {};
  }
}

async function handleResponse(response) {
  console.log(response)
  const data = response.data;
  // check for error response
  if (!response.status || response.status < 200 || response.status >= 300) {
    if ([401, 403].includes(response.status) && userService.userValue) {
      // auto logout if 401 Unauthorized or 403 Forbidden response returned from API
      // userService.logout();
    }
    // get error message from data or default to response status text
    const error = data || response.statusText;
    // alertService.error(error, true);
    return Promise.reject(error);
  }
  if (JSON.stringify(data).includes("doctype")) {
    return Promise.reject("User not signed in.")
  }

  return data;
}
