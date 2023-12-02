import { fetchWrapper } from "../helpers/fetch-wrapper";

export const dbCSService = {
  getDBNames,
  delete: _delete,
  saveDBConnection,
};

// retrieves stored database names associated with signed in user
async function getDBNames() {
  return await fetchWrapper.get("cs/getall");
}

async function _delete(id) {
  return await fetchWrapper.delete(`cs/delete/${id}`);
}

async function saveDBConnection(data) {
  const cs = {
    id: uuidv4(),
    dbName: data.csStringName,
    dbType: data.dropdown,
    dbConnectionString: `host=${data.host}; port=${data.port}; database=${data.dbName}; user id=${data.dbUserId}; password=${data.cspassword};`,
    userId: uuidv4()
};
  return await fetchWrapper.post("cs/create", cs);
}

function uuidv4() {
    return "10000000-1000-4000-8000-100000000000".replace(/[018]/g, c =>
      (c ^ crypto.getRandomValues(new Uint8Array(1))[0] & 15 >> c / 4).toString(16)
    );
  }