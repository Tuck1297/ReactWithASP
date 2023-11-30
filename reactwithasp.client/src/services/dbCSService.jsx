import { fetchWrapper } from "../helpers/fetch-wrapper";

export const dbCSService = {
    getDBNames,
    delete: _delete
}

// retrieves stored database names associated with signed in user
async function getDBNames() {
    return await fetchWrapper.get("cs/getall");
}

async function _delete(id) {
    return await fetchWrapper.delete(`cs/delete/${id}`);
}