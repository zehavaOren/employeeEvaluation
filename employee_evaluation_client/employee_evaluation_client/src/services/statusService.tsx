import EvaluationStatus from "../models/EvaluationStatus";
const BASE_URL = 'https://localhost:44379/api/Status';

export const statusService = {

    getAllStatuses: async () => {
        try {
            const response = await fetch(`${BASE_URL}/GetAllStatuses`);
            if (!response.ok) {
                throw new Error('Error fetching employees');
            }
            return await response.json() as EvaluationStatus[];
        } catch (error) {
            throw new Error(`Error fetching employees: ${error.message}`);
        }
    },

    getOutstandingEmployeeStatusCodes: async (schoolCode) => {
        try {
            const response = await fetch(`${BASE_URL}/GetOutstandingEmployeeStatusCodes/${schoolCode}`);
            if (!response.ok) {
                throw new Error('Error fetching employees');
            }
            return await response.json();

        } catch (error) {
            throw new Error(`Error fetching employees: ${error.message}`);
        }
    },

    getUpdatedGradesForOutstandingEmployee: async (employeeId) => {
        try {
            const response = await fetch(`${BASE_URL}/GetUpdatedGradesForOutstandingEmployee/${employeeId}`);
            if (!response.ok) {
                throw new Error('Error fetching employees');
            }
            return await response.json();

        } catch (error) {
            throw new Error(`Error fetching employees: ${error.message}`);
        }
    }
}