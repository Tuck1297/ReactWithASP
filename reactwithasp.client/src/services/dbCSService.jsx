import { fetchWrapper } from "../helpers/fetch-wrapper";

export const dbCSService = {
    getDBNames
}

// retrieves stored database names associated with signed in user
async function getDBNames() {
    return await fetchWrapper.get("cs/getall");
}