import { fetchWrapper } from "../helpers/fetch-wrapper";

export const externalDbService = {
  getAllTableNames,
  deleteTable,
  getDataFromTable,
  deleteRow,
  createRow,
  updateRow,
};

async function getAllTableNames(id) {
    return await fetchWrapper.get(`external/table/${id}`);
}

async function deleteTable(tableName) {
  return await fetchWrapper.delete(`external/table/${tableName}`);
}

async function getDataFromTable(tableName) {
  return await fetchWrapper.get(`external/data/${tableName}`);
}

async function deleteRow(rowId) {
  return await fetchWrapper.delete(`external/data/${rowId}`);
}

async function createRow(newRowData) {
  return await fetchWrapper.post("external/data/add", { newRowData });
}

async function updateRow(updateRowData) {
  return await fetchWrapper.put("external/data/update", { updateRowData });
}
