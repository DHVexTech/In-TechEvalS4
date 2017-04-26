import { getAsync, postAsync, putAsync, deleteAsync } from '../helpers/apiHelper'

const endpoint = "/api/student";

class StudentApiService {
    constructor() {

    }

    async getStudentFilter(name)
    {
        return await getAsync(`${endpoint}/filter/${name}`);
    }


    async getStudentByClass(classId)
    {
        return await getAsync(`${endpoint}/getByClass/${classId}`);
    }

    async getStudentByNameAsync() {
        return await getAsync(endpoint);
    }

    async getStudentListAsync() {
        return await getAsync(endpoint);
    }

    async getStudentAsync(studentId) {
        return await getAsync(`${endpoint}/${studentId}`);
    }

    async createStudentAsync(model) {
        return await postAsync(endpoint, model);
    }

    async updateStudentAsync(model) {
        return await putAsync(`${endpoint}/${model.studentId}`, model);
    }

    async deleteStudentAsync(studentId) {
        return await deleteAsync(`${endpoint}/${studentId}`);
    }
}

export default new StudentApiService()