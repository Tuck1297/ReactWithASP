import { fetchWrapper } from "../helpers/fetch-wrapper";

export const externalDbService = {
  getAllTableNames,
  deleteTable,
  getDataFromTable,
  deleteRow,
  createRow,
  updateRow,
  updateTableInteracting,
};

async function getAllTableNames(id) {
  return await fetchWrapper.get(`external/table/${id}`);
}

async function updateTableInteracting(tableName, dbId) {
  return await fetchWrapper.put(
    `external/table/${dbId}/update-accessing`,
    tableName
  );
}

async function deleteTable(dbId, tableName) {
  return await fetchWrapper.delete(`external/table/${dbId}/${tableName}`);
}

// end-page is limit in sql
async function getDataFromTable(dbId, startPage, endPage) {
  return await fetchWrapper.get(
    `external/data/${dbId}?pagestart=${startPage}&pageend=${endPage}`
  );
}

async function deleteRow(dbId, jsonData) {
  return await fetchWrapper.delete(`external/data/${dbId}/delete`, convertStringsToNumbers(jsonData));
}

async function createRow(dbId, newData) {
  return await fetchWrapper.post(`external/data/${dbId}/add`, convertStringsToNumbers(newData));
}

async function updateRow(dbId, oldRowData, newRowData) {
  var updatedObject = {};

  // Loop through the original object
  for (var key in oldRowData) {
    // Update the key as needed
    var updatedKey = "old_" + key; // You can modify this line to update the key as per your requirement

    // Assign the value to the updated key in the new object
    updatedObject[updatedKey] = oldRowData[key];
  }
  return await fetchWrapper.put(`external/data/${dbId}/update`, { oldData: updatedObject, newData: convertStringsToNumbers(newRowData) });
}

function convertStringsToNumbers(obj) {
  var updatedObject = {};

  for (var key in obj) {
      if (obj.hasOwnProperty(key)) {
          if (isNaN(obj[key])) {
              updatedObject[key] = obj[key];
          } else {
              updatedObject[key] = Number(obj[key]);
          }
    }
  }
  return updatedObject;
}